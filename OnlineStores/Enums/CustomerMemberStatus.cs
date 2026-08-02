using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerMemberStatus
{
    Invited,
    Active,
    Disabled
}
