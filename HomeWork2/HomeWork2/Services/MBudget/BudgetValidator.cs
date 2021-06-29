using HomeWork2.Common;
using HomeWork2.Entities;
using HomeWork2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Services.MBudget
{
    public interface IBudgetValidator : IServiceScoped
    {
        Task<bool> Create(Budget Budget);
        Task<bool> Update(Budget Budget);
    }
    public class BudgetValidator : IBudgetValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            ProjectNotExisted,
            CompanyNotExisted,
            BudgetTypeNotExisted,
            AmountInvalid
        }
        private IUOW UOW;
        public BudgetValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<bool> ValidateId(Budget Budget)
        {
            BudgetFilter BudgetFilter = new BudgetFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Budget.Id },
                Selects = BudgetSelect.Id
            };

            int count = await UOW.BudgetRepository.Count(BudgetFilter);
            if (count == 0)
                Budget.AddError(nameof(BudgetValidator), nameof(Budget.Id), ErrorCode.IdNotExisted);
            return Budget.IsValidated;
        }

        public async Task<bool> ValidateProject(Budget Budget)
        {
            ProjectFilter ProjectFilter = new ProjectFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Budget.ProjectId },
                Selects = ProjectSelect.Id
            };

            int count = await UOW.ProjectRepository.Count(ProjectFilter);
            if (count == 0)
                Budget.AddError(nameof(BudgetValidator), nameof(Budget.Project), ErrorCode.ProjectNotExisted);
            return Budget.IsValidated;
        }

        public async Task<bool> ValidateCompany(Budget Budget)
        {
            CompanyFilter CompanyFilter = new CompanyFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Budget.CompanyId },
                Selects = CompanySelect.Id
            };

            int count = await UOW.CompanyRepository.Count(CompanyFilter);
            if (count == 0)
                Budget.AddError(nameof(BudgetValidator), nameof(Budget.Company), ErrorCode.CompanyNotExisted);
            return Budget.IsValidated;
        }

        public async Task<bool> ValidateBudgetType(Budget Budget)
        {
            BudgetTypeFilter BudgetTypeFilter = new BudgetTypeFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Budget.BudgetTypeId },
                Selects = BudgetTypeSelect.Id
            };

            int count = await UOW.BudgetTypeRepository.Count(BudgetTypeFilter);
            if (count == 0)
                Budget.AddError(nameof(BudgetValidator), nameof(Budget.BudgetType), ErrorCode.BudgetTypeNotExisted);
            return Budget.IsValidated;
        }

        public async Task<bool> ValidateAmount(Budget Budget)
        {
            if (Budget.Amount <= 0)
            {
                Budget.AddError(nameof(BudgetValidator), nameof(Budget.Amount), ErrorCode.AmountInvalid);
            }
            return Budget.IsValidated;
        }

        public async Task<bool> Create(Budget Budget)
        {
            await ValidateProject(Budget);
            await ValidateCompany(Budget);
            await ValidateBudgetType(Budget);
            await ValidateAmount(Budget);
            return Budget.IsValidated;
        }

        public async Task<bool> Update(Budget Budget)
        {
            if (await ValidateId(Budget))
            {
                await ValidateProject(Budget);
                await ValidateCompany(Budget);
                await ValidateBudgetType(Budget);
                await ValidateAmount(Budget);
            }
            return Budget.IsValidated;
        }
    }
}
