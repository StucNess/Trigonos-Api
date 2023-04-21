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
    internal class InstruccionsConfiguration : IEntityTypeConfiguration<REACT_CEN_instructions>
    {
        public void Configure(EntityTypeBuilder<REACT_CEN_instructions> builder)
        {
            builder.HasOne(m => m.cEN_Payment_Matrices).WithMany().HasForeignKey(p => p.Payment_matrix);
            builder.HasOne(c => c.Participants_creditor).WithMany().HasForeignKey(p => p.Creditor);
            builder.HasOne(d => d.Participants_debtor).WithMany().HasForeignKey(p => p.Debtor);
            builder.Property(p => p.Amount);
            builder.Property(p => p.Amount_Gross);
            builder.Property(p => p.Closed);
            builder.Property(p => p.Status).HasMaxLength(100);
            builder.Property(p => p.Status_billed);
            builder.Property(p => p.Status_paid);
            builder.Property(p => p.Resolution).HasMaxLength(500);
            builder.Property(p => p.Max_payment_date).HasMaxLength(500);
            builder.Property(p => p.Informed_paid_amount);
            builder.Property(p => p.Is_paid);
            builder.Property(p => p.Payment_matrix_natural_key).HasMaxLength(500);
            builder.Property(p => p.Payment_matrix_concept).HasMaxLength(500);
            builder.Property(p => p.accept_partial_payments);
            builder.Property(p => p.Created_ts).HasMaxLength(500);
            builder.Property(p => p.Updated_ts).HasMaxLength(500);
            builder.Property(p => p.Trgns_Status_Instructions);

        }
    }
}
