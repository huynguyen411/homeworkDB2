using HomeWork2.Common;
using HomeWork2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        Task Begin();
        Task Commit();
        Task Rollback();
        IBudgetRepository BudgetRepository { get; }
        IBudgetTypeRepository BudgetTypeRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IProjectRepository ProjectRepository { get; }
    }
    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IBudgetRepository BudgetRepository { get; private set;}
        public IBudgetTypeRepository BudgetTypeRepository { get; private set;}
        public ICompanyRepository CompanyRepository { get; private set; }
        public IProjectRepository ProjectRepository { get; private set; }

        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;
            BudgetRepository = new BudgetRepository(DataContext);
            BudgetTypeRepository = new BudgetTypeRepository(DataContext);
            CompanyRepository = new CompanyRepository(DataContext);
            ProjectRepository = new ProjectRepository(DataContext);
        }
        public async Task Begin()
        {
            return;
            //await DataContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            //DataContext.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            //DataContext.Database.RollbackTransaction();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }

            this.DataContext.Dispose();
            this.DataContext = null;
        }
    }
}
