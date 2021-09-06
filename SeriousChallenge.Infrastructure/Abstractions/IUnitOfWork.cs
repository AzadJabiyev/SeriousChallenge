using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriousChallenge.Infrastructure.Abstractions
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
