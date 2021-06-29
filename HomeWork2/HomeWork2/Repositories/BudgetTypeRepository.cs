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
    public interface IBudgetTypeRepository
    {
        Task<int> Count(BudgetTypeFilter BudgetTypeFilter);
        Task<List<BudgetType>> List(BudgetTypeFilter BudgetTypeFilter);
        Task<List<BudgetType>> List(List<long> Ids);
        Task<BudgetType> Get(long Id);
        Task<bool> Create(BudgetType BudgetType);
        Task<bool> Update(BudgetType BudgetType);
        Task<bool> Delete(BudgetType BudgetType);
        Task<bool> BulkMerge(List<BudgetType> BudgetTypes);
        Task<bool> BulkDelete(List<BudgetType> BudgetTypes);
    }

    public class BudgetTypeRepository : IBudgetTypeRepository
    {
        private DataContext DataContext;
        public BudgetTypeRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<BudgetTypeDAO> DynamicFilter(IQueryable<BudgetTypeDAO> query, BudgetTypeFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            return query;
        }
        private async Task<List<BudgetType>> DynamicSelect(IQueryable<BudgetTypeDAO> query, BudgetTypeFilter filter)
        {
            List<BudgetType> BudgetTypes = await query.Select(q => new BudgetType()
            {
                Id = filter.Selects.Contains(BudgetTypeSelect.Id) ? q.Id : default(long),
                Name = filter.Selects.Contains(BudgetTypeSelect.Name) ? q.Name : default(string),
                Code = filter.Selects.Contains(BudgetTypeSelect.Code) ? q.Code : default(string)
            }).ToListAsync();
            return BudgetTypes;
        }

        private IQueryable<BudgetTypeDAO> DynamicOrder(IQueryable<BudgetTypeDAO> query, BudgetTypeFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case BudgetTypeOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case BudgetTypeOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case BudgetTypeOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case BudgetTypeOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case BudgetTypeOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case BudgetTypeOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        public async Task<BudgetType> Get(long Id)
        {
            BudgetType BudgetType = await DataContext.BudgetType.
                Where(x => x.Id == Id).Select(x => new BudgetType()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                }).FirstOrDefaultAsync();
            if (BudgetType == null)
            {
                return null;
            }
            return BudgetType;
        }

        public async Task<bool> Create(BudgetType BudgetType)
        {
            BudgetTypeDAO BudgetTypeDAO = new BudgetTypeDAO();
            BudgetTypeDAO.Id = BudgetType.Id;
            BudgetTypeDAO.Name = BudgetType.Name;
            BudgetTypeDAO.Code = BudgetType.Code;
            DataContext.BudgetType.Add(BudgetTypeDAO);
            await DataContext.SaveChangesAsync();
            BudgetType.Id = BudgetTypeDAO.Id;
            return true;
        }

        public async Task<bool> Update(BudgetType BudgetType)
        {
            BudgetTypeDAO BudgetTypeDAO = DataContext.BudgetType.Where(x => x.Id == BudgetType.Id).FirstOrDefault();
            if (BudgetTypeDAO == null)
                return false;
            BudgetTypeDAO.Id = BudgetType.Id;
            BudgetTypeDAO.Name = BudgetType.Name;
            BudgetTypeDAO.Code = BudgetType.Code;
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(BudgetType BudgetType)
        {
            await DataContext.BudgetType.Where(x => x.Id == BudgetType.Id).DeleteFromQueryAsync();
            return true;
        }
        public async Task<bool> BulkMerge(List<BudgetType> BudgetTypes)
        {
            List<BudgetTypeDAO> BudgetTypeDAOs = new List<BudgetTypeDAO>();
            foreach (BudgetType BudgetType in BudgetTypes)
            {
                BudgetTypeDAO BudgetTypeDAO = new BudgetTypeDAO();
                BudgetTypeDAO.Id = BudgetType.Id;
                BudgetTypeDAO.Name = BudgetType.Name;
                BudgetTypeDAO.Code = BudgetType.Code;
                BudgetTypeDAOs.Add(BudgetTypeDAO);
            }
            await DataContext.BulkMergeAsync(BudgetTypeDAOs);
            return true;
        }
        public async Task<bool> BulkDelete(List<BudgetType> BudgetTypes)
        {
            List<long> Ids = BudgetTypes.Select(x => x.Id).ToList();
            await DataContext.BudgetType
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            return true;
        }
        public async Task<int> Count(BudgetTypeFilter BudgetTypeFilter)
        {
            IQueryable<BudgetTypeDAO> BudgetTypes = DataContext.BudgetType.AsNoTracking();
            BudgetTypes = DynamicFilter(BudgetTypes, BudgetTypeFilter);
            return await BudgetTypes.CountAsync();
        }

        public async Task<List<BudgetType>> List(BudgetTypeFilter filter)
        {
            if (filter == null)
                return new List<BudgetType>();
            IQueryable<BudgetTypeDAO> BudgetTypeDAOs = DataContext.BudgetType.AsNoTracking();
            BudgetTypeDAOs = DynamicFilter(BudgetTypeDAOs, filter);
            BudgetTypeDAOs = DynamicOrder(BudgetTypeDAOs, filter);
            List<BudgetType> BudgetTypes = await DynamicSelect(BudgetTypeDAOs, filter);
            return BudgetTypes;
        }
        public async Task<List<BudgetType>> List(List<long> Ids)
        {
            List<BudgetType> BudgetTypes = await DataContext.BudgetType.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new BudgetType()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                }).ToListAsync();

            return BudgetTypes;
        }
    }
}
