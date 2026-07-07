using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using diploma_strava_ex_api.Models.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using diploma_strava_ex_api.Utils;
using diploma_strava_ex_api.Models.DBModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IWebRequestHelper, WebRequestHelper>();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = "/login";
        var del = o.Events.OnRedirectToAccessDenied;
        o.Events.OnRedirectToAccessDenied = ctx =>
        {
            //if (ctx.Request.Path.StartsWithSegments("/strava", StringComparison.OrdinalIgnoreCase))
            if (ctx.Request.Path.Value.StartsWith("/strava", StringComparison.OrdinalIgnoreCase))
            {
                return ctx.HttpContext.ChallengeAsync("strava");
            }
            return del(ctx);
        };
    })
    .AddOAuth("strava", options =>
    {
        var settigns = builder.Configuration.GetSection("Authentication");
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.AuthorizationEndpoint = "https://www.strava.com/api/v3/oauth/authorize";
        options.TokenEndpoint = "https://www.strava.com/api/v3/oauth/token";
        options.ClientId = settigns["clientId"];
        options.ClientSecret = settigns["clientSecret"];
        options.Scope.Clear();
        options.Scope.Add("activity:read_all,profile:read_all");
        
        options.CallbackPath = "/oauth/strava_cb";

        options.Events.OnCreatingTicket = async ctx =>
        {
            if (ctx.TokenResponse.Response == null)
                throw new Exception("Invalid token response");

            var res = JsonSerializer.Deserialize<TokenResponse>(ctx.TokenResponse.Response);
            var authHandlerProvider = ctx.HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var authHandler = await authHandlerProvider.GetHandlerAsync(ctx.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme);
            var authResult = await authHandler.AuthenticateAsync();
            if (!authResult.Succeeded)
            {
                ctx.Fail("Failed authentication");
                return;
            }
            var cp = authResult.Principal;

            var userId = cp.FindFirstValue("user_id");

            ctx.Principal = cp.Clone();
            var identity = ctx.Principal.Identities.First(x => x.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme);
            var cookie = JsonSerializer.Serialize(new Dictionary<string, string> { { "access_token", res.Access_token }, { "refresh_token", res.Refresh_token }, { "athlete_id", res.Athlete.Id.ToString() } });
            ctx.HttpContext.Response.Cookies.Append("auth", cookie, new CookieOptions { Expires = DateTime.Now.AddSeconds(res.Expires_in) });
            identity.AddClaim(new Claim("strava-token", "y"));
        };
    });
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("strava-allowed", pb =>
    {
        pb.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim("strava-token", "y")
            .RequireAuthenticatedUser();
    });
});
builder.Services.AddDbContext<PlannerContext>(options => options.UseSqlite());

var app = builder.Build();

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] { new Claim("user_id", Guid.NewGuid().ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme
            )
        ),
    authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme
    ));


app.UseStaticFiles();
app.UseAuthorization();

app.MapControllerRoute(
    name: "dashboard",
    pattern: "StravaDashboard",
    defaults: new { controller = "StravaDashboard", action = "Index" }
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
