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
    internal class TrgnsFacturacionConfiguration : IEntityTypeConfiguration<REACT_TRGNS_Datos_Facturacion>
    {
        public void Configure(EntityTypeBuilder<REACT_TRGNS_Datos_Facturacion> builder)
        {
            builder.HasOne(i => i.CEN_instruction).WithMany().HasForeignKey(p => p.id_instructions);
            builder.HasOne(b => b.CEN_billing_status_type).WithMany().HasForeignKey(p => p.Estado_emision);
            builder.HasOne(r => r.TRGNS_dte_reception_status).WithMany().HasForeignKey(p => p.Estado_recepcion);
            builder.HasOne(o => o.CEN_payment_status_type).WithMany().HasForeignKey(p => p.Estado_pago);
            builder.HasOne(a => a.CEN_dte_acceptance_status).WithMany().HasForeignKey(p => p.Estado_aceptacion);
            builder.HasOne(a => a.CEN_nonconformities).WithMany().HasForeignKey(p => p.nonconformitie);
            builder.Property(p => p.Fecha_emision);
            builder.Property(p => p.Fecha_recepcion);
            builder.Property(p => p.Fecha_pago);
            builder.Property(p => p.Fecha_aceptacion);
            builder.Property(p => p.tipo_instructions);
            builder.Property(p => p.Folio);


        }
    }
}
