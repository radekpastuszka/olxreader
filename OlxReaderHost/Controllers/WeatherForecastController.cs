using System;
using System.Collections.Generic;
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
    public class WeatherForecastController : ControllerBase
    {
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
        public IEnumerable<OlxDataDto> Get(DataTypeEnum dataType)
        {
            string olxRentCsv = $"{_env.ContentRootPath}/olxData.csv";

            List<CsvRow> records = _localCSVHelper.ReadCSVOldData(olxRentCsv);

            List<OlxDataDto> toReturn= new List<OlxDataDto>();

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.All,
                ToSell = x.TotalToSell,
                ToRent = x.TotalToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Wroclaw,
                ToSell = x.WroclawToSell,
                ToRent = x.WroclawToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Gdansk,
                ToSell = x.GdanskToSell,
                ToRent = x.GdanskToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Warszawa,
                ToSell = x.WarszawaToSell,
                ToRent = x.WarszawaToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Krakow,
                ToSell = x.KrakowToSell,
                ToRent = x.KrakowToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Poznan,
                ToSell = x.PoznanToSell,
                ToRent = x.PoznanToRent
            }));

            toReturn.AddRange(records.Select(x => new OlxDataDto()
            {
                Date = Convert.ToDateTime(x.Date),
                DataType = DataTypeEnum.Lodz,
                ToSell = x.LodzToSell,
                ToRent = x.LodzToRent
            }));


            return toReturn.Where(x=> x.DataType == dataType);
        }
    }
}
