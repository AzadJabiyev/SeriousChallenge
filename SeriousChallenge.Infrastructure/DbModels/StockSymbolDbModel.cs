using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeriousChallenge.Infrastructure.DbModel
{
    [Table("StockSymbol")]
    public class StockSymbolDbModel
    {
        public string Name { get; set; }

        public DateTime InfoDate { get; set; }

        public float Price { get; set; }
    }
}
