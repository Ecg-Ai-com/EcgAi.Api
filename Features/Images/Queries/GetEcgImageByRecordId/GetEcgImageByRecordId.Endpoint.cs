// ReSharper disable PositionalPropertyUsedProblem
namespace EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;

public class GetEcgImageByRecordId : Endpoint<GetEcgImageByRecordIdRequest,CreateEcgPlotResponse>
{
    private readonly ILogger<GetEcgImageByRecordId> _logger;
    private readonly IMediator _mediator;

    public GetEcgImageByRecordId(ILogger<GetEcgImageByRecordId> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public override async Task HandleAsync(GetEcgImageByRecordIdRequest req, CancellationToken ct)
    {
        try
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var op = Operation.Begin(
                       "Images/GetEcgImageByRecordId endpoint called with a TransactionId {0} with " +
                       "the RecordId {1} from the Database {2}", req.TransactionId, req.RecordId,
                       req.DatabaseName!))
            {
                var response = await _mediator.Send(req, ct);
                await SendOkAsync(response, ct);
                op.Complete();
            }
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning("Error message: {0}", e.Message);
            ThrowError(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Error message: {0}", e.Message);
            ThrowError(e.Message);
        }
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("Images/GetEcgImageByRecordId");
        AllowAnonymous();
    }
}