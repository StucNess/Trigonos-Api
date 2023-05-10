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
    internal class TrgnsRolPaginasConfiguration : IEntityTypeConfiguration<REACT_TRGNS_RolPaginas>
    {
        public void Configure(EntityTypeBuilder<REACT_TRGNS_RolPaginas> builder)
        {
            builder.HasOne(p => p.tRGNS_PaginasWeb).WithMany().HasForeignKey(i => i.Idpagina);
            builder.HasOne(x => x.rolAspNet).WithMany().HasForeignKey(z => z.Idrol);
        }
    }
}
