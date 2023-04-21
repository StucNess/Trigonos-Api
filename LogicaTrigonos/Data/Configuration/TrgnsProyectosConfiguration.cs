﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data.Configuration
{
    internal class TrgnsProyectosConfiguration: IEntityTypeConfiguration<REACT_TRGNS_PROYECTOS>
    {
        public void Configure(EntityTypeBuilder<REACT_TRGNS_PROYECTOS> builder)
        {
            builder.HasOne(p => p.cEN_Participants).WithMany().HasForeignKey(i => i.Id_participants);

            

        }
    }
}
