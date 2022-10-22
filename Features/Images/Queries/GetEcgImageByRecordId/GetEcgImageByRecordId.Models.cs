// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;

public class GetEcgImageByRecordIdRequest :IRequest<CreateEcgPlotResponse>
{
    public string TransactionId { get; init; } = Guid.NewGuid().ToString();
    public int RecordId { get; init; }
    public string? DatabaseName { get; init; }
    public int SampleRate { get; init; }
    public Artifact Artifact { get; init; }
    public ColorStyle ColorStyle { get; init; }
    public string? FileName { get; init; }
    public bool ShowGrid { get; init; }
}

public enum Artifact
{
    // ArtifactUnspecified = 0,
    Salt = 1,
    Pepper = 2,
    SaltAndPepper = 3,
    Poisson = 4,
    Speckle = 5,
    None = 6
}

public enum ColorStyle
{
    BlackAndWhite = 1,
    Color = 2,
    Mask = 3,
    GreyScale = 4
}

public class EcgPlotRequest
{
    public string? TransactionId { get; init; }
    public string? RecordName { get; init; }
    public int SampleRate { get; init; }
    public ColorStyle ColorStyle { get; init; }
    public bool ShowGrid { get; init; }
    public Artifact Artifact { get; init; }
    public List<EcgLead>? EcgLeads { get; init; }
    public string? FileName { get; init; }
}

public class CreateEcgPlotResponse
{
    public string? TransactionId { get; set; }
    public string? RecordName { get; set; }
    public string? FileName { get; set; }
    public string? FileExtension { get; set; }
    public string? Image { get; set; }
}

