using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MCompany
{
    public interface ICompanyValidator : IServiceScoped
    {
        Task<bool> Create(Company Company);
        Task<bool> Update(Company Company);
    }
    public class CompanyValidator : ICompanyValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted
        }
        private IUOW UOW;
        public CompanyValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<bool> ValidateId(Company Company)
        {
            CompanyFilter CompanyFilter = new CompanyFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Company.Id },
                Selects = CompanySelect.Id
            };

            int count = await UOW.CompanyRepository.Count(CompanyFilter);
            if (count == 0)
                Company.AddError(nameof(CompanyValidator), nameof(Company.Id), ErrorCode.IdNotExisted);
            return Company.IsValidated;
        }

        public async Task<bool> ValidateCode(Company Company)
        {
            CompanyFilter CompanyFilter = new CompanyFilter
            {
                Skip = 0,
                Take = 10,
                Code = new StringFilter { Equal = Company.Code },
                Selects = CompanySelect.Code
            };
            int count = await UOW.CompanyRepository.Count(CompanyFilter);
            if (count != 0)
                Company.AddError(nameof(CompanyValidator), nameof(Company.Code), ErrorCode.CodeExisted);
            return Company.IsValidated;
        }
        public async Task<bool> Create(Company Company)
        {
            await ValidateCode(Company);
            return Company.IsValidated;
        }

        public async Task<bool> Update(Company Company)
        {
            if (await ValidateId(Company))
            {
                await ValidateCode(Company);
            }
            return Company.IsValidated;
        }
    }
}
