using diploma_strava_ex_api.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace diploma_strava_ex_api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]/[action]")]
    public class StravaClubsController : BaseStravaController
    {
        public StravaClubsController(IHttpClientFactory httpClientFactory, IWebRequestHelper requestHelper, IMemoryCache memoryCache, IConfiguration configuration) : base(httpClientFactory, requestHelper, memoryCache, configuration)
        {
        }
        
        [Authorize(Policy = "strava-allowed")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]
        public async Task<string> GetAthleteClubs()
        {
            var url = $"https://www.strava.com/api/v3/athlete/clubs";

            return await GetRequest(url, "GetAthleteClubs");
        }

    }
}
