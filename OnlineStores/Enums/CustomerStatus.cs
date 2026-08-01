using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerStatus
{
    Active,
    Inactive,
    Blocked
}
