using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class InstruccionesRelationSpecification : BaseSpecification<REACT_TRGNS_Datos_Facturacion>
    {
        public InstruccionesRelationSpecification(int id, InstruccionesSpecificationParams productoParams)
              //
              : base(x => (x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id) &&
             (!productoParams.Acreedor.HasValue || x.CEN_instruction.Creditor == productoParams.Acreedor) &&
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
             (string.IsNullOrEmpty(productoParams.Carta) || x.CEN_instruction.cEN_Payment_Matrices.Letter_code == productoParams.Carta) &&
              (string.IsNullOrEmpty(productoParams.CodigoRef)|| x.CEN_instruction.cEN_Payment_Matrices.Reference_code == productoParams.CodigoRef) &&
             (!productoParams.FechaEmision.HasValue || x.Fecha_recepcion == productoParams.FechaEmision) &&


             (
             !productoParams.InicioPeriodo.HasValue && !productoParams.TerminoPeriodo.HasValue
             ||

             (productoParams.InicioPeriodo.HasValue && !productoParams.TerminoPeriodo.HasValue &&

             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period == productoParams.InicioPeriodo)

             ||

             (productoParams.TerminoPeriodo.HasValue && productoParams.InicioPeriodo.HasValue &&
             !x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period <= productoParams.TerminoPeriodo
              && x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period >= productoParams.InicioPeriodo)
             ||
            (productoParams.TerminoPeriodo.HasValue && productoParams.InicioPeriodo.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end <= productoParams.TerminoPeriodo
              && x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period >= productoParams.InicioPeriodo)

             )
             &&
            (!productoParams.Acreedor.HasValue || x.CEN_instruction.Creditor == productoParams.Acreedor) &&
            (!productoParams.Deudor.HasValue || x.CEN_instruction.Debtor == productoParams.Deudor) &&

            (!productoParams.MontoNeto.HasValue || x.CEN_instruction.Amount >= productoParams.MontoNeto) &&
            (!productoParams.MontoBruto.HasValue || x.CEN_instruction.Amount_Gross >= productoParams.MontoBruto) &&
            (!productoParams.Folio.HasValue || x.Folio == productoParams.Folio)

            )
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);

            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            
            AddInclude(p => p.CEN_instruction.Participants_debtor);
            ApplyPaging(productoParams.PageSize * (productoParams.PageIndex - 1), productoParams.PageSize);
            
            if  (!string.IsNullOrEmpty(productoParams.OrderByNeto))
            {
                if (productoParams.OrderByNeto == "desc")
                {
                    AddOrderByDescending(p => p.CEN_instruction.Amount);
                }
                else if (productoParams.OrderByNeto == "asc")
                {
                    AddOrderBy(p => p.CEN_instruction.Amount);
                }
            }
            else if (!string.IsNullOrEmpty(productoParams.OrderByBruto))
            {
                if (productoParams.OrderByBruto == "desc")
                {
                    AddOrderByDescending(p => p.CEN_instruction.Amount_Gross);
                }
                else if (productoParams.OrderByBruto == "asc")
                {
                    AddOrderBy(p => p.CEN_instruction.Amount_Gross);
                }
            }
            else if (!string.IsNullOrEmpty(productoParams.OrderByFechaEmision))
            {
                if (productoParams.OrderByFechaEmision == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_emision);
                }
                else if (productoParams.OrderByFechaEmision == "asc")
                {
                    AddOrderBy(p => p.Fecha_emision);
                }
            }
            else if (!string.IsNullOrEmpty(productoParams.OrderByFechaPago))
            {
                if (productoParams.OrderByFechaPago == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_pago);
                }
                else if (productoParams.OrderByFechaPago == "asc")
                {
                    AddOrderBy(p => p.Fecha_pago);
                }
            }
            else if (!string.IsNullOrEmpty(productoParams.OrderByFechaCarta))
            {
                if (productoParams.OrderByFechaCarta == "desc")
                {
                    AddOrderByDescending(p => p.CEN_instruction.cEN_Payment_Matrices.Publish_date);
                }
                else if (productoParams.OrderByFechaCarta == "asc")
                {
                    AddOrderBy(p => p.CEN_instruction.cEN_Payment_Matrices.Publish_date);
                }
            }
            else if (!string.IsNullOrEmpty(productoParams.OrderByFolio))
            {
                if (productoParams.OrderByFolio == "desc")
                {
                    AddOrderByDescending(p => p.Folio);
                }
                else if (productoParams.OrderByFolio == "asc")
                {
                    AddOrderBy(p => p.Folio);
                }
            } 
            else
            {
                AddOrderByDescending(p => p.id_instructions);
            }


        }
        public InstruccionesRelationSpecification(InstruccionesSpecificationParams productoParams)
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            AddInclude(p => p.CEN_instruction.Participants_debtor);

            ApplyPaging(productoParams.PageSize * (productoParams.PageIndex - 1), productoParams.PageSize);
        }
        public InstruccionesRelationSpecification(int id,int pa ,InstruccionesSpecificationParams productoParams)

             : base(x => (x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id) &&
             (!productoParams.Acreedor.HasValue || x.CEN_instruction.Creditor == productoParams.Acreedor) &&
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
               
            (!productoParams.Acreedor.HasValue || x.CEN_instruction.Creditor == productoParams.Acreedor) &&
            (!productoParams.Deudor.HasValue || x.CEN_instruction.Debtor == productoParams.Deudor) &&

            (!productoParams.MontoNeto.HasValue || x.CEN_instruction.Amount >= productoParams.MontoNeto) &&
            (!productoParams.MontoBruto.HasValue || x.CEN_instruction.Amount_Gross >= productoParams.MontoBruto) &&
            (!productoParams.Folio.HasValue || x.Folio == productoParams.Folio)
             &&
             (
             !productoParams.InicioPeriodo.HasValue && !productoParams.TerminoPeriodo.HasValue
             ||

             (productoParams.InicioPeriodo.HasValue && !productoParams.TerminoPeriodo.HasValue &&

             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period == productoParams.InicioPeriodo)

             ||

             (productoParams.TerminoPeriodo.HasValue && productoParams.InicioPeriodo.HasValue &&
             !x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period <= productoParams.TerminoPeriodo
              && x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period >= productoParams.InicioPeriodo)
             ||
            (productoParams.TerminoPeriodo.HasValue && productoParams.InicioPeriodo.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period_end <= productoParams.TerminoPeriodo
              && x.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period >= productoParams.InicioPeriodo)

             )
           

            )   


        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction);
            AddOrderByDescending(p => p.id_instructions);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            AddInclude(p => p.CEN_instruction.Participants_debtor);



        }
    }
}
