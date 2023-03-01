using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TRGNS_UserProyects:TRGNS_base
    {
       
        public int idProyect { get; set; }
        public CEN_Participants? cEN_Participants { get; set; }
        public string idUser { get; set; }
    }
}
