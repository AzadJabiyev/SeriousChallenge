using SeriousChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriousChallengeApi.ControllerServices
{
    public interface IStockSymbolControllerService
    {
        Task<Dictionary<string, List<string>>> Compare(string symbolName);
    }
}
