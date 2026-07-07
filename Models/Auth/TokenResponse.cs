using diploma_strava_ex_api.Models.Shared;
using System.Text.Json.Serialization;

namespace diploma_strava_ex_api.Models.Auth
{
    public class TokenResponse
    {
        [JsonPropertyName("token_type")]
        public required string Token_type { get; set; }
        [JsonPropertyName("expires_at")]
        public int Expires_at { get; set; }
        [JsonPropertyName("expires_in")]
        public int Expires_in { get; set; }
        [JsonPropertyName("refresh_token")]
        public required string Refresh_token { get; set; }
        [JsonPropertyName("access_token")]
        public required string Access_token { get; set; }
        [JsonPropertyName("athlete")]
        public SummaryAthlete Athlete { get; set; }

    }
}
