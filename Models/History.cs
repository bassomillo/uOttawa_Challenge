using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WordleBackEndApi.Models;

[BsonIgnoreExtraElements]
public class History
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? id { get; set; } = null!;

    public string key { get; set; } = null!;

    public string word { get; set; } = null!;

     public string guess { get; set; } = null!;

    public string date { get; set; } = null!;


}