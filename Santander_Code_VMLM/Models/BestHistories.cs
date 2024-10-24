using System.Text.Json.Serialization;
using Santander_Code_VMLM.Helpers;

namespace Santander_Code_VMLM.Models
{
    public class BestHistories
    {
        [JsonPropertyName("by")]
        public string? By { get; set; }

        [JsonPropertyName("descendants")]
        public int Descendants { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeOffSetConverter))]
        public DateTimeOffset? Time { get; set; }

        [JsonPropertyName("score")]
        public int? Score { get; set; }

        [JsonPropertyName("kids")]
        public int[]? Kids { get; set; }
    }
}
