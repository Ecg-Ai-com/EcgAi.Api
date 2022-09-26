// ReSharper disable MemberCanBePrivate.Global
namespace EcgAi.Api.Configuration;

public class HostConfiguration
{
    public static string SectionName => "Host";
    public string Url { get; set; } = string.Empty;
    public int Port { get; set; } = 0;

    public override string ToString()
    {
        return $"https://{Url}:{Port}";
    }
}