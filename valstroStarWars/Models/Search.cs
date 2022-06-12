namespace ValstroStarWars.Models;

using System.Text.Json.Serialization;

    public class Search 
    {
        [JsonPropertyName("query")]
        public string Query {get; set;}
    }