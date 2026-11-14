using Cs_Hub.Domain.RepositoryContracts;
using Cs_Hub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Repositories
{
    public class Transaction : ITransaction
    {
        private readonly ApplicationDbContext _db;
        private IDbContextTransaction? _transaction;

        public Transaction(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            if(_transaction is null)
                throw new InvalidOperationException("No transaction in progress.");

            await _db.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransaction()
        {
            if (_transaction == null)
                return;

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
