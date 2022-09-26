namespace EcgAi.Api.Configuration;

public class TrainingDbConfiguration
{
    public const string SectionName = "TrainingDb";

    public string DatabaseName { get; set; } = string.Empty;
    public string EndpointUri { get; set; } = string.Empty;
    public string PrimaryKey { get; set; } = string.Empty;
}