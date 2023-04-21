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
    internal class BillingWindowsConfiguration : IEntityTypeConfiguration<REACT_CEN_billing_windows>
    {
        public void Configure(EntityTypeBuilder<REACT_CEN_billing_windows> builder)
        {
            builder.HasOne(p => p.Billing_Types).WithMany().HasForeignKey(i => i.billing_type);
            builder.Property(p => p.natural_key);
            builder.Property(p => p.periods);
            builder.Property(p => p.created_ts);
            builder.Property(p => p.updated_ts);
            builder.Property(p => p.period);


        }

    }
}
