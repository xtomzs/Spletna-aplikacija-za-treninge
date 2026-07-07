using Microsoft.EntityFrameworkCore;

namespace diploma_strava_ex_api.Models.DBModels
{
    public class PlannerContext : DbContext
    {
        public DbSet<PlannerItem> PlannerItems { get; set; }
        public string DbPath { get; set; }

        public PlannerContext(DbContextOptions<PlannerContext> options) : base(options)
        {            
            DbPath = "./planner.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
