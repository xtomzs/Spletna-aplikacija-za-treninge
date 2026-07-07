namespace diploma_strava_ex_api.Models
{
    using System;
    using System.Collections.Generic;
    using diploma_strava_ex_api.Models.Shared;

    namespace StravaApi.Models
    {
        public class SummarySegmentEffort
        {
            public long Id { get; set; }
            public int ResourceState { get; set; }
            public string Name { get; set; }
            public MetaActivity Activity { get; set; }
            public SummaryAthlete Athlete { get; set; }
            public int ElapsedTime { get; set; }
            public int MovingTime { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime StartDateLocal { get; set; }
            public float Distance { get; set; }
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
            public float? AverageCadence { get; set; }
            public bool DeviceWatts { get; set; }
            public float? AverageWatts { get; set; }
            public MetaSegment Segment { get; set; }
            public int? KomRank { get; set; }
            public int? PrRank { get; set; }
            public List<object> Achievements { get; set; }  // depending on how achievements are modeled
            public bool Hidden { get; set; }
        }
    }

}
