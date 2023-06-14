using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class InstruccionesDefRelationSpecification : BaseSpecification<REACT_CEN_instructions_Def>
    {

        public InstruccionesDefRelationSpecification(int id, InstruccionesDefSpecificationParams parametros)
           : base(x => (x.Creditor == id || x.Debtor == id) &&
             (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
            (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
             (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
             (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
             (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
            (string.IsNullOrEmpty(parametros.conFolio) || x.Folio >0) &&
            (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
            (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
            (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
            (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
            (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
            (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
            (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
            (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
            (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
            (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&

            (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
            (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

            (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
            (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
            (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
             &&
             (
             !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
             ||

             (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

             x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

             ||

             (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
             !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
              && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
             ||
            (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
             x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
             x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
              && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

             )


            )
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.cEN_Payment_Matrices);
            AddInclude(p => p.Participants_creditor);
            AddInclude(p => p.Participants_debtor);

            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
            if (!string.IsNullOrEmpty(parametros.OrderByNeto))
            {
                if (parametros.OrderByNeto == "desc")
                {
                    AddOrderByDescending(p => p.Amount);
                }
                else if (parametros.OrderByNeto == "asc")
                {
                    AddOrderBy(p => p.Amount);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByBruto))
            {
                if (parametros.OrderByBruto == "desc")
                {
                    AddOrderByDescending(p => p.Amount_Gross);
                }
                else if (parametros.OrderByBruto == "asc")
                {
                    AddOrderBy(p => p.Amount_Gross);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaEmision))
            {
                if (parametros.OrderByFechaEmision == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_emision);
                }
                else if (parametros.OrderByFechaEmision == "asc")
                {
                    AddOrderBy(p => p.Fecha_emision);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaPago))
            {
                if (parametros.OrderByFechaPago == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_pago);
                }
                else if (parametros.OrderByFechaPago == "asc")
                {
                    AddOrderBy(p => p.Fecha_pago);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaCarta))
            {
                if (parametros.OrderByFechaCarta == "desc")
                {
                    AddOrderByDescending(p => p.cEN_Payment_Matrices.Publish_date);
                }
                else if (parametros.OrderByFechaCarta == "asc")
                {
                    AddOrderBy(p => p.cEN_Payment_Matrices.Publish_date);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFolio))
            {
                if (parametros.OrderByFolio == "desc")
                {
                    AddOrderByDescending(p => p.Folio);
                }
                else if (parametros.OrderByFolio == "asc")
                {
                    AddOrderBy(p => p.Folio);
                }
            }
            else
            {
                AddOrderByDescending(p => p.ID);
            }
        }

        public InstruccionesDefRelationSpecification(int id, int pa,InstruccionesDefSpecificationParams parametros)
          : base(x => (x.Creditor == id || x.Debtor == id) &&
            (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
           (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
            (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
            (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
            (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
           (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
           (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
           (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
           (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
           (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
           (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
           (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
           (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
           (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
           (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
           (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&

           (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
           (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

           (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
           (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
           (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
            &&
            (
            !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
            ||

            (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

            x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

            ||

            (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
            !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
            x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
             && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
            ||
           (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
            x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
            x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
             && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

            )


           )
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.cEN_Payment_Matrices);
            AddInclude(p => p.Participants_creditor);
            AddInclude(p => p.Participants_debtor);

            //ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
            if (!string.IsNullOrEmpty(parametros.OrderByNeto))
            {
                if (parametros.OrderByNeto == "desc")
                {
                    AddOrderByDescending(p => p.Amount);
                }
                else if (parametros.OrderByNeto == "asc")
                {
                    AddOrderBy(p => p.Amount);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByBruto))
            {
                if (parametros.OrderByBruto == "desc")
                {
                    AddOrderByDescending(p => p.Amount_Gross);
                }
                else if (parametros.OrderByBruto == "asc")
                {
                    AddOrderBy(p => p.Amount_Gross);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaEmision))
            {
                if (parametros.OrderByFechaEmision == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_emision);
                }
                else if (parametros.OrderByFechaEmision == "asc")
                {
                    AddOrderBy(p => p.Fecha_emision);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaPago))
            {
                if (parametros.OrderByFechaPago == "desc")
                {
                    AddOrderByDescending(p => p.Fecha_pago);
                }
                else if (parametros.OrderByFechaPago == "asc")
                {
                    AddOrderBy(p => p.Fecha_pago);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFechaCarta))
            {
                if (parametros.OrderByFechaCarta == "desc")
                {
                    AddOrderByDescending(p => p.cEN_Payment_Matrices.Publish_date);
                }
                else if (parametros.OrderByFechaCarta == "asc")
                {
                    AddOrderBy(p => p.cEN_Payment_Matrices.Publish_date);
                }
            }
            else if (!string.IsNullOrEmpty(parametros.OrderByFolio))
            {
                if (parametros.OrderByFolio == "desc")
                {
                    AddOrderByDescending(p => p.Folio);
                }
                else if (parametros.OrderByFolio == "asc")
                {
                    AddOrderBy(p => p.Folio);
                }
            }
            else
            {
                AddOrderByDescending(p => p.ID);
            }
        }

    }
}
