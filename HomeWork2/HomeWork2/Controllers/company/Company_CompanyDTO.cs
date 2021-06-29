﻿using HomeWork2.Common;
using HomeWork2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.company
{
    public class Company_CompanyDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public long? Level { get; set; }
        public Company_CompanyDTO Parent { get; set; }
        public Company_CompanyDTO() { }
        public Company_CompanyDTO(Company Company)
        {
            this.Id = Company.Id;
            this.Code = Company.Code;
            this.Name = Company.Name;
            this.ParentId = Company.ParentId;
            this.Level = Company.Level;
            this.Parent = Company.Parent == null ? null : new Company_CompanyDTO(Company.Parent);
            this.Errors = Company.Errors;
        }
    }

    public class Company_CompanyFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public LongFilter Level { get; set; }
        public CompanyOrder OrderBy { get; set; }
        public CompanySelect Selects { get; set; }
    }
}
