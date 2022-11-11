using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class InstruccionesForCountingSpecification : BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public InstruccionesForCountingSpecification(int id, InstruccionesSpecificationParams productoParams)

             : base(x => (x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id) &&
              (string.IsNullOrEmpty(productoParams.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == productoParams.EstadoAceptacion) &&
             (string.IsNullOrEmpty(productoParams.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == productoParams.EstadoRecepcion) &&
             (string.IsNullOrEmpty(productoParams.EstadoEmision) || x.CEN_billing_status_type.Name == productoParams.EstadoEmision) &&
             (string.IsNullOrEmpty(productoParams.EstadoPago) || x.CEN_payment_status_type.Name == productoParams.EstadoPago) &&
             (string.IsNullOrEmpty(productoParams.NombreAcreedor) || x.CEN_instruction.Participants_creditor.Business_Name.Contains(productoParams.NombreAcreedor)) &&
             (string.IsNullOrEmpty(productoParams.NombreDeudor) || x.CEN_instruction.Participants_debtor.Business_Name.Contains(productoParams.NombreDeudor)) &&
             (string.IsNullOrEmpty(productoParams.RutAcreedor) || x.CEN_instruction.Participants_creditor.Rut.Contains(productoParams.RutAcreedor)) &&
             (string.IsNullOrEmpty(productoParams.RutDeudor) || x.CEN_instruction.Participants_debtor.Rut.Contains(productoParams.RutDeudor)) &&
             (string.IsNullOrEmpty(productoParams.Glosa) || x.CEN_instruction.Payment_matrix_natural_key.Contains(productoParams.Glosa)) &&
             (string.IsNullOrEmpty(productoParams.Concepto) || x.CEN_instruction.Payment_matrix_concept.Contains(productoParams.Concepto)) &&
             (!productoParams.FechaRecepcion.HasValue || x.Fecha_recepcion == productoParams.FechaRecepcion) &&
            (!productoParams.FechaAceptacion.HasValue || x.Fecha_recepcion == productoParams.FechaAceptacion) &&
            (!productoParams.FechaPago.HasValue || x.Fecha_recepcion == productoParams.FechaPago) &&
            (!productoParams.FechaEmision.HasValue || x.Fecha_recepcion == productoParams.FechaEmision) &&
              (!productoParams.InicioPeriodo.HasValue || x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period >= productoParams.InicioPeriodo) &&
            (!productoParams.TerminoPeriodo.HasValue || x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period <= productoParams.TerminoPeriodo) &&
             (!productoParams.Acreedor.HasValue || x.CEN_instruction.Creditor == productoParams.Acreedor) &&
            (!productoParams.Deudor.HasValue || x.CEN_instruction.Debtor == productoParams.Deudor) &&
            (!productoParams.MontoNeto.HasValue || x.CEN_instruction.Amount >= productoParams.MontoNeto) &&
            (!productoParams.MontoBruto.HasValue || x.CEN_instruction.Amount_Gross >= productoParams.MontoBruto) &&
            (!productoParams.Folio.HasValue || x.Folio == productoParams.Folio)

            )
        {

        }
    }
}
