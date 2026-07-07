using diploma_strava_ex_api.Extensions;
using diploma_strava_ex_api.Models.Dashboard;
using diploma_strava_ex_api.Models.DBModels;
using diploma_strava_ex_api.Models.Shared;
using diploma_strava_ex_api.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace diploma_strava_ex_api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]/[action]")]
    public class StravaPlannerController : BaseStravaController
    {
        private readonly PlannerContext _dbContext;
        public StravaPlannerController(IHttpClientFactory httpClientFactory, IWebRequestHelper requestHelper, IMemoryCache memoryCache, IConfiguration configuration, PlannerContext dbContext) : base(httpClientFactory, requestHelper, memoryCache, configuration)
        {
            _dbContext = dbContext;
        }

        [Authorize(Policy = "strava-allowed")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "strava-allowed")]
        public async Task<string> GetPlannedDataAsync()
        {
            var dateTimeNow = DateTime.Now;
            var firstDayOfMonth = new DateTime(dateTimeNow.Year, dateTimeNow.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            var r = _dbContext.PlannerItems.Where(x => x.PlanDate >= firstDayOfMonth && x.PlanDate <= lastDayOfMonth);
            
            return JsonSerializer.Serialize(r);
        }

        [Authorize(Policy = "strava-allowed")]
        public async Task<string> GetRealizationDataAsync()
        {
            var url = "https://www.strava.com/api/v3/athlete/activities";
            var dateStart = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var weeks = new List<Week>();
            var data = new List<RealizationItem>();

            for (var i = 0; i < 10; i++)
            {
                var startofWeek = dateStart.AddDays(-i * 7);
                var week = new Week
                {
                    StartOfWeek = startofWeek,
                    EndOfWeek = startofWeek.AddDays(6),
                };

                weeks.Add(week);
            }

            foreach (var week in weeks)
            {
                var uri = QueryHelpers.AddQueryString(url, new Dictionary<string, string?>
                {
                    { "before", ((DateTimeOffset)week.EndOfWeek.AddDays(1)).ToUnixTimeSeconds().ToString()},
                    { "after", ((DateTimeOffset)week.StartOfWeek).ToUnixTimeSeconds().ToString()}
                });
                var weekDate = week.StartOfWeek.Date.ToString("d") + " - " + week.EndOfWeek.Date.ToString("d");
                var res = await GetRequest(uri, "Realization" + weekDate);
                var responseData = JsonSerializer.Deserialize<SummaryActivity[]>(res);

                data.AddRange(responseData.Select(x => new RealizationItem
                {
                    ActivityDate = x.StartDate,
                    ActivityDistance = x.Distance,
                    ActivityElevation = x.TotalElevationGain,
                    ActivityId = x.Id,
                    ActivityName = x.Name
                }));
            }

            return JsonSerializer.Serialize(data);
        }

        [Authorize(Policy = "strava-allowed")]
        [HttpPost]
        public async Task<PlannerItem> SavePlanAsync([FromBody] PlannerItem item)
        {
            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return item;
        }

        [Authorize(Policy = "strava-allowed")]
        public async Task<PlannerItem> GetPlanDetailsAsync(long id)
        {
            var item = await _dbContext.FindAsync<PlannerItem>(id);

            return item;
        }

        private class RealizationItem
        {
            public long ActivityId { get; set; }
            public DateTime ActivityDate { get; set; }
            public string ActivityName { get; set; }
            public float ActivityDistance { get; set; }
            public float ActivityElevation { get; set; }
        }
    }
}
