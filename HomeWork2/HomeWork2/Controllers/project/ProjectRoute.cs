using HomeWork2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeWork2.Controllers.project
{
    public class ProjectRoute : Root
    {
        private const string Default = Rpc + Module + "/project";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string BulkMerge = Default + "/bulk-merge";
        public const string BulkDelete = Default + "/bulk-delete";
    }
}
