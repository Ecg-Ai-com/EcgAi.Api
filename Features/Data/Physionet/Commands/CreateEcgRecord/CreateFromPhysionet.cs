namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

public class CreateFromPhysionet : Endpoint<CreateFromPhysionetRequest>
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public CreateFromPhysionet(ILogger<CreateFromPhysionet> logger, IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task HandleAsync(CreateFromPhysionetRequest req, CancellationToken ct)
    {
        var startTime = DateTime.UtcNow;
        _logger.LogInformation("TransactionId {TransactionId}, function started at {StartTime}",
            req.TransactionId, startTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffK"));

        try
        {
            await _mediator.Send(req, ct);
            await SendOkAsync("All loaded", ct);
            // await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
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
        Verbs(Http.POST);
        Routes("Physionet/CreateByRecordId");
        AllowAnonymous();
    }
}