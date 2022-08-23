using Core.Entities;
using Core.EntitiesPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data
{
    public class TrigonosDBContext : DbContext
    {
        public TrigonosDBContext(DbContextOptions<TrigonosDBContext> options) : base(options) { }
        public DbSet<CEN_banks> CEN_banks { get; set; }
        public DbSet<CEN_billing_status_type> CEN_billing_status_type { get; set; }
        public DbSet<CEN_billing_types> CEN_billing_types { get; set; }
        public DbSet<CEN_billing_windows> CEN_billing_windows { get; set; }
        public DbSet<CEN_dte_acceptance_status> CEN_dte_acceptance_status { get; set; }
        public DbSet<CEN_instructions> CEN_instructions { get; set; }
        public DbSet<CEN_Participants> CEN_Participants { get; set; }
        public DbSet<CEN_payment_due_type> CEN_payment_due_type { get; set; }
        public DbSet<CEN_payment_matrices> CEN_payment_matrices { get; set; }
        public DbSet<CEN_payment_status_type> CEN_payment_status_type { get; set; }
        public DbSet<CEN_transaction_types> CEN_transaction_types { get; set; }
        public DbSet<TRGNS_Datos_Facturacion> TRGNS_Datos_Facturacion { get; set; }
        //public DbSet<Patch_TRGNS_Datos_Facturacion> Patch_TRGNS_Datos_Facturacion { get; set; }

    
        public DbSet<TRGNS_dte_reception_status> TRGNS_dte_reception_status { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Patch_TRGNS_Datos_Facturacion>().ToTable("TRGNS_Datos_Facturacion");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
