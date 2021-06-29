using System;
using System.Collections.Generic;

namespace HomeWork2.Models
{
    public partial class BudgetDAO
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long BudgetTypeId { get; set; }
        public long CompanyId { get; set; }
        public decimal Amount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public virtual BudgetTypeDAO BudgetType { get; set; }
        public virtual CompanyDAO Company { get; set; }
        public virtual ProjectDAO Project { get; set; }
    }
}
