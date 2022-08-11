﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class InstruccionesRelationSpecification : BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public InstruccionesRelationSpecification(int id, InstruccionesSpecificationParams productoParams)

            : base(x => (x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id) &&
            (string.IsNullOrEmpty(productoParams.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name.Contains(productoParams.EstadoAceptacion)) &&
             (string.IsNullOrEmpty(productoParams.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name.Contains(productoParams.EstadoRecepcion)) &&
             (string.IsNullOrEmpty(productoParams.EstadoEmision) || x.CEN_billing_status_type.Name.Contains(productoParams.EstadoEmision)) &&
             (string.IsNullOrEmpty(productoParams.EstadoPago) || x.CEN_payment_status_type.Name.Contains(productoParams.EstadoPago)) &&
            (string.IsNullOrEmpty(productoParams.NombreAcreedor) || x.CEN_instruction.Participants_creditor.Business_Name.Contains(productoParams.NombreAcreedor)) &&
            (string.IsNullOrEmpty(productoParams.NombreDeudor) || x.CEN_instruction.Participants_debtor.Business_Name.Contains(productoParams.NombreDeudor)) &&
            (string.IsNullOrEmpty(productoParams.RutAcreedor) || x.CEN_instruction.Participants_creditor.Rut.Contains(productoParams.RutAcreedor)) &&
            (string.IsNullOrEmpty(productoParams.RutDeudor) || x.CEN_instruction.Participants_debtor.Rut.Contains(productoParams.RutDeudor)) &&
            (!productoParams.FechaRecepcion.HasValue || x.Fecha_recepcion == productoParams.FechaRecepcion) &&
            (!productoParams.FechaAceptacion.HasValue || x.Fecha_recepcion == productoParams.FechaAceptacion) &&
            (!productoParams.FechaPago.HasValue || x.Fecha_recepcion == productoParams.FechaPago) &&
            (!productoParams.FechaEmision.HasValue || x.Fecha_recepcion == productoParams.FechaEmision) &&
            (string.IsNullOrEmpty(productoParams.Concepto) || x.CEN_instruction.Payment_matrix_concept.Contains(productoParams.Concepto)) &&
            (!productoParams.MontoNeto.HasValue||x.CEN_instruction.Amount == productoParams.MontoNeto)&&
            (!productoParams.MontoBruto.HasValue || x.CEN_instruction.Amount_Gross == productoParams.MontoBruto) &&
            (! productoParams.Folio.HasValue || x.Folio == productoParams.Folio) 

            )
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);

            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            AddInclude(p => p.CEN_instruction.Participants_debtor);
            
            ApplyPaging(productoParams.PageSize * (productoParams.PageIndex - 1), productoParams.PageSize);


        }
    }
}
