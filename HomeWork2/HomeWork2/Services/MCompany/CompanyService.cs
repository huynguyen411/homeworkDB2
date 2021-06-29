using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using HomeWork2.Services.MCompany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MBudget
{
    public interface ICompanyService : IServiceScoped
    {
        Task<int> Count(CompanyFilter CompanyFilter);
        Task<List<Company>> List(CompanyFilter CompanyFilter);
        Task<Company> Get(long Id);
        Task<Company> Create(Company Company);
        Task<Company> Update(Company Company);
        Task<Company> Delete(Company Company);
        Task<List<Company>> BulkMerge(List<Company> Companies);
        Task<List<Company>> BulkDelete(List<Company> Companies);
    }
    public class CompanyService : ICompanyService
    {
        private IUOW UOW;
        private ICompanyValidator CompanyValidator;
        public CompanyService(IUOW UOW, ICompanyValidator CompanyValidator)
        {
            this.UOW = UOW;
            this.CompanyValidator = CompanyValidator;
        }
        public async Task<int> Count(CompanyFilter CompanyFilter)
        {
            try
            {
                int result = await UOW.CompanyRepository.Count(CompanyFilter);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Company>> List(CompanyFilter CompanyFilter)
        {
            try
            {
                List<Company> Companys = await UOW.CompanyRepository.List(CompanyFilter);
                return Companys;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<Company> Get(long Id)
        {
            Company Company = await UOW.CompanyRepository.Get(Id);
            if (Company == null)
                return null;
            return Company;
        }
        public async Task<Company> Create(Company Company)
        {
            if (!await CompanyValidator.Create(Company))
                return Company;
            try
            {
                await UOW.Begin();
                await UOW.CompanyRepository.Create(Company);
                await UOW.Commit();

                var Companys = await UOW.CompanyRepository.List(new List<long> { Company.Id });
                Company = Companys.FirstOrDefault();
                return Company;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }

        }
        public async Task<Company> Update(Company Company)
        {
            if (!await CompanyValidator.Update(Company))
                return Company;
            try
            {
                await UOW.Begin();
                await UOW.CompanyRepository.Update(Company);
                await UOW.Commit();

                var Companys = await UOW.CompanyRepository.List(new List<long> { Company.Id });
                Company = Companys.FirstOrDefault();
                return Company;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<Company> Delete(Company Company)
        {
            try
            {
                await UOW.Begin();
                await UOW.CompanyRepository.Delete(Company);
                await UOW.Commit();

                var Companies = await UOW.CompanyRepository.List(new List<long> { Company.Id });
                Company = Companies.FirstOrDefault();
                return Company;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Company>> BulkMerge(List<Company> Companies)
        {
            try
            {
                await UOW.Begin();
                await UOW.CompanyRepository.BulkMerge(Companies);
                await UOW.Commit();

                var Ids = Companies.Select(x => x.Id).ToList();
                Companies = await UOW.CompanyRepository.List(Ids);
                return Companies;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
        public async Task<List<Company>> BulkDelete(List<Company> Companies)
        {
            try
            {
                await UOW.Begin();
                await UOW.CompanyRepository.BulkDelete(Companies);
                await UOW.Commit();

                var Ids = Companies.Select(x => x.Id).ToList();
                Companies = await UOW.CompanyRepository.List(Ids);
                return Companies;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();
                if (ex.InnerException == null)
                {
                    throw new MessageException(ex);
                }
                else
                {
                    throw new MessageException(ex.InnerException);
                }
            }
        }
    }
}
