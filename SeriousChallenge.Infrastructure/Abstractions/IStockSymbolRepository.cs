using SeriousChallenge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriousChallenge.Infrastructure.Abstractions
{
    public interface IStockSymbolRepository
    {
        public void Create(StockSymbolModel model);

        public Task<List<StockSymbolModel>> GetWeeklyStockDatas(string symbolName, DateTime lastMondayDate, DateTime lastSundayDate);
    }
}
