using System.Text.Json.Serialization;

namespace diploma_strava_ex_api.Models.Dashboard
{
    public class ActivityStats
    {
        [JsonPropertyName("biggest_ride_distance")]
        public double BiggestRideDistance { get; set; }

        [JsonPropertyName("biggest_climb_elevation_gain")]
        public double BiggestClimbElevationGain { get; set; }

        [JsonPropertyName("recent_ride_totals")]
        public ActivityTotal RecentRideTotals { get; set; }

        [JsonPropertyName("recent_run_totals")]
        public ActivityTotal RecentRunTotals { get; set; }

        [JsonPropertyName("recent_swim_totals")]
        public ActivityTotal RecentSwimTotals { get; set; }

        [JsonPropertyName("ytd_ride_totals")]
        public ActivityTotal YtdRideTotals { get; set; }

        [JsonPropertyName("ytd_run_totals")]
        public ActivityTotal YtdRunTotals { get; set; }

        [JsonPropertyName("ytd_swim_totals")]
        public ActivityTotal YtdSwimTotals { get; set; }

        [JsonPropertyName("all_ride_totals")]
        public ActivityTotal AllRideTotals { get; set; }

        [JsonPropertyName("all_run_totals")]
        public ActivityTotal AllRunTotals { get; set; }

        [JsonPropertyName("all_swim_totals")]
        public ActivityTotal AllSwimTotals { get; set; }
    }
    public class ActivityTotal
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("distance")]
        public double Distance { get; set; } // in meters

        [JsonPropertyName("moving_time")]
        public int MovingTime { get; set; } // in seconds

        [JsonPropertyName("elapsed_time")]
        public int ElapsedTime { get; set; } // in seconds

        [JsonPropertyName("elevation_gain")]
        public double ElevationGain { get; set; } // in meters

        [JsonPropertyName("achievement_count")]
        public int AchievementCount { get; set; }
    }

}
