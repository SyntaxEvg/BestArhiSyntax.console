using App.Web.Api.Controllers.Base;
using Interfaces.Base.Base;
using Interfaces.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Web.Api.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Extension.Base;

namespace App.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController<WeatherForecastController>
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IDistributedCache distributedCache;

        public WeatherForecastController(IDistributedCache distributedCache,ILogger<WeatherForecastController> logger, IlangDictionaryScopedService lang) :base(logger,lang)
        {
            this.distributedCache = distributedCache;
        }

        [MiddlewareFilter(typeof(FilterMiddlewarePipeline))]
        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IEnumerable<WeatherForecast>> Get([FromServices] ITestTransientService service)
        {
            var types=  await distributedCache.GetAsync("Hi");
            if (types is not null)
            {
                var obj =types.ByteArrayToString().StringToToObject<List<WeatherForecast>>();
            }
            else
            {
                var t = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                })
                .ToJson();
                var res = t.StringToByteArray();
                if (res is not null)
                {
                    var opt = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(DateTime.Now.AddDays(1));
                    await distributedCache.SetAsync("Hi", res, opt);
                }

            }
            "csa".ToJson();
            var token = await HttpContext.GetTokenAsync("access_token");//получить  jwt token
            service.Test();
            var trans =_lang.Get("welcome");
            await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    HttpContext.User);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Lang = trans
            })
            .ToArray();
        }
    }
}