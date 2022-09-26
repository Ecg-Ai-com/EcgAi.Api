namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

public class Models
{
    public class CreateFromPhysionetRequest : IRequest
    {
        public string TransactionId { get; init; } = Guid.NewGuid().ToString();
        public List<int> RecordIds { get; init; } =new List<int>();
        public int SampleRate { get; init; }
    }
}