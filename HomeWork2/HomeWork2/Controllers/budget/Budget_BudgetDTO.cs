using HomeWork2.Common;
using HomeWork2.Controllers.project;
using HomeWork2.Controllers.company;
using HomeWork2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork2.Controllers.budget_type;

namespace HomeWork2.Controllers.budget
{
    public class Budget_BudgetDTO : DataDTO
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long BudgetTypeId { get; set; }
        public long CompanyId { get; set; }
        public decimal Amount { get; set; }
        public decimal? AccumulatedAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Budget_ProjectDTO Project { get; set; }
        public Budget_BudgetTypeDTO BudgetType { get; set; }
        public Budget_CompanyDTO Company { get; set; }
        public Budget_BudgetDTO() { }
        public Budget_BudgetDTO(Budget Budget)
        {
            this.Id = Budget.Id;
            this.ProjectId = Budget.ProjectId;
            this.BudgetTypeId = Budget.BudgetTypeId;
            this.CompanyId = Budget.CompanyId;
            this.Amount = Budget.Amount;
            this.AccumulatedAmount = Budget.AccumulatedAmount;
            this.Year = Budget.Year;
            this.Month = Budget.Month;
            this.Project = new Budget_ProjectDTO(Budget.Project);
            this.BudgetType = new Budget_BudgetTypeDTO(Budget.BudgetType);
            this.Company = new Budget_CompanyDTO(Budget.Company);
            this.Errors = Budget.Errors;
        }
    }

    public class Budget_BudgetFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProjectId { get; set; }
        public IdFilter BudgetTypeId { get; set; }
        public IdFilter CompanyId { get; set; }
        public DecimalFilter Amount { get; set; }
        public DecimalFilter AccumulatedAmount { get; set; }
        public LongFilter Year { get; set; }
        public LongFilter Month { get; set; }
        public BudgetOrder OrderBy { get; set; }
        public BudgetSelect Selects { get; set; }
    }
}
