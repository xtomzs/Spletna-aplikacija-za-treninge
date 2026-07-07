using diploma_strava_ex_api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace diploma_strava_ex_api.Controllers
{
    public class BaseStravaController : Controller
    {
        public readonly IHttpClientFactory HttpClientFactory;
        public readonly IWebRequestHelper RequestHelper;
        public readonly IMemoryCache _memoryCache;
        public readonly IConfiguration? _configuration;

        public BaseStravaController(IHttpClientFactory httpClientFactory, IWebRequestHelper requestHelper, IMemoryCache memoryCache, IConfiguration? configuration = null)
        {
            HttpClientFactory = httpClientFactory;
            RequestHelper = requestHelper;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public async Task<string> GetRequest(string url, string cacheKey = null)
        {
            var client = HttpClientFactory.CreateClient();
            var content = "";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", RequestHelper.getTokenFromRequest(HttpContext));
            if(!_memoryCache.TryGetValue(cacheKey, out content))
            {
                using var response = await client.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
                _memoryCache.Set(cacheKey, content, new TimeSpan(1,0,0));
            }

            // If we have a cacheKey, attempt to save the content to file when enabled.
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                var savedJson = await SaveContentObjectAsync(content, cacheKey);
                if (savedJson != null)
                {
                    return savedJson;
                }
            }

            return content;
        }

        public async Task<T> GetRequest<T>(string url, string cacheKey = null)
        {
            var client = HttpClientFactory.CreateClient();
            var content = "";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", RequestHelper.getTokenFromRequest(HttpContext));
            if(cacheKey != null)
            {
                if (!_memoryCache.TryGetValue(cacheKey, out content))
                {
                    using var response = await client.GetAsync(url);
                    content = await response.Content.ReadAsStringAsync();
                    _memoryCache.Set(cacheKey, content, new TimeSpan(1, 0, 0));
                }
            }
            else
            {
                using var response = await client.GetAsync(url);
                content = await response.Content.ReadAsStringAsync();
            }

            var deserialized = JsonSerializer.Deserialize<T>(content);

            // If we have a cacheKey, attempt to save the deserialized object to file when enabled.
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                var savedJson = await SaveContentObjectAsync(deserialized!, cacheKey);
                if (savedJson != null)
                {
                    return JsonSerializer.Deserialize<T>(savedJson);
                }
            }

            return deserialized!;
        }

        // Saves the provided contentObject to the project root as {cacheKey}.json when enabled in appsettings.json
        // If the appsetting "SaveResponsesToFile" is true, the method writes the JSON and returns the JSON string
        // Otherwise it returns null.
        public async Task<string?> SaveContentObjectAsync(object contentObject, string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey)) throw new ArgumentException("cacheKey must be provided", nameof(cacheKey));

            var saveEnabled = _configuration?.GetValue<bool>("SaveResponsesToFile") ?? false;
            if (!saveEnabled) return null;

            string json;
            if (contentObject is string str)
            {                
                json = str;
            }
            else
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                json = JsonSerializer.Serialize(contentObject, options);
            }

            var fileName = cacheKey.EndsWith(".json", StringComparison.OrdinalIgnoreCase) ? cacheKey : cacheKey + ".json";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            await System.IO.File.WriteAllTextAsync(filePath, json);

            return json;
        }
    }
}
