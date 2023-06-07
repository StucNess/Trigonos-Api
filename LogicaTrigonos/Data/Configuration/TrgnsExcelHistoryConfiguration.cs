using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data.Configuration
{
    internal class TrgnsExcelHistoryConfiguration : IEntityTypeConfiguration<REACT_TRGNS_Excel_History>
    {
        public void Configure(EntityTypeBuilder<REACT_TRGNS_Excel_History> builder)
        {
            
            builder.Property(p => p.status);
            builder.Property(p => p.date);
            builder.Property(p => p.excelName);
            builder.Property(p => p.idParticipant); 


        }

    }
}
