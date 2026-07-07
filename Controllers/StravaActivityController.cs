using diploma_strava_ex_api.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace diploma_strava_ex_api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]/[action]")]
    public class StravaActivityController : BaseStravaController
    {
        public StravaActivityController(IHttpClientFactory httpClientFactory, IWebRequestHelper requestHelper, IMemoryCache memoryCache, IConfiguration configuration) : base(httpClientFactory, requestHelper, memoryCache, configuration)
        {
        }
        
        [Authorize(Policy = "strava-allowed")]
        public async Task<IActionResult> Index()
        {   
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "strava-allowed")]        
        public async Task<string> GetActivityDetails(string id)
        {
            var url = $"https://www.strava.com/api/v3/activities/{id}";

            return await GetRequest(url, "GetActivityDetails"+id);
        }
    }
}
