using System;
using System.Collections.Generic;

namespace HomeWork2.Models
{
    public partial class CompanyDAO
    {
        public CompanyDAO()
        {
            Budgets = new HashSet<BudgetDAO>();
            InverseParent = new HashSet<CompanyDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public long? Level { get; set; }

        public virtual CompanyDAO Parent { get; set; }
        public virtual ICollection<BudgetDAO> Budgets { get; set; }
        public virtual ICollection<CompanyDAO> InverseParent { get; set; }
    }
}
