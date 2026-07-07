using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace diploma_strava_ex_api.Migrations
{
    /// <inheritdoc />
    public partial class InsertPlannerItemsForCurrentMOnth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            var rng = new Random();
            var now = DateTime.UtcNow;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var lastDay = firstDay.AddDays(daysInMonth - 1);

            for (int i = 0; i < daysInMonth; i++)
            {
                var date = firstDay.AddDays(i);
                var distance = rng.Next(10000, 20001);
                var elevation = rng.Next(200, 2001);

                var baseDesc = distance > 15000 ? "long training run" : "short training run";

                var suffixes = new[] { "tempo", "easy", "intervals", "progression", "fartlek", "recovery", "threshold" };
                var suffix = suffixes[rng.Next(suffixes.Length)];
                var desc = $"{baseDesc} ({suffix} #{rng.Next(1, 1000)})";

                var descEscaped = desc.Replace("'", "''");

                var sql = $"INSERT INTO PlannerItems (Description, PlanDate, Distance, Elevation) VALUES ('{descEscaped}', '{date:yyyy-MM-dd}', {distance}, {elevation});";

                migrationBuilder.Sql(sql);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.UtcNow;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            var lastDay = firstDay.AddDays(daysInMonth - 1);

            // Delete all PlannerItems for the current month that look like our seeded descriptions
            var sql = $"DELETE FROM PlannerItems WHERE PlanDate >= '{firstDay:yyyy-MM-dd}' AND PlanDate <= '{lastDay:yyyy-MM-dd}' AND Description LIKE '%training run%';";
            migrationBuilder.Sql(sql);
        }
    }
}
