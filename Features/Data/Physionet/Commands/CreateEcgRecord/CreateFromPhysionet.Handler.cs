using EcgAi.Api.Data.Database.Cosmos;
using EcgAi.Data.Api.Features.Physionet.Grpc;
using Grpc.Net.Client;

// ReSharper disable PositionalPropertyUsedProblem

namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

// ReSharper disable once UnusedType.Global
public class CreateFromPhysionetHandler : IRequestHandler<CreateFromPhysionetRequest>
{
    private readonly ILogger<CreateFromPhysionetHandler> _logger;

    private readonly IMapper _mapper;

    // private readonly TrainingDbContext _dbContext;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SemaphoreSlim _semaphoreSlim;

    public CreateFromPhysionetHandler(ILogger<CreateFromPhysionetHandler> logger,
        IMapper mapper,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _mapper = mapper;
        _scopeFactory = scopeFactory;
        _semaphoreSlim = new SemaphoreSlim(200);
    }


    public async Task<Unit> Handle(CreateFromPhysionetRequest request, CancellationToken cancellationToken)
    {
        var recordIds = request.RecordIds;
        var tasks = new List<Task<bool>>();
        var startTime = DateTime.UtcNow;

        tasks = request.RecordIds.Select(async recordId =>
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                return await ProcessRecord(request.TransactionId, recordId, request.SampleRate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }).ToList();
        await Task.WhenAll(tasks);
        var finishTime = DateTime.UtcNow;
        TimeSpan ts = (finishTime - startTime);
        Console.WriteLine($"Finished and took {ts.Seconds} seconds");

        return Unit.Value;
    }

    private async Task<bool> ProcessRecord(string transactionId, int recordId, int sampleRate)
    {
        var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();
        var existingRecord = dbContext.EcgRecords.Count(c => c.RecordId == recordId && c.SampleRate == sampleRate);
        if (existingRecord == 0)
        {
            var result = await GetById(transactionId, recordId, sampleRate);
            await dbContext.EcgRecords.AddAsync(result);
            await dbContext.SaveChangesAsync();
            return true;
            // return result;
        }

        if (existingRecord > 1)
        {
            var items = dbContext.EcgRecords.Where(
                c => c.RecordId == recordId && c.SampleRate == sampleRate).ToList();
            _logger.LogInformation("{0} records was already in the database: TransactionId {1}, " +
                                   "RecordId {2}", items.Count, transactionId, recordId);
            var recordCountToKeep = 1;
            foreach (var ecgRecord in items)
            {
                if (recordCountToKeep < existingRecord)
                {
                    dbContext.EcgRecords.Remove(ecgRecord);
                    recordCountToKeep++;
                }
            }

            await dbContext.SaveChangesAsync();
        }

        _logger.LogInformation("Record was already in the database: TransactionId {0}, " +
                               "RecordId {1}", transactionId, recordId);
        return false;
    }

    // private async Task ProcessTask(Task<EcgRecord> finishedTask)
    // {
    //     try
    //     {
    //         var result = await finishedTask;
    //         using (var scope = _scopeFactory.CreateScope())
    //         {
    //             var dbContext = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();
    //             await dbContext.EcgRecords.AddAsync(result);
    //             await dbContext.SaveChangesAsync();
    //         }
    //
    //         // await _dbContext.EcgRecords.AddAsync(result);
    //         // await _dbContext.SaveChangesAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         // throw;
    //     }
    //     // Console.WriteLine(result.TransactionId);
    //     // Console.WriteLine(result.RecordId);
    //     // Console.WriteLine(result.DatabaseName);
    // }

    private async Task<EcgRecord> GetById(string transactionId, int recordId, int sampleRate)
    {
        _logger.LogInformation("Getting record from GRPC: TransactionId {0}, " +
                               "RecordId {1}", transactionId, recordId);
        var grpcRequest = new GetByIdRequest
        {
            TransactionId = transactionId,
            RecordId = recordId,
            SampleRate = sampleRate,
        };
        var httpHandler = GetHttpClientHandler();

        using var channel = GrpcChannel.ForAddress("http://localhost:45210",
            new GrpcChannelOptions {HttpHandler = httpHandler});
        var client = new GetByIdService.GetByIdServiceClient(channel);
        var grpcResponse = await client.GetByIdAsync(grpcRequest);

        var response = _mapper.Map<EcgRecord>(grpcResponse);
        _logger.LogInformation("Got record from GRPC: TransactionId {0}, " +
                               "RecordId {1}", transactionId, recordId);
        return response;
    }

    private static HttpClientHandler GetHttpClientHandler()
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        return httpHandler;
    }
}