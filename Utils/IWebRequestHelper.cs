
namespace diploma_strava_ex_api.Utils
{
    public interface IWebRequestHelper
    {
        Task<string> GetDataFromApi(string uri, string userId, string token, string[] parameters);
        string getTokenFromRequest(HttpContext httpContext);
        string getAthleteIdFromRequest(HttpContext httpContext);
    }
}