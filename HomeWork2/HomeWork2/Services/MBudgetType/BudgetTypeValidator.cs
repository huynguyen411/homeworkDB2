using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MBudgetType
{
    public interface IBudgetTypeValidator : IServiceScoped
    {
        Task<bool> Create(BudgetType BudgetType);
        Task<bool> Update(BudgetType BudgetType);
    }
    public class BudgetTypeValidator : IBudgetTypeValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted
        }
        private IUOW UOW;
        public BudgetTypeValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<bool> ValidateId(BudgetType BudgetType)
        {
            BudgetTypeFilter BudgetTypeFilter = new BudgetTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = BudgetType.Id },
                Selects = BudgetTypeSelect.Id
            };

            int count = await UOW.BudgetTypeRepository.Count(BudgetTypeFilter);
            if (count == 0)
                BudgetType.AddError(nameof(BudgetTypeValidator), nameof(BudgetType.Id), ErrorCode.IdNotExisted);
            return BudgetType.IsValidated;
        }

        public async Task<bool> ValidateCode(BudgetType BudgetType)
        {
            BudgetTypeFilter BudgetTypeFilter = new BudgetTypeFilter
            {
                Skip = 0,
                Take = 10,
                Code = new StringFilter { Equal = BudgetType.Code },
                Selects = BudgetTypeSelect.Code
            };
            int count = await UOW.BudgetTypeRepository.Count(BudgetTypeFilter);
            if (count != 0)
                BudgetType.AddError(nameof(BudgetTypeValidator), nameof(BudgetType.Code), ErrorCode.CodeExisted);
            return BudgetType.IsValidated;
        }
        public async Task<bool> Create(BudgetType BudgetType)
        {
            await ValidateCode(BudgetType);
            return BudgetType.IsValidated;
        }

        public async Task<bool> Update(BudgetType BudgetType)
        {
            if (await ValidateId(BudgetType))
            {
                await ValidateCode(BudgetType);
            }
            return BudgetType.IsValidated;
        }
    }
}
