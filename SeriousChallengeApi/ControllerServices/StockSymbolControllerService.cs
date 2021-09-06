using Newtonsoft.Json;
using SeriousChallenge.Infrastructure.Abstractions;
using SeriousChallenge.Models;
using SeriousChallengeApi.Dtos;
using SeriousChallengeApi.Extensions;
using SeriousChallengeApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SeriousChallengeApi.ControllerServices
{
    public class StockSymbolControllerService : IStockSymbolControllerService
    {
        private Dictionary<string, List<string>> symbolPercentages = new Dictionary<string, List<string>>();

        private const string compareStockName = "SPY";

        private IStockSymbolRepository _stockSymbolRepository { get; set; }

        private IUnitOfWork _unitOfWork { get; set; }

        private readonly PolygonService _polygonService;

        public StockSymbolControllerService(IUnitOfWork unitOfWork,
            IStockSymbolRepository stockSymbolRepository,
            IHttpClientFactory httpClientFacotory,
            PolygonService polygonService)
        {
            _polygonService = polygonService;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _stockSymbolRepository = stockSymbolRepository ?? throw new ArgumentNullException(nameof(stockSymbolRepository));
        }

        public async Task<Dictionary<string, List<string>>> Compare(string symbolName)
        {
            if (symbolName == null)
            {
                throw new ArgumentNullException(nameof(symbolName));
            }

            var weekDaysDates = DateTime.Now.GetWeekDaysDates();

            var lastWeeklyDatas = await _stockSymbolRepository.GetWeeklyStockDatas(symbolName, weekDaysDates[DayOfWeek.Monday].Date, weekDaysDates[DayOfWeek.Sunday].Date);

            if (lastWeeklyDatas.Count == 0)
            {
                var stockSymbol = await _polygonService.GetSymbolDatas(symbolName, weekDaysDates[DayOfWeek.Monday], weekDaysDates[DayOfWeek.Sunday]);

                if (stockSymbol.ResultsCount == 0)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                for (int i = 1; i < stockSymbol.Results.Count + 1; i++)
                {
                    var data = new StockSymbolModel()
                    {
                        InfoDate = weekDaysDates[(DayOfWeek)i].Date,
                        Name = symbolName,
                        Price = stockSymbol.Results[i - 1].Price
                    };

                    _stockSymbolRepository.Create(data);

                    lastWeeklyDatas.Add(data);
                }

                await _unitOfWork.Commit();
            }

            symbolPercentages.Add(lastWeeklyDatas.First().Name,
                GetPriceChangePercentages(lastWeeklyDatas));

            var lastWeeklyDatasForCompare = await _stockSymbolRepository.GetWeeklyStockDatas(compareStockName, weekDaysDates[DayOfWeek.Monday].Date, weekDaysDates[DayOfWeek.Sunday].Date);

            symbolPercentages.Add(lastWeeklyDatasForCompare.First().Name,
                 GetPriceChangePercentages(lastWeeklyDatasForCompare));

            return symbolPercentages;
        }

        private List<string> GetPriceChangePercentages(List<StockSymbolModel> weeklyDatas)
        {
            if (weeklyDatas.Count == 0)
            {
                throw new ArgumentException(nameof(weeklyDatas), "can't be empty");
            }

            var prices = weeklyDatas.Select(x => x.Price).ToList();

            var percentages = new List<string>();

            for (int i = 0; i < prices.Count; i++)
            {
                if (i == 0)
                {
                    percentages.Add("0%");
                    continue;
                }

                var perc = (prices[i] / prices[0] * 100) - 100;

                percentages.Add($"{perc.ToString("0.##")}%");
            }

            return percentages;
        }
    }
}
