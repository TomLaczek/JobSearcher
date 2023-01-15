// See https://aka.ms/new-console-template for more information
using JobSearcher.App;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("Job Searcher starts work!");
var configParams = new ConfigurationHelper()
{
    BipUrls = config.GetSection("Urls:Bip").Get<List<string>>(),
    PinbUrls = config.GetSection("Urls:Pinb").Get<List<string>>()
};
var app = new Crawler(configParams);
await app.CrawlThroughSites();

Console.WriteLine("Job Searcher ends work!");
Thread.Sleep(10000);