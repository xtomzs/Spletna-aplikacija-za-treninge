using System.Text.Json.Serialization;

namespace diploma_strava_ex_api.Models.Dashboard
{
    public class SummaryActivity
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("upload_id")]
        public long UploadId { get; set; }

        [JsonPropertyName("athlete")]
        public MetaAthlete Athlete { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("distance")]
        public float Distance { get; set; }

        [JsonPropertyName("moving_time")]
        public int MovingTime { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int ElapsedTime { get; set; }

        [JsonPropertyName("total_elevation_gain")]
        public float TotalElevationGain { get; set; }

        [JsonPropertyName("elev_high")]
        public float ElevHigh { get; set; }

        [JsonPropertyName("elev_low")]
        public float ElevLow { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } // Deprecated. Prefer SportType

        [JsonPropertyName("sport_type")]
        public string SportType { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("start_date_local")]
        public DateTime StartDateLocal { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        //[JsonPropertyName("start_latlng")]
        //public Dictionary<float,float> StartLatLng { get; set; }

        //[JsonPropertyName("end_latlng")]
        //public Dictionary<float, float> EndLatLng { get; set; }

        [JsonPropertyName("achievement_count")]
        public int AchievementCount { get; set; }

        [JsonPropertyName("kudos_count")]
        public int KudosCount { get; set; }

        [JsonPropertyName("comment_count")]
        public int CommentCount { get; set; }

        [JsonPropertyName("athlete_count")]
        public int AthleteCount { get; set; }

        [JsonPropertyName("photo_count")]
        public int PhotoCount { get; set; }

        [JsonPropertyName("total_photo_count")]
        public int TotalPhotoCount { get; set; }

        [JsonPropertyName("map")]
        public PolylineMap Map { get; set; }

        [JsonPropertyName("trainer")]
        public bool Trainer { get; set; }

        [JsonPropertyName("commute")]
        public bool Commute { get; set; }

        [JsonPropertyName("manual")]
        public bool Manual { get; set; }

        [JsonPropertyName("private")]
        public bool Private { get; set; }

        [JsonPropertyName("flagged")]
        public bool Flagged { get; set; }

        [JsonPropertyName("workout_type")]
        public int? WorkoutType { get; set; }

        [JsonPropertyName("upload_id_str")]
        public string UploadIdStr { get; set; }

        [JsonPropertyName("average_speed")]
        public float AverageSpeed { get; set; }

        [JsonPropertyName("max_speed")]
        public float MaxSpeed { get; set; }

        [JsonPropertyName("has_kudoed")]
        public bool HasKudoed { get; set; }

        [JsonPropertyName("hide_from_home")]
        public bool HideFromHome { get; set; }

        [JsonPropertyName("gear_id")]
        public string GearId { get; set; }

        [JsonPropertyName("kilojoules")]
        public float? Kilojoules { get; set; }

        [JsonPropertyName("average_watts")]
        public float? AverageWatts { get; set; }

        [JsonPropertyName("device_watts")]
        public bool? DeviceWatts { get; set; }

        [JsonPropertyName("max_watts")]
        public int? MaxWatts { get; set; }

        [JsonPropertyName("weighted_average_watts")]
        public int? WeightedAverageWatts { get; set; }
    }

}

