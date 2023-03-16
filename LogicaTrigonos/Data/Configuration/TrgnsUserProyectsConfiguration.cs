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
    internal class TrgnsUserProyectsConfiguration: IEntityTypeConfiguration<TRGNS_UserProyects>
    {

    internal class TrgnsUserProyectsConfiguration : IEntityTypeConfiguration<TRGNS_UserProyects>
    {
        public void Configure(EntityTypeBuilder<TRGNS_UserProyects> builder)
        {
            builder.HasOne(p => p.cEN_Participants).WithMany().HasForeignKey(i => i.idProyect);



        }
    }
}
