using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerMemberRole
{
    Owner,
    Admin,
    Member
}
