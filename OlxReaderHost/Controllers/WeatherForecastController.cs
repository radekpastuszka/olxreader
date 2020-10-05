using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerService1.Models;
using WorkerService1.Utilities;

namespace OlxReaderHost.Controllers
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
        private readonly IHostEnvironment _env;
        private readonly ILocalCSVHelper _localCSVHelper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHostEnvironment env, ILocalCSVHelper localCSVHelper)
        {
            _logger = logger;
            _env = env;
            _localCSVHelper = localCSVHelper;
        }

        [HttpGet]
        public IEnumerable<CsvRow> Get()
        {
            string olxRentCsv = $"{_env.ContentRootPath}/olxData.csv";

            List<CsvRow> records = _localCSVHelper.ReadCSVOldData(olxRentCsv);
            return records;






            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}
