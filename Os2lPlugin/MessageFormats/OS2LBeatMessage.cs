using System.Text.Json.Serialization;

namespace Os2lPlugin.MessageFormats
{
    public readonly struct OS2LBeatMessage
    {
        [JsonConstructor]
        public OS2LBeatMessage(string evt, bool change, long pos, double bpm, double strength) => (Event, Change, Position, BPM, Strength) = (evt, change, pos, bpm, strength);

        [JsonPropertyName("evt")]
        public readonly string Event { get; } = "";

        [JsonPropertyName("change")]
        public readonly bool Change { get; } = false;

        [JsonPropertyName("pos")]
        public readonly long Position { get; } = 0;

        [JsonPropertyName("bpm")]
        public readonly double BPM { get; } = 0;

        [JsonPropertyName("strength")]
        public readonly double Strength { get; } = 0;
    }
}
