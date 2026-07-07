using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using diploma_strava_ex_api.Utils;
using System.Text.Json.Serialization;
using diploma_strava_ex_api.Models.Dashboard;
using diploma_strava_ex_api.Extensions;
using Microsoft.Extensions.Caching.Memory;
using diploma_strava_ex_api.Models.Shared;
using diploma_strava_ex_api.Models;
using diploma_strava_ex_api.Models.DBModels;

namespace diploma_strava_ex_api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]/[action]")]
    public partial class StravaDashboardController : BaseStravaController
    {
        public StravaDashboardController(IHttpClientFactory httpClientFactory, IWebRequestHelper requestHelper, IMemoryCache memoryCache, IConfiguration configuration) : base(httpClientFactory, requestHelper, memoryCache, configuration)
        {
        }

        [Authorize(Policy = "strava-allowed")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]
        public async Task<string> getActivitiesSummaryAsync()
        {
            var url = "https://www.strava.com/api/v3/athlete/activities";
            var lastWeek = DateTimeOffset.Now.AddDays(-7).ToUnixTimeSeconds().ToString();
            var uri = QueryHelpers.AddQueryString(url, new Dictionary<string, string?>
            {
                { "after", lastWeek}/*, {"page", "1"}*//*, {"per_page","10" }*/
            });

            return await GetRequest(uri, "getActivitiesSummaryAsync");
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]
        public async Task<string> getGraphDataAsync()
        {
            var url = "https://www.strava.com/api/v3/athlete/activities";
            var dateStart = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            //var data = new Dictionary<string, double>();
            var data = new List<GraphNode>();
            var weeks = new List<Week>();
            var distanceNode = new GraphNode
            {
                ValueType = "Distance",
                Values = new List<GraphItem>()
            };

            var totalElevationGainNode = new GraphNode
            {
                ValueType = "TotalElevationGain",
                Values = new List<GraphItem>()
            };

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
                var res = await GetRequest(uri, weekDate);
                var responseData = JsonSerializer.Deserialize<SummaryActivity[]>(res);

                distanceNode.Values.Add(new GraphItem
                {
                    Index = weeks.IndexOf(week),
                    Week = weekDate,
                    Value = responseData.Where(x => x.SportType == "TrailRun" || x.SportType == "Run" || x.SportType == "Hike" || x.SportType == "Walk").Sum(x => x.Distance / 10),
                    ValueType = "Distance"

                });
                totalElevationGainNode.Values.Add(new GraphItem
                {
                    Index = weeks.IndexOf(week),
                    Week = weekDate,
                    Value = responseData.Where(x => x.SportType == "TrailRun" || x.SportType == "Run" || x.SportType == "Hike" || x.SportType == "Walk").Sum(x => x.TotalElevationGain),
                    ValueType = "TotalElevationGain"
                });
            }
            distanceNode.Values.Reverse();
            totalElevationGainNode.Values.Reverse();
            data.Add(distanceNode);
            data.Add(totalElevationGainNode);

            return JsonSerializer.Serialize(data.ToArray());
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]
        public async Task<string> getAthleteStatsAsync()
        {
            var url = $"https://www.strava.com/api/v3/athletes/{RequestHelper.getAthleteIdFromRequest(HttpContext)}/stats";

            return await GetRequest(url, "athleteStats");
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]
        public async Task<string> getAthleteGearSummaryAsync()
        {
            var athleteUrl = $"https://www.strava.com/api/v3/athlete";           
            var athlete = await GetRequest<DetailedAthlete>(athleteUrl);


            //var res = new List<DetailedGear>();
            //foreach (var shoe in athlete.Shoes)
            //{
            //    var gear = await GetRequest<DetailedGear>($"https://www.strava.com/api/v3/gear/{shoe.Id}", "shoe" + shoe.Id.ToString());
            //    res.Add(gear);
            //}

            return JsonSerializer.Serialize(athlete.Shoes.ToArray());
        }
    }
}
