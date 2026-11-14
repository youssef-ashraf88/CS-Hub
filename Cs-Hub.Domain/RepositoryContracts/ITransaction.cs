using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.RepositoryContracts
{
    public interface ITransaction
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
