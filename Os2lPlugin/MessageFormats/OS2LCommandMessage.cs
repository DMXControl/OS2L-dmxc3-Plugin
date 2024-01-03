using System.Text.Json.Serialization;

namespace Os2lPlugin.MessageFormats
{
    public struct OS2LCommandMessage
    {
        [JsonPropertyName("evt")]
        public string Event { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("param")]
        public double Parameter { get; set; }
    }
}
