using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                foreach(var city in cities)
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
                    }
                    catch(Exception ex)
                    {
                        _logger.LogInformation(ex.Message, DateTimeOffset.Now);
                    }
                }
                

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
