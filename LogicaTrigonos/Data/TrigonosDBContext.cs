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
        public TrigonosDBContext(DbContextOptions<TrigonosDBContext> options) : base(options) { }
        public DbSet<REACT_CEN_banks> REACT_CEN_banks { get; set; }
        public DbSet<REACT_CEN_billing_status_type> REACT_CEN_billing_status_type { get; set; }
        public DbSet<REACT_CEN_billing_types> REACT_CEN_billing_types { get; set; }
        public DbSet<REACT_CEN_billing_windows> REACT_CEN_billing_windows { get; set; }
        public DbSet<REACT_CEN_dte_acceptance_status> REACT_CEN_dte_acceptance_status { get; set; }
        public DbSet<REACT_CEN_instructions> REACT_CEN_instructions { get; set; }
        public DbSet<REACT_CEN_Participants> REACT_CEN_Participants { get; set; }
        public DbSet<REACT_CEN_payment_due_type> REACT_CEN_payment_due_type { get; set; }
        public DbSet<REACT_CEN_payment_matrices> REACT_CEN_payment_matrices { get; set; }
        public DbSet<REACT_CEN_payment_status_type> REACT_CEN_payment_status_type { get; set; }
        public DbSet<REACT_CEN_nonconformities> REACT_CEN_nonconformities { get; set; }
        public DbSet<REACT_CEN_transaction_types> REACT_CEN_transaction_types { get; set; }
        public DbSet<REACT_TRGNS_Datos_Facturacion> REACT_TRGNS_Datos_Facturacion { get; set; }
        
        public DbSet<REACT_TRGNS_PROYECTOS> REACT_TRGNS_PROYECTOS { get; set; }
        public DbSet<REACT_TRGNS_H_CEN_participants> REACT_TRGNS_H_CEN_participants { get; set; }

        public DbSet<REACT_TRGNS_dte_reception_status> REACT_TRGNS_dte_reception_status { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<REACT_TRGNS_H_Datos_Facturacion> REACT_TRGNS_H_Datos_Facturacion { get; set; }
        public DbSet<REACT_TRGNS_UserProyects> REACT_TRGNS_UserProyects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<CEN_billing_windows>().ToTable("CEN_billing_windows");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
