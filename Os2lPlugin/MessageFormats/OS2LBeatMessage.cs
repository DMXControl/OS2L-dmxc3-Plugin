using System.Text.Json.Serialization;

namespace Os2lPlugin.MessageFormats
{
    public struct OS2LBeatMessage
    {
        [JsonPropertyName("evt")]
        public string Event { get; set; }

        [JsonPropertyName("change")]
        public bool Change { get; set; }

        [JsonPropertyName("pos")]
        public int Position { get; set; }

        [JsonPropertyName("bpm")]
        public double Bpm { get; set; }

        [JsonPropertyName("strength")]
        public double Strength { get; set; }
    }
}
