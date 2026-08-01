using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductType
{
    Physical,
    Digital,
    Service
}
