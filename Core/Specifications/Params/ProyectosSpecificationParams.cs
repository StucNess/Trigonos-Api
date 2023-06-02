using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class ProyectosSpecificationParams
    {
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 1000;
        private int _pageSize = 3;
        public int PageSize { get => _pageSize; set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        public string? Search { get; set; }
    }
}
