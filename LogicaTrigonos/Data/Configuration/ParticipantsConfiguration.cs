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
    internal class ParticipantsConfiguration : IEntityTypeConfiguration<CEN_Participants>
    {
        public void Configure(EntityTypeBuilder<CEN_Participants> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(500);
            builder.Property(p => p.Rut).HasMaxLength(500);
            builder.Property(p => p.Verification_Code).HasMaxLength(500);
            builder.Property(p => p.Business_Name).HasMaxLength(500);
            builder.Property(p => p.Commercial_Business).HasMaxLength(500);
            builder.Property(p => p.Dte_Reception_Email).HasMaxLength(500);
            builder.Property(p => p.Bank_Account).HasMaxLength(500);
            builder.Property(p => p.Commercial_address).HasMaxLength(500);
            builder.Property(p => p.Postal_address).HasMaxLength(500);
            builder.Property(p => p.Manager).HasMaxLength(500);
            builder.Property(p => p.Pay_Contact_First_Name).HasMaxLength(500);
            builder.Property(p => p.Pay_contact_last_name).HasMaxLength(500);
            builder.Property(p => p.Pay_contact_address).HasMaxLength(500);
            builder.Property(p => p.Pay_contact_phones).HasMaxLength(500);
            builder.Property(p => p.Pay_contact_email).HasMaxLength(500);
            builder.Property(p => p.Bills_contact_first_name).HasMaxLength(500);
            builder.Property(p => p.Bills_contact_last_name).HasMaxLength(500);
            builder.Property(p => p.Bills_contact_address).HasMaxLength(500);
            builder.Property(p => p.Bills_contact_phones).HasMaxLength(500);
            builder.Property(p => p.Bills_contact_email).HasMaxLength(500);
            builder.Property(p => p.Created_ts).HasMaxLength(500);
            builder.Property(p => p.Updated_ts).HasMaxLength(500);
            builder.HasOne(m => m.CEN_banks).WithMany().HasForeignKey(p => p.bank);
        }
    }
}
