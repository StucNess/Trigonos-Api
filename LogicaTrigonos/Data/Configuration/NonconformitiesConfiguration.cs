using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data.Configuration
{
    internal class NonconformitiesConfiguration : IEntityTypeConfiguration<REACT_CEN_nonconformities>
    {
        public void Configure(EntityTypeBuilder<REACT_CEN_nonconformities> builder)
        {
            builder.HasOne(i => i.CEN_Instructions).WithMany().HasForeignKey(p => p.ID);
        }
    }
}
