namespace EcgAi.Api.Features.Data.Physionet.GetByRecordId;

public class GetByRecordId : Endpoint<Request, Response>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public GetByRecordId(ILogger<GetByRecordId> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var startTime = DateTime.UtcNow;
        _logger.LogInformation("TransactionId {TransactionId}, function started at {StartTime}",
            req.TransactionId, startTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));

        try
        {
            var response = await _mediator.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
        finally
        {
            var finishTime = DateTime.UtcNow;
            TimeSpan ts = (finishTime - startTime);
            _logger.LogInformation(
                "TransactionId {TransactionId}, function finished at {FinishTime} and took {TimeSpan} ms",
                req.TransactionId, finishTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"), ts.Milliseconds);
        }

    }


    public override void Configure()
    {
        Verbs(Http.GET,Http.POST);
        Routes("GetByRecordId");
        AllowAnonymous();
    }
}

// public class MySummary : Summary<GetByRecordId>
// {
//     public MySummary()
//     {
//         Summary = "short summary goes here";
//         Description = "long description goes here";
//         // ExampleRequest = new Response() {...};
//         Response<Response>(200, "ok response with body");
//         Response<ErrorResponse>(400, "validation failure");
//         Response(404, "account not found");
//     }
// }