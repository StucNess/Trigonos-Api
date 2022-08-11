using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class InstruccionesForCountingSpecification:BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public InstruccionesForCountingSpecification(int id, InstruccionesSpecificationParams productoParams)

             : base(x => (x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id) &&
            (string.IsNullOrEmpty(productoParams.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name.Contains(productoParams.EstadoAceptacion))&&
             (string.IsNullOrEmpty(productoParams.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name.Contains(productoParams.EstadoRecepcion)) &&
             (string.IsNullOrEmpty(productoParams.EstadoEmision) || x.CEN_billing_status_type.Name.Contains(productoParams.EstadoEmision)) &&
             (string.IsNullOrEmpty(productoParams.EstadoPago) || x.CEN_payment_status_type.Name.Contains(productoParams.EstadoPago)) &&
             (string.IsNullOrEmpty(productoParams.NombreAcreedor) || x.CEN_instruction.Participants_creditor.Business_Name.Contains(productoParams.NombreAcreedor)) &&
            (!productoParams.Folio.HasValue || x.Folio == productoParams.Folio)

            )
        {

        }
    }
}
