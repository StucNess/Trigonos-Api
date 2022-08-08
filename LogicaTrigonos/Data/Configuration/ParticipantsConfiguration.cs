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
            builder.Property(p => p.Name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Rut).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Verification_Code).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Business_Name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Commercial_Business).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Dte_Reception_Email).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bank_Account).IsRequired().HasMaxLength(500);
            builder.HasOne(m => m.CEN_Banks).WithMany().HasForeignKey(p => p.bank);
            builder.Property(p => p.Commercial_address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Postal_address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Manager).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Pay_Contact_First_Name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Pay_contact_last_name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Pay_contact_address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Pay_contact_phones).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Pay_contact_email).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bills_contact_first_name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bills_contact_last_name).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bills_contact_address).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bills_contact_phones).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Bills_contact_email).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Created_ts).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Updated_ts).IsRequired().HasMaxLength(500);

        }
    }
}
