namespace EcgAi.Api;

public class ExampleEndPoint : EndpointWithoutRequest
{
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new
        {
            message = "Hello World"
        }, cancellation: ct);
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("example");
        AllowAnonymous();
    }
}