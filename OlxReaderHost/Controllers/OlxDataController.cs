using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OlxReaderHost.Model;
using WorkerService1.Models;
using WorkerService1.Utilities;

namespace OlxReaderHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OlxDataController : ControllerBase
    {
        private readonly ILogger<OlxDataController> _logger;
        private readonly IHostEnvironment _env;
        private readonly ILocalCSVHelper _localCSVHelper;

        public OlxDataController(ILogger<OlxDataController> logger, IHostEnvironment env, ILocalCSVHelper localCSVHelper)
        {
            _logger = logger;
            _env = env;
            _localCSVHelper = localCSVHelper;
        }

        [HttpGet]
        public IEnumerable<OlxDataDto> Get(string city)
        {
            string olxRentCsv = $"{_env.ContentRootPath}/olxData.csv";

            List<CsvRow> records = _localCSVHelper.ReadCSVOldData(olxRentCsv);

            List<OlxDataDto> toReturn= new List<OlxDataDto>();

            if(string.IsNullOrWhiteSpace(city))
            {
                city = "all";
            }

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "all",
                ToSell = x.TotalToSell,
                ToRent = x.TotalToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Wrocław",
                ToSell = x.WroclawToSell,
                ToRent = x.WroclawToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Gdańsk",
                ToSell = x.GdanskToSell,
                ToRent = x.GdanskToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Warszawa",
                ToSell = x.WarszawaToSell,
                ToRent = x.WarszawaToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Kraków",
                ToSell = x.KrakowToSell,
                ToRent = x.KrakowToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Poznań",
                ToSell = x.PoznanToSell,
                ToRent = x.PoznanToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = DateTime.ParseExact(x.Date, "dd.MM.yyyy", CultureInfo.CurrentCulture),
                City = "Łódź",
                ToSell = x.LodzToSell,
                ToRent = x.LodzToRent
            }));


            var olxData = toReturn.Where(x=> x.City.ToLower() == city.ToLower());
            return olxData;
        }
    }
}
