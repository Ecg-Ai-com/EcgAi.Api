

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace EcgAi.Api.Features.Data.Physionet.GetByRecordId;

public class Request : IRequest<Response>
{
    public string TransactionId { get; init; } = Guid.NewGuid().ToString();
    public int RecordId { get; init; }
    public int SampleRate { get; init; }
}

public class Response
{
    public EcgRecord? EcgRecord { get; init; }
    public string? TransactionId { get; init; }
}