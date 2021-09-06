using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeriousChallenge.Infrastructure.Abstractions;
using SeriousChallenge.Infrastructure.DbModel;
using SeriousChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriousChallenge.Infrastructure.Repos
{
    public class StockSymbolRepository : IStockSymbolRepository
    {
        private EFContext _efContext { get; set; }

        private readonly IMapper _mapper;

        public StockSymbolRepository(EFContext efContext, IMapper mapper)
        {
            _efContext = efContext ?? throw new ArgumentNullException(nameof(efContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void Create(StockSymbolModel model)
        {
            _efContext.StockSymbols.Add(_mapper.Map<StockSymbolDbModel>(model));

        }

        public async Task<List<StockSymbolModel>> GetWeeklyStockDatas(string symbolName, DateTime lastMondayDate, DateTime lastSundayDate)
        {
            var model = await _efContext.StockSymbols
                .Where(s => s.InfoDate >= lastMondayDate &&
                       s.InfoDate < lastSundayDate &&
                       s.Name == symbolName)
                .ToListAsync();

            return _mapper.Map<List<StockSymbolModel>>(model);
        }
    }
}
