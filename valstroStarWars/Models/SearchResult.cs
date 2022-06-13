namespace ValstroStarWars.Models;

using System.Text.Json.Serialization;

public class SearchResult
{
    [JsonPropertyName("page")]
    public int Page {get; set;}
    [JsonPropertyName("resultCount")]
    public int ResultCount {get; set;}
    [JsonPropertyName("name")]
    public string Name {get; set;}
    [JsonPropertyName("films")]
    public string Films {get; set;}
    
    
    
}