using HomeWork2.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Entities
{
    public class Budget : DataEntity, IEquatable<Budget>
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long BudgetTypeId { get; set; }
        public long CompanyId { get; set; }
        public decimal Amount { get; set; }
        public decimal? AccumulatedAmount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public Project Project { get; set; }
        public BudgetType BudgetType { get; set; }
        public Company Company { get; set; }
        public bool Equals(Budget other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class BudgetFilter : FilterEntity
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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BudgetOrder
    {
        Id = 0,
        Project = 1,
        BudgetType = 2,
        Company = 3,
        Amount = 4,
        Year = 5,
        Month = 6,
    }

    [Flags]
    public enum BudgetSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Project = E._1,
        BudgetType = E._2,
        Company = E._3,
        Amount = E._4,
        Year = E._5,
        Month = E._6,
    }
}
