using System.Diagnostics.CodeAnalysis;
using EcgAi.Data.Api.Features.Physionet.Grpc;
using Grpc.Core;
using Grpc.Net.Client;

namespace EcgAi.Api.Features.Data.Physionet.GetByRecordId;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class Handler : IRequestHandler<Request, Response>
{
    private readonly ILogger<Handler> _logger;
    private readonly IMapper _mapper;

    public Handler(ILogger<Handler> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    // public async Task<EcgRecord> Get(string transactionId, int recordId, int sampleRate)
    // {
    //     var grpcRequest = new GetByIdRequest
    //     {
    //         RecordId = recordId,
    //         SampleRate = sampleRate,
    //         TransactionId = transactionId
    //     };
    //     var httpHandler = new HttpClientHandler
    //     {
    //         ServerCertificateCustomValidationCallback =
    //             HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    //     };
    //     try
    //     {
    //         using var channel = GrpcChannel.ForAddress("http://localhost:45210",
    //             new GrpcChannelOptions {HttpHandler = httpHandler});
    //         var client = new GetByIdService.GetByIdServiceClient(channel);
    //         var response = await client.GetByIdAsync(grpcRequest);
    //         var t = response.TransactionId;
    //         return new EcgRecord(response.Ecg.RecordId, response.Ecg.RecordName, "", 1, new List<EcgLead>());
    //     }
    //     catch (RpcException e)
    //     {
    //         _logger.LogError("RpcException");
    //         Console.WriteLine("RpcException");
    //         throw;
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError("Exception");
    //
    //         Console.WriteLine("Exception");
    //         throw;
    //     }
    // }

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        
        var startTime = DateTime.UtcNow;
        _logger.LogInformation("TransactionId {TransactionId}, function started at {StartTime}",
            request.TransactionId, startTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));

        try
        {
            var response = await GetById(request);
            return response;
        }
        catch (RpcException e)
        {
            _logger.LogError("RpcException {Exception}", e.ToString());
            //TODO fix throw exception
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError("RpcException {Exception}", e.ToString());
            //TODO fix throw exception
            throw;
        }
        finally
        {
            var finishTime = DateTime.UtcNow;
            TimeSpan ts = (finishTime - startTime);
            _logger.LogInformation(
                "TransactionId {TransactionId}, function finished at {FinishTime} and took {TimeSpan} ms",
                request.TransactionId, finishTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"), ts.Milliseconds);
        }
    }

    private async Task<Response> GetById(Request request)
    {
        var grpcRequest = _mapper.Map<GetByIdRequest>(request);
        // var client = GetServiceClient();
        var httpHandler = GetHttpClientHandler();

        using var channel = GrpcChannel.ForAddress("http://localhost:45210",
            new GrpcChannelOptions {HttpHandler = httpHandler});
        var client = new GetByIdService.GetByIdServiceClient(channel);
        var grpcResponse = await client.GetByIdAsync(grpcRequest);

        var response = _mapper.Map<Response>(grpcResponse);
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

    // private static GetByIdService.GetByIdServiceClient GetServiceClient()
    // {
    //     var httpHandler = new HttpClientHandler
    //     {
    //         ServerCertificateCustomValidationCallback =
    //             HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    //     };
    //     // try
    //     // {
    //     using var channel = GrpcChannel.ForAddress("http://localhost:45210",
    //         new GrpcChannelOptions {HttpHandler = httpHandler});
    //     var client = new GetByIdService.GetByIdServiceClient(channel);
    //     return client;
    // }
}