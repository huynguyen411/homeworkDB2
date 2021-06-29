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
    public interface ICompanyRepository
    {
        Task<int> Count(CompanyFilter CompanyFilter);
        Task<List<Company>> List(CompanyFilter CompanyFilter);
        Task<List<Company>> List(List<long> Ids);
        Task<Company> Get(long Id);
        Task<bool> Create(Company Company);
        Task<bool> Update(Company Company);
        Task<bool> Delete(Company Company);
        Task<bool> BulkMerge(List<Company> Companys);
        Task<bool> BulkDelete(List<Company> Companys);
    }

    public class CompanyRepository : ICompanyRepository
    {
        private DataContext DataContext;
        public CompanyRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        private IQueryable<CompanyDAO> DynamicFilter(IQueryable<CompanyDAO> query, CompanyFilter filter)
        {
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Code, filter.Code);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.ParentId, filter.ParentId);
            query = query.Where(q => q.Level, filter.Level);
            return query;
        }
        private async Task<List<Company>> DynamicSelect(IQueryable<CompanyDAO> query, CompanyFilter filter)
        {
            List<Company> Companys = await query.Select(q => new Company()
            {
                Id = filter.Selects.Contains(CompanySelect.Id) ? q.Id : default(long),
                Name = filter.Selects.Contains(CompanySelect.Name) ? q.Name : default(string),
                Code = filter.Selects.Contains(CompanySelect.Code) ? q.Code : default(string),
                ParentId = filter.Selects.Contains(CompanySelect.Parent) ? q.ParentId : default(long?),
                Level = filter.Selects.Contains(CompanySelect.Level) ? q.Level : default(long),
            }).ToListAsync();
            return Companys;
        }

        private IQueryable<CompanyDAO> DynamicOrder(IQueryable<CompanyDAO> query, CompanyFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case CompanyOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case CompanyOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case CompanyOrder.Code:
                            query = query.OrderBy(q => q.Code);
                            break;
                        case CompanyOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case CompanyOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case CompanyOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case CompanyOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case CompanyOrder.Code:
                            query = query.OrderByDescending(q => q.Code);
                            break;
                        case CompanyOrder.Parent:
                            query = query.OrderBy(q => q.ParentId);
                            break;
                        case CompanyOrder.Level:
                            query = query.OrderBy(q => q.Level);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        public async Task<Company> Get(long Id)
        {
            Company Company = await DataContext.Company.
                Where(x => x.Id == Id).Select(x => new Company()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Level = x.Level,
                    ParentId = x.ParentId,
                    Parent = x.Parent == null? null : new Company
                    {
                        Id = x.Parent.Id,
                        Code = x.Parent.Code,
                        Name = x.Parent.Name,
                        Level = x.Parent.Level,
                    }

                }).FirstOrDefaultAsync();
            if (Company == null)
            {
                return null;
            }
            return Company;
        }

        public async Task<bool> Create(Company Company)
        {
            CompanyDAO CompanyDAO = new CompanyDAO();
            CompanyDAO.Id = Company.Id;
            CompanyDAO.Name = Company.Name;
            CompanyDAO.Code = Company.Code;
            CompanyDAO.ParentId = Company.ParentId;
            CompanyDAO.Level = 1;
            DataContext.Company.Add(CompanyDAO);
            Company.Id = CompanyDAO.Id;
            await BuildPath();
            await DataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(Company Company)
        {
            CompanyDAO CompanyDAO = DataContext.Company.Where(x => x.Id == Company.Id).FirstOrDefault();
            if (CompanyDAO == null)
                return false;
            CompanyDAO.Id = Company.Id;
            CompanyDAO.Name = Company.Name;
            CompanyDAO.Code = Company.Code;
            CompanyDAO.ParentId = Company.ParentId;
            CompanyDAO.Level = Company.Level;
            await DataContext.SaveChangesAsync();
            await BuildPath();
            return true;
        }

        public async Task<bool> Delete(Company Company)
        {
            await DataContext.Company.Where(x => x.Id == Company.Id).DeleteFromQueryAsync();
            await BuildPath();
            return true;
        }
        public async Task<bool> BulkMerge(List<Company> Companys)
        {
            List<CompanyDAO> CompanyDAOs = new List<CompanyDAO>();
            foreach (Company Company in Companys)
            {
                CompanyDAO CompanyDAO = new CompanyDAO();
                CompanyDAO.Id = Company.Id;
                CompanyDAO.Name = Company.Name;
                CompanyDAO.Code = Company.Code;
                CompanyDAO.ParentId = Company.ParentId;
                CompanyDAO.Level = Company.Level;
                CompanyDAOs.Add(CompanyDAO);
            }
            await DataContext.BulkMergeAsync(CompanyDAOs);
            await BuildPath();
            return true;
        }
        public async Task<bool> BulkDelete(List<Company> Companys)
        {
            List<long> Ids = Companys.Select(x => x.Id).ToList();
            await DataContext.Company
                .Where(x => Ids.Contains(x.Id))
                .DeleteFromQueryAsync();
            await BuildPath();
            return true;
        }
        public async Task<int> Count(CompanyFilter CompanyFilter)
        {
            IQueryable<CompanyDAO> Companys = DataContext.Company.AsNoTracking();
            Companys = DynamicFilter(Companys, CompanyFilter);
            return await Companys.CountAsync();
        }

        public async Task<List<Company>> List(CompanyFilter filter)
        {
            if (filter == null)
                return new List<Company>();
            IQueryable<CompanyDAO> CompanyDAOs = DataContext.Company.AsNoTracking();
            CompanyDAOs = DynamicFilter(CompanyDAOs, filter);
            CompanyDAOs = DynamicOrder(CompanyDAOs, filter);
            List<Company> Companys = await DynamicSelect(CompanyDAOs, filter);
            return Companys;
        }
        public async Task<List<Company>> List(List<long> Ids)
        {
            List<Company> Companys = await DataContext.Company.AsNoTracking()
                .Where(x => Ids.Contains(x.Id)).Select(x => new Company()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Level = x.Level,
                    ParentId = x.ParentId,
                    Parent = x.Parent == null ? null : new Company
                    {
                        Id = x.Parent.Id,
                        Code = x.Parent.Code,
                        Name = x.Parent.Name,
                        Level = x.Parent.Level,
                    }
                }).ToListAsync();

            return Companys;
        }
        private async Task BuildPath()
        {
            List<CompanyDAO> CompanyDAOs = await DataContext.Company
                .AsNoTracking().ToListAsync();
            Queue<CompanyDAO> queue = new Queue<CompanyDAO>();
            CompanyDAOs.ForEach(x =>
            {
                if (!x.ParentId.HasValue)
                {
                    x.Level = 1;
                    queue.Enqueue(x);
                }
            });
            while (queue.Count > 0)
            {
                CompanyDAO Parent = queue.Dequeue();
                foreach (CompanyDAO CompanyDAO in CompanyDAOs)
                {
                    if (CompanyDAO.ParentId == Parent.Id)
                    {
                        CompanyDAO.Level = Parent.Level + 1;
                        queue.Enqueue(CompanyDAO);
                    }
                }
            }
            await DataContext.BulkMergeAsync(CompanyDAOs);
        }
    }
}
