using SeriousChallenge.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriousChallenge.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private EFContext _efContext;

        public UnitOfWork(EFContext efContext)
        {
            _efContext = efContext;
        }

        public Task Commit()
        {
            return _efContext.SaveChangesAsync();
        }
    }
}
