namespace diploma_strava_ex_api.Models
{
    namespace StravaApi.Models
    {
        public class SplitMetric
        {
            public float Distance { get; set; }
            public int ElapsedTime { get; set; }
            public float ElevationDifference { get; set; }
            public int MovingTime { get; set; }
            public int Split { get; set; }
            public float AverageSpeed { get; set; }
            public int PaceZone { get; set; }
        }
    }

}
