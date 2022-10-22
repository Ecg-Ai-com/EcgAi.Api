namespace EcgAi.Api.Configuration;

public class EcgAiDrawingAzureApiConfiguration
{
    public const string SectionName = "EcgAiDrawingAzureApi";

    public string BaseAddress { get; init; } = string.Empty;
    public string FunctionName { get; init; } = string.Empty;
    public string FunctionKey { get; init; } = string.Empty;
}