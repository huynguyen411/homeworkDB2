using HomeWork2.Entities;
using HomeWork2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork2.Common;
using Microsoft.EntityFrameworkCore;

namespace HomeWork2.Repositories
{
    public interface IBudgetRepository
    {
        Task<int> Count(BudgetFilter BudgetFilter);
        Task<List<Budget>> List(BudgetFilter BudgetFilter);
        Task<List<Budget>> List(List<long> Ids);
        Task<Budget> Get(long Id);
        Task<bool> Create(Budget Budget);
        Task<bool> Update(Budget Budget);
        Task<bool> Delete(Budget Budget);
        Task<bool> BulkMerge(List<Budget> Budgets);
        Task<bool> BulkDelete(List<Budget> Budgets);
    }

    public class BudgetRepository : IBudgetRepository
    {
        private DataContext DataContext;
        public BudgetRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<BudgetDAO> DynamicFilter(IQueryable<BudgetDAO> query, BudgetFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.BudgetTypeId, filter.BudgetTypeId);
            query = query.Where(q => q.CompanyId, filter.CompanyId);
            query = query.Where(q => q.ProjectId, filter.ProjectId);
            query = query.Where(q => q.Amount, filter.Amount);
            query = query.Where(q => q.Year, filter.Year);
            query = query.Where(q => q.Month, filter.Month);
            return query;
        }
        private async Task<List<Budget>> DynamicSelect(IQueryable<BudgetDAO> query, BudgetFilter filter)
        {
            List<Budget> Budgets = await query.Select(q => new Budget()
            {
                Id = filter.Selects.Contains(BudgetSelect.Id) ? q.Id : default(long),
                BudgetTypeId = filter.Selects.Contains(BudgetSelect.BudgetType) ? q.BudgetTypeId : default(long),
                CompanyId = filter.Selects.Contains(BudgetSelect.Company) ? q.CompanyId : default(long),
                ProjectId = filter.Selects.Contains(BudgetSelect.Project) ? q.ProjectId : default(long),
                Amount = filter.Selects.Contains(BudgetSelect.Amount) ? q.Amount : default(decimal),
                Year = filter.Selects.Contains(BudgetSelect.Year) ? q.Year : default(int),
                Month = filter.Selects.Contains(BudgetSelect.Month) ? q.Month : default(int),
            }).ToListAsync();
            return Budgets;
        }

        private IQueryable<BudgetDAO> DynamicOrder(IQueryable<BudgetDAO> query, BudgetFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case BudgetOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case BudgetOrder.Company:
                            query = query.OrderBy(q => q.CompanyId);
                            break;
                        case BudgetOrder.Project:
                            query = query.OrderBy(q => q.ProjectId);
                            break;
                        case BudgetOrder.BudgetType:
                            query = query.OrderBy(q => q.BudgetTypeId);
                            break;
                        case BudgetOrder.Amount:
                            query = query.OrderBy(q => q.Amount);
                            break;
                        case BudgetOrder.Year:
                            query = query.OrderBy(q => q.Year);
                            break;
                        case BudgetOrder.Month:
                            query = query.OrderBy(q => q.Month);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case BudgetOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case BudgetOrder.Company:
                            query = query.OrderByDescending(q => q.CompanyId);
                            break;
                        case BudgetOrder.Project:
                            query = query.OrderByDescending(q => q.ProjectId);
                            break;
                        case BudgetOrder.BudgetType:
                            query = query.OrderByDescending(q => q.BudgetTypeId);
                            break;
                        case BudgetOrder.Amount:
                            query = query.OrderByDescending(q => q.Amount);
                            break;
                        case BudgetOrder.Year:
                            query = query.OrderByDescending(q => q.Year);
                            break;
                        case BudgetOrder.Month:
                            query = query.OrderByDescending(q => q.Month);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        public async Task<Budget> Get(long Id)
        {
            Budget Budget = await DataContext.Budget.
                Where(x => x.Id == Id).Select(x => new Budget()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Company = new Company()
                    {
                        Id = x.Company.Id,
                        Code = x.Company.Code,
                        Name = x.Company.Name,
                        ParentId = x.Company.ParentId,
                        Level = x.Company.Level,
                    },
                    ProjectId = x.ProjectId,
                    Project = new Project()
                    {
                        Id = x.Project.Id,
                        Code = x.Project.Code,
                        Name = x.Project.Name,
                        StartAt = x.Project.StartAt,
                        EndAt = x.Project.EndAt
                    },
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetType = new BudgetType()
                    {
                        Id = x.BudgetType.Id,
                        Code = x.BudgetType.Code,
                        Name = x.BudgetType.Name,
                    },
                    Amount = x.Amount,
                    Year = x.Year,
                    Month = x.Month,
                }).FirstOrDefaultAsync();
            if (Budget == null)
            {
                return null;
            }
            return Budget;
        }

        public async Task<bool> Create(Budget Budget)
        {
            BudgetDAO BudgetDAO = new BudgetDAO();
            BudgetDAO.Id = Budget.Id;
            BudgetDAO.ProjectId = Budget.ProjectId;
            BudgetDAO.CompanyId = Budget.CompanyId;
            BudgetDAO.BudgetTypeId = Budget.BudgetTypeId;
            BudgetDAO.Amount = Budget.Amount;
            BudgetDAO.Year = Budget.Year;
            BudgetDAO.Month = Budget.Month;
            DataContext.Budget.Add(BudgetDAO);
            await DataContext.SaveChangesAsync();
            Budget.Id = BudgetDAO.Id;
            return true;
        }

        public async Task<bool> Update(Budget Budget)
        {
            BudgetDAO BudgetDAO = DataContext.Budget.Where(x => x.Id == Budget.Id).FirstOrDefault();
            if (BudgetDAO == null)
                return false;
            BudgetDAO.Id = Budget.Id;
            BudgetDAO.ProjectId = Budget.ProjectId;
            BudgetDAO.CompanyId = Budget.CompanyId;
            BudgetDAO.BudgetTypeId = Budget.BudgetTypeId;
            BudgetDAO.Amount = Budget.Amount;
            BudgetDAO.Year = Budget.Year;
            BudgetDAO.Month = Budget.Month;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Budget Budget)
        {
            await DataContext.Budget.Where(x => x.Id == Budget.Id).DeleteFromQueryAsync();
            return true;
        }
        public async Task<bool> BulkMerge(List<Budget> Budgets)
        {
            List<BudgetDAO> BudgetDAOs = new List<BudgetDAO>();
            foreach (Budget Budget in Budgets)
            {
                BudgetDAO BudgetDAO = new BudgetDAO();
                BudgetDAO.Id = Budget.Id;
                BudgetDAO.ProjectId = Budget.ProjectId;
                BudgetDAO.CompanyId = Budget.CompanyId;
                BudgetDAO.BudgetTypeId = Budget.BudgetTypeId;
                BudgetDAO.Amount = Budget.Amount;
                BudgetDAO.Year = Budget.Year;
                BudgetDAO.Month = Budget.Month;
                BudgetDAOs.Add(BudgetDAO);
            }
            await DataContext.BulkMergeAsync(BudgetDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<Budget> Budgets)
        {
            List<long> Ids = Budgets.Select(x => x.Id).ToList();
            await DataContext.Budget
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }
        public async Task<int> Count(BudgetFilter BudgetFilter)
        {
            IQueryable<BudgetDAO> Budgets = DataContext.Budget.AsNoTracking();
            Budgets = DynamicFilter(Budgets, BudgetFilter);
            return await Budgets.CountAsync();
        }

        public async Task<List<Budget>> List(BudgetFilter filter)
        {
            if (filter == null)
                return new List<Budget>();
            IQueryable<BudgetDAO> BudgetDAOs = DataContext.Budget.AsNoTracking();
            BudgetDAOs = DynamicFilter(BudgetDAOs, filter);
            BudgetDAOs = DynamicOrder(BudgetDAOs, filter);
            List<Budget> Budgets = await DynamicSelect(BudgetDAOs, filter);
            return Budgets;
        }
        public async Task<List<Budget>> List(List<long> Ids)
        {
            List<Budget> Budgets = await DataContext.Budget.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new Budget()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Company = new Company()
                    {
                        Id = x.Company.Id,
                        Code = x.Company.Code,
                        Name = x.Company.Name,
                        ParentId = x.Company.ParentId,
                        Level = x.Company.Level,
                    },
                    ProjectId = x.ProjectId,
                    Project = new Project()
                    {
                        Id = x.Project.Id,
                        Code = x.Project.Code,
                        Name = x.Project.Name,
                        StartAt = x.Project.StartAt,
                        EndAt = x.Project.EndAt
                    },
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetType = new BudgetType()
                    {
                        Id = x.BudgetType.Id,
                        Code = x.BudgetType.Code,
                        Name = x.BudgetType.Name,
                    },
                    Amount = x.Amount,
                    Year = x.Year,
                    Month = x.Month
                }).ToListAsync();

            return Budgets;
        }
    }
}
