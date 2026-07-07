namespace diploma_strava_ex_api.Models.DBModels
{
    public class PlannerItem
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime PlanDate { get; set; }
        public float Distance { get; set; }
        public float Elevation { get; set; }
    }
}
