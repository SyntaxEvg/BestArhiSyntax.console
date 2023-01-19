using Microsoft.AspNetCore.Mvc;
using TestConsul.Model;

namespace TestConsul.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var consulDemoKey = await ConsulKeyValueProvider.GetValueAsync<ConsulDemoKey>(key: "TestKey");

            if (consulDemoKey != null && consulDemoKey.IsEnabled)
            {
                return Ok(consulDemoKey);
            }

            return Ok("ConsulDemoKey is null");
        }
    }
    [Produces("application/json")]
    [Route("api/Health")]
    public class apiHealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            //Console.WriteLine("" + DateTime.Now);
            return Content("ok");
        }
    } 

    //[Produces("application/json")]
    //[Route("health")]
    //public class HealthController : Controller
    //{
    //    [HttpGet]
    //    public IActionResult Get()
    //    {
    //        //Console.WriteLine("" + DateTime.Now);
    //        return Content("ok");
    //    }
    //}
    [Route("[Controller]")]
    public class HealthCheckController : Controller
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}