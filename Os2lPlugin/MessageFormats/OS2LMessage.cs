using System.Text.Json.Serialization;

namespace Os2lPlugin.MessageFormats
{
    public readonly struct OS2LMessage
    {
        [JsonConstructor]
        public OS2LMessage(string evt) => (Event) = (evt);

        [JsonPropertyName("evt")]
        public readonly string Event { get; } = "";
    }
}
