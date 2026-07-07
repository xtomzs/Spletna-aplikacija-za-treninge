namespace diploma_strava_ex_api.Models
{
    using System.Collections.Generic;

    namespace StravaApi.Models
    {
        public class MetaSegment
        {
            public long Id { get; set; }
            public int ResourceState { get; set; }
            public string Name { get; set; }
            public string ActivityType { get; set; }
            public float Distance { get; set; }
            public float AverageGrade { get; set; }
            public float MaximumGrade { get; set; }
            public float ElevationHigh { get; set; }
            public float ElevationLow { get; set; }
            public List<float> StartLatLng { get; set; }
            public List<float> EndLatLng { get; set; }
            public int ClimbCategory { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public bool Private { get; set; }
            public bool Hazardous { get; set; }
            public bool Starred { get; set; }
        }
    }

}
