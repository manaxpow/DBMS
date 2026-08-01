using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductStatus
{
    Draft,
    Published,
    Archived
}
