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
    internal class Payment_MatricesConfiguration : IEntityTypeConfiguration<CEN_payment_matrices>
    {
        public void Configure(EntityTypeBuilder<CEN_payment_matrices> builder)
        {
            builder.Property(p => p.Auxiliary_data).HasMaxLength(500);
            builder.Property(p => p.Created_ts).HasMaxLength(500);
            builder.Property(p => p.Updated_ts).HasMaxLength(500);
            builder.Property(p => p.Payment_type).HasMaxLength(500);
            builder.Property(p => p.Version);
            builder.Property(p => p.Payment_file).HasMaxLength(500);
            builder.Property(p => p.Letter_code).HasMaxLength(500);
            builder.Property(p => p.Letter_year);
            builder.Property(p => p.Letter_file).HasMaxLength(500);
            builder.Property(p => p.Matrix_file).HasMaxLength(500);
            builder.Property(p => p.Publish_date).HasMaxLength(500);
            builder.Property(p => p.Payment_days);
            builder.Property(p => p.Payment_date).HasMaxLength(500);
            builder.Property(p => p.Billing_date).HasMaxLength(500);
            builder.Property(p => p.Payment_window);
            builder.Property(p => p.Natural_key).HasMaxLength(500);
            builder.Property(p => p.Reference_code).HasMaxLength(500);
            builder.HasOne(m => m.CEN_billing_windows).WithMany().HasForeignKey(p => p.billing_window);
            builder.HasOne(m => m.CEN_payment_due_type).WithMany().HasForeignKey(p => p.payment_due_type);
        }
    }
}
