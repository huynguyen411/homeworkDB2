using HomeWork2.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Entities
{
    public class Project : DataEntity, IEquatable<Project>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool Equals(Project other)
        {
            return other != null && Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    public class ProjectFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public ProjectOrder OrderBy { get; set; }
        public ProjectSelect Selects { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        StartAt = 3,
        EndAt = 4,
    }

    [Flags]
    public enum ProjectSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        StartAt = E._3,
        EndAt = E._4,
    }
}
