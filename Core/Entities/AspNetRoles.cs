﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AspNetRoles
    {
        public string? Id { get; set; }

        public string? Descripcion { get; set; }
        public int? Bhabilitado { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

    }
}
