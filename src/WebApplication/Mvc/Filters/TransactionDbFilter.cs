using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace Infrastructure.Mvc.Filters
{
    public class TransactionDbFilter : IActionFilter
    {
        private readonly IStorage _storage;
        private DbContext _dbContext;

        private IDbContextTransaction _currentTransaction;

        public TransactionDbFilter(IStorage storage)
        {
            _storage = storage;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_currentTransaction != null)
            {
                if (context.Exception != null)
                    _currentTransaction.Rollback();
                else
                    _currentTransaction.Commit();

                _currentTransaction.Dispose();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (new string[] { "POST", "PUT", "DELETE" }.Contains(context.HttpContext.Request.Method))
            {
                //if (_dbContext == null)
                _dbContext = _storage.StorageContext as DbContext;

                _currentTransaction = _dbContext.Database.BeginTransaction();
            }
        }
    }
}