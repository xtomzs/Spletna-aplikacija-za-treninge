using diploma_strava_ex_api.Models.Shared;

namespace diploma_strava_ex_api.Models
{

    public class Lap
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
        public float TotalElevationGain { get; set; }
        public float AverageSpeed { get; set; }
        public float MaxSpeed { get; set; }
        public float? AverageCadence { get; set; }
        public bool DeviceWatts { get; set; }
        public float? AverageWatts { get; set; }
        public int LapIndex { get; set; }
        public int Split { get; set; }
    }
}


