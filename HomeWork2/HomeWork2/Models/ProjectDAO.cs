using System;
using System.Collections.Generic;

namespace HomeWork2.Models
{
    public partial class ProjectDAO
    {
        public ProjectDAO()
        {
            Budgets = new HashSet<BudgetDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public virtual ICollection<BudgetDAO> Budgets { get; set; }
    }
}
