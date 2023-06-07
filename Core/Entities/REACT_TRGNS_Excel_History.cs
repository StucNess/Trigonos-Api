using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public  class REACT_TRGNS_Excel_History : TRGNS_base
    {
        public string excelName { get; set; }
        public string status { get; set; }
        public DateTime? date { get; set; }
        public int? idParticipant { get; set; }
        public string? type { get; set; }
        public string? description { get; set; }
    }
}
