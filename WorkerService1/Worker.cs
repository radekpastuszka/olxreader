﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerService1.Constants;
using WorkerService1.Models;
using WorkerService1.Utilities;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHostEnvironment _env;
        private readonly ILocalCSVHelper _localCSVHelper;
        private DateTime? lastRun;

        public Worker(ILogger<Worker> logger, IHostEnvironment env, ILocalCSVHelper localCSVHelper)
        {
            _logger = logger;
            _env = env;
            _localCSVHelper = localCSVHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!lastRun.HasValue || (lastRun.HasValue && !lastRun.Value.Equals(DateTime.Now.Date)))
                {
                    lastRun = DateTime.Now.Date;

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    CsvRow newRow = new CsvRow();
                    newRow.Date = DateTime.Now.Date.ToString("dd.MM.yyyy");

                    string path = _env.ContentRootPath;
                    string olxRentCsv = $"{path}/olxData.csv";

                    List<CsvRow> records = _localCSVHelper.ReadCSVOldData(olxRentCsv);

                    if (!records.Any(x => x.Date == newRow.Date))
                    {
                        foreach (var city in Cities.GetCities())
                        {
                            try
                            {
                                (int rentCount, int sellCount) = ReadDataFromOlx(city);
                                newRow = GetNewRowData(newRow, city, rentCount, sellCount);

                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                            }
                        }

                        _localCSVHelper.SaveCSVData(records, newRow, olxRentCsv);
                    }
                }

                int delay = 1000 * 60 * 60 * 1; //1h

                await Task.Delay(delay, stoppingToken);
            }
        }

        private static CsvRow GetNewRowData(CsvRow newRow, string city, int rentCount, int sellCount)
        {
            switch (city)
            {
                case Cities.Total:
                    newRow.TotalToRent = rentCount;
                    newRow.TotalToSell = sellCount;
                    break;
                case Cities.Wroclaw:
                    newRow.WroclawToRent = rentCount;
                    newRow.WroclawToSell = sellCount;
                    break;
                case Cities.Gdansk:
                    newRow.GdanskToRent = rentCount;
                    newRow.GdanskToSell = sellCount;
                    break;
                case Cities.Warszawa:
                    newRow.WarszawaToRent = rentCount;
                    newRow.WarszawaToSell = sellCount;
                    break;
                case Cities.Krakow:
                    newRow.KrakowToRent = rentCount;
                    newRow.KrakowToSell = sellCount;
                    break;
                case Cities.Poznan:
                    newRow.PoznanToRent = rentCount;
                    newRow.PoznanToSell = sellCount;
                    break;
                case Cities.Lodz:
                    newRow.LodzToRent = rentCount;
                    newRow.LodzToSell = sellCount;
                    break;

                default:
                    break;

            }

            return newRow;
        }

        private static (int, int) ReadDataFromOlx(string city)
        {
            var serachString = city;

            if (!string.IsNullOrWhiteSpace(city))
            {
                serachString = $"{serachString}/";
            }

            // From Web
            var url = $"https://www.olx.pl/nieruchomosci/mieszkania/{serachString}";
            var web = new HtmlWeb();
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.122 Safari/537.36";
            var htmlDoc = web.Load(url);

            string rentString = htmlDoc.DocumentNode
            .SelectSingleNode($"//a[@href=\"https://www.olx.pl/nieruchomosci/mieszkania/wynajem/{serachString}\"]/following-sibling::span")?.InnerText;

            int rentCount = Convert.ToInt32(rentString.Replace(" ", ""));
            string sellString = htmlDoc.DocumentNode
            .SelectSingleNode($"//a[@href=\"https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/{serachString}\"]/following-sibling::span")?.InnerText;

            int sellCount = Convert.ToInt32(sellString.Replace(" ", ""));

            return (rentCount, sellCount);
        }
    }
}
