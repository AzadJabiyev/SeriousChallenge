using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeriousChallenge.Infrastructure.Abstractions;
using SeriousChallenge.Models;
using SeriousChallengeApi.Dtos;
using SeriousChallengeApi.Extensions;
using SeriousChallengeApi.Services;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SeriousChallengeApi.Jobs
{
    public class UpdateSpyData : BackgroundService
    {
        private readonly ILogger<UpdateSpyData> _logger;

        private readonly PolygonService _polygonService;

        private readonly TimeSpan _waitTimeSpan = TimeSpan.FromDays(7);

        private IServiceProvider _serviceProvider { get; set; }

        public UpdateSpyData(
            ILogger<UpdateSpyData> logger,
            PolygonService polygonService,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _polygonService = polygonService;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WeeklyUpdateSPY is started");

            while (true)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var repo = scope.ServiceProvider.GetService<IStockSymbolRepository>();
                        var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                        var weekDaysDates = DateTime.Now.GetWeekDaysDates();
                        var datas = await _polygonService.GetSymbolDatas("SPY", weekDaysDates[DayOfWeek.Monday], weekDaysDates[DayOfWeek.Sunday]);
                        var stockSymbol = new StockSymbolDto();

                        if (stockSymbol.ResultsCount == 0)
                            throw new HttpResponseException(HttpStatusCode.NotFound);

                        for (int i = 1; i < stockSymbol.Results.Count + 1; i++)
                        {
                            var data = new StockSymbolModel()
                            {
                                InfoDate = weekDaysDates[(DayOfWeek)i].Date,
                                Name = "SPY",
                                Price = stockSymbol.Results[i - 1].Price
                            };

                            repo.Create(data);
                        }

                        await unitOfWork.Commit();

                        _logger.LogInformation("WeeklyUpdateSPY is finished");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex} is occured while executing WeeklyUpdateSPYJob.");
                }

                await Task.Delay(_waitTimeSpan);
            }
        }
    }
}
