using System.Text.Json.Serialization;

namespace Os2lPlugin.MessageFormats
{
    public struct OS2LButtonMessage
    {
        [JsonPropertyName("evt")]
        public string Event { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("page")]
        public string Page { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
