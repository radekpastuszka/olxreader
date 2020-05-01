using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkerService1.Models;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Dictionary<string, string> cities = new Dictionary<string, string>();
            cities.Add("Total", "");
            cities.Add("Wrocław", "wroclaw");
            cities.Add("Gdańsk", "gdansk");
            cities.Add("Warszawa", "warszawa");
            cities.Add("Kraków", "krakow");
            cities.Add("Poznań", "poznan");
            cities.Add("Lodz", "lodz");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                List<CsvRow> records = new List<CsvRow>();
                CsvRow newRow = new CsvRow();

                foreach (var city in cities)
                {
                    try
                    {
                        var serachString = city.Value;

                        if (!string.IsNullOrWhiteSpace(city.Value))
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

                        int rentCount = Convert.ToInt32(rentString.Replace(" ",""));

                        string sellString = htmlDoc.DocumentNode
                        .SelectSingleNode($"//a[@href=\"https://www.olx.pl/nieruchomosci/mieszkania/sprzedaz/{serachString}\"]/following-sibling::span")?.InnerText;

                        int sellCount = Convert.ToInt32(sellString.Replace(" ", ""));

                        newRow.Date = DateTime.Now.Date;

                        switch(city.Key)
                        {
                            case "Total":
                                newRow.TotalToRent = rentCount;
                                newRow.TotalToSell = sellCount;
                                break;
                            case "Wrocław":
                                newRow.WroclawToRent = rentCount;
                                newRow.WroclawToSell = sellCount;
                                break;
                            case "Gdańsk":
                                newRow.GdanskToRent = rentCount;
                                newRow.GdanskToSell = sellCount;
                                break;
                            case "Warszawa":
                                newRow.WarszawaToRent = rentCount;
                                newRow.WarszawaToSell = sellCount;
                                break;
                            case "Kraków":
                                newRow.KrakowToRent = rentCount;
                                newRow.KrakowToSell = sellCount;
                                break;
                            case "Poznań":
                                newRow.PoznanToRent = rentCount;
                                newRow.PoznanToSell = sellCount;
                                break;
                            case "Lodz":
                                newRow.LodzToRent = rentCount;
                                newRow.LodzToSell = sellCount;
                                break;

                            default:
                                break;

                        }
                       
                    }
                    catch(Exception ex)
                    {
                        _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    }
                }


                string path = new FileInfo(AppDomain.CurrentDomain.BaseDirectory).Directory.Parent.Parent.Parent.FullName;
                string olxRentCsv = $"{path}/olxData.csv";

                if (File.Exists(olxRentCsv))
                {
                    using (var reader = new StreamReader(olxRentCsv))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        records = csv.GetRecords<CsvRow>().ToList();
                    }
                }

                if(!records.Any(x=> x.Date == newRow.Date))
                {
                    records.Add(newRow);

                    using (var writer = new StreamWriter(olxRentCsv))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(records);
                    }
                }
                
                
                await Task.Delay(1000*60*60*24, stoppingToken);
            }
        }
    }
}
