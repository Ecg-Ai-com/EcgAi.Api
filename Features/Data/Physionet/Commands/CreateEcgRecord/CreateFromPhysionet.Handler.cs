using EcgAi.Api.Data.Database.Cosmos;
using EcgAi.Data.Api.Features.Physionet.Grpc;
using Grpc.Net.Client;

namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

public class CreateFromPhysionetHandler : IRequestHandler<Models.CreateFromPhysionetRequest>
{
    private readonly ILogger<CreateFromPhysionetHandler> _logger;
    private readonly IMapper _mapper;
    private readonly TrainingDbContext _dbContext;

    public CreateFromPhysionetHandler(ILogger<CreateFromPhysionetHandler> logger, IMapper mapper, TrainingDbContext dbContext)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }


    public async Task<Unit> Handle(Models.CreateFromPhysionetRequest request, CancellationToken cancellationToken)
    {
        List<Task<EcgRecord>> tasks = new();
        var startTime = DateTime.UtcNow;
        Console.WriteLine($"Starting with {request.RecordIds.Count}");
        foreach (var recordId in request.RecordIds)
        {
            var result = GetById(request.TransactionId, recordId, request.SampleRate);
            tasks.Add(result);
        }
        
        var processingTasks = tasks.Select(ProcessTask).ToList();
        await Task.WhenAll(processingTasks);
        var finishTime = DateTime.UtcNow;
        TimeSpan ts = (finishTime - startTime);
        Console.WriteLine($"Finished and took {ts.Seconds} seconds");
        return Unit.Value;
    }

    private async Task ProcessTask(Task<EcgRecord> finishedTask)
    {
        var result = await finishedTask;
        _dbContext.EcgRecords?.Add(result);
        await _dbContext.SaveChangesAsync();
        // Console.WriteLine(result.TransactionId);
        Console.WriteLine(result.RecordId);
        Console.WriteLine(result.DatabaseName);
    }

    private async Task<EcgRecord> GetById(string transactionId, int recordId, int sampleRate)
    {

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