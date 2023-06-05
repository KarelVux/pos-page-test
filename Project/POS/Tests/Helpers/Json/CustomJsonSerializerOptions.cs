using System.Text.Json;

namespace Tests.Helpers.Json;

public class CustomJsonSerializerOptions
{
    public readonly JsonSerializerOptions CamelCaseJsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}