using Core.Entities;
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
        public TrigonosDBContext(DbContextOptions<TrigonosDBContext> options): base(options) { }
        public DbSet<CEN_banks> CEN_banks { get; set; }
        public DbSet<CEN_billing_status_type> CEN_BillingStatus { get; set; }
        public DbSet<CEN_billing_types> CEN_BillingTypes { get; set; }
        public DbSet<CEN_billing_windows> CEN_BillingWindows { get; set; }
        public DbSet<CEN_dte_acceptance_status> CEN_DTEAcceptanceStatus { get; set; }
        public DbSet<CEN_instructions> CEN_Instructions { get; set; }
        public DbSet<CEN_Participants> CEN_participants { get; set; }
        public DbSet<CEN_payment_due_type> CEN_PaymentDueType { get; set; }
        public DbSet<CEN_payment_matrices> CEN_PaymentMatrices { get; set; }
        public DbSet<CEN_payment_status_type> CEN_Payment_Status_Types { get; set; }
        public DbSet<CEN_transaction_types> CEN_TransactionTypes { get; set; }
        public DbSet<TRGNS_Datos_Facturacion> TRGNS_DatosFacturacion { get; set; }
        public DbSet<TRGNS_dte_reception_status> TRGNS_DteReceptionstatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
