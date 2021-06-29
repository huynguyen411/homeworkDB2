using HomeWork2.Common;
using HomeWork2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.project
{
    public class Budget_ProjectDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public Budget_ProjectDTO() { }
        public Budget_ProjectDTO(Project Project)
        {
            this.Id = Project.Id;
            this.Code = Project.Code;
            this.Name = Project.Name;
            this.StartAt = Project.StartAt;
            this.EndAt = Project.EndAt;
            this.Errors = Project.Errors;
        }
    }

    public class Budget_ProjectFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public ProjectOrder OrderBy { get; set; }
    }
}
