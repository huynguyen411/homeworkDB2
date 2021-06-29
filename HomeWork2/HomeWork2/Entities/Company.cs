using HomeWork2.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Entities
{
    public class Company : DataEntity, IEquatable<Company>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public long? Level { get; set; }

        public Company Parent { get; set; }
        public bool Equals(Company other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class CompanyFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public LongFilter Level { get; set; }
        public CompanyOrder OrderBy { get; set; }
        public CompanySelect Selects { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CompanyOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Parent = 3,
        Level = 4,
    }

    [Flags]
    public enum CompanySelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Parent = E._3,
        Level = E._4,
    }
}
