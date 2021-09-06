using AutoMapper;
using SeriousChallenge.Infrastructure.DbModel;
using SeriousChallenge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeriousChallenge.Infrastructure.AutoMapperProfiles
{
    public class DatabaseMappingProfile: Profile
    {
        public DatabaseMappingProfile()
        {
            CreateMap<StockSymbolDbModel, StockSymbolModel>(MemberList.Destination);

            CreateMap<StockSymbolModel, StockSymbolDbModel>(MemberList.Destination);
        }
    }
}
