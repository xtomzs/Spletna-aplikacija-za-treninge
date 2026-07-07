using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace diploma_strava_ex_api.Utils
{
    public class WebRequestHelper : IWebRequestHelper
    {        
        public WebRequestHelper()
        {         
        }

        public Task<string> GetDataFromApi(string uri, string userId, string token, string[] parameters)
        {
            //var client = _httpClientFactory.CreateClient();

            ////var accessToken = HttpContext.Session.Get(userID).ToString();
            //using var req = new HttpRequestMessage(HttpMethod.Get, uri);
            //req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //using var response = await client.SendAsync(req);
            //var content = await response.Content.ReadAsStringAsync();
            return Task.FromResult(string.Empty);
        }

        public string getTokenFromRequest(HttpContext httpContext)
        {
            var token = "";
            httpContext.Request.Cookies.TryGetValue("auth", out token);
            var cookieObj = JsonSerializer.Deserialize<Dictionary<string, string>>(token);
            return cookieObj["access_token"];
        }

        public string getAthleteIdFromRequest(HttpContext httpContext)
        {
            var token = "";
            httpContext.Request.Cookies.TryGetValue("auth", out token);
            var cookieObj = JsonSerializer.Deserialize<Dictionary<string, string>>(token);
            return cookieObj["athlete_id"];
        }

    }
}
