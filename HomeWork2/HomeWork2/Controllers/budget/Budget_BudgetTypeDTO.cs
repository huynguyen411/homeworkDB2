using HomeWork2.Common;
using HomeWork2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.budget_type
{
    public class Budget_BudgetTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Budget_BudgetTypeDTO() { }
        public Budget_BudgetTypeDTO(BudgetType BudgetType)
        {
            this.Id = BudgetType.Id;
            this.Code = BudgetType.Code;
            this.Name = BudgetType.Name;
            this.Errors = BudgetType.Errors;
        }
    }

    public class Budget_BudgetTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public BudgetTypeOrder OrderBy { get; set; }
        public BudgetTypeSelect Selects { get; set; }
    }
}
