﻿using AutoMapper;
using Core.Entities;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergy.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<REACT_CEN_payment_matrices, FolioDto>()
                .ForMember(p => p.label, x => x.MapFrom(a => a.Natural_key));
            CreateMap<REACT_CEN_Participants, RutDto>()
                .ForMember(p => p.label, x => x.MapFrom(a => String.Concat(a.Rut, '-', a.Verification_Code)));
            CreateMap<REACT_CEN_Participants, BusinessNameDto>()
                .ForMember(p => p.label, x => x.MapFrom(a => a.Business_Name));
            CreateMap<REACT_CEN_Participants, ParticipantesDTO>()
                .ForMember(p => p.BanksName, x => x.MapFrom(a => a.CEN_banks.Name))
                 .ForMember(p => p.RutCompleto, x => x.MapFrom(a => String.Concat(a.Rut, '-', a.Verification_Code)));
            CreateMap<REACT_TRGNS_PROYECTOS, ParticipantesDTO>()
                .ForMember(p => p.Id, x => x.MapFrom(a => a.cEN_Participants.ID))
                .ForMember(p => p.Name, x => x.MapFrom(a => a.cEN_Participants.Name))
                .ForMember(p => p.Rut, x => x.MapFrom(a => a.cEN_Participants.Rut))
                .ForMember(p => p.RutCompleto, x => x.MapFrom(a => String.Concat(a.cEN_Participants.Rut, '-', a.cEN_Participants.Verification_Code)))
                .ForMember(p => p.Verification_Code, x => x.MapFrom(a => a.cEN_Participants.Verification_Code))
                .ForMember(p => p.Business_Name, x => x.MapFrom(a => a.cEN_Participants.Business_Name))
                .ForMember(p => p.Commercial_Business, x => x.MapFrom(a => a.cEN_Participants.Commercial_Business))
                .ForMember(p => p.Dte_Reception_Email, x => x.MapFrom(a => a.cEN_Participants.Dte_Reception_Email))
                .ForMember(p => p.Bank_Account, x => x.MapFrom(a => a.cEN_Participants.Bank_Account))
                .ForMember(p => p.bank, x => x.MapFrom(a => a.cEN_Participants.bank))
                .ForMember(p => p.BanksName, x => x.MapFrom(a => a.cEN_Participants.CEN_banks.Name))
                .ForMember(p => p.Commercial_address, x => x.MapFrom(a => a.cEN_Participants.Commercial_address))
                .ForMember(p => p.Postal_address, x => x.MapFrom(a => a.cEN_Participants.Postal_address))
                .ForMember(p => p.Manager, x => x.MapFrom(a => a.cEN_Participants.Manager))
                .ForMember(p => p.Pay_Contact_First_Name, x => x.MapFrom(a => a.cEN_Participants.Pay_Contact_First_Name))
                .ForMember(p => p.Pay_contact_last_name, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_last_name))
                .ForMember(p => p.Pay_contact_address, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_address))
                .ForMember(p => p.Pay_contact_phones, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_phones))
                .ForMember(p => p.Pay_contact_email, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_email))
                .ForMember(p => p.Bills_contact_last_name, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_last_name))
                .ForMember(p => p.Bills_contact_first_name, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_first_name))
                .ForMember(p => p.Bills_contact_address, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_address))
                .ForMember(p => p.Bills_contact_phones, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_phones))
                .ForMember(p => p.Bills_contact_email, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_email))
                .ForMember(p => p.Created_ts, x => x.MapFrom(a => a.cEN_Participants.Created_ts))
                .ForMember(p => p.Updated_ts, x => x.MapFrom(a => a.cEN_Participants.Updated_ts));

            CreateMap<REACT_TRGNS_UserProyects, ParticipantesDTO>()
                .ForMember(p => p.Id, x => x.MapFrom(a => a.cEN_Participants.ID))
                .ForMember(p => p.Name, x => x.MapFrom(a => a.cEN_Participants.Name))
                .ForMember(p => p.Rut, x => x.MapFrom(a => a.cEN_Participants.Rut))
                .ForMember(p => p.RutCompleto, x => x.MapFrom(a => String.Concat(a.cEN_Participants.Rut, '-', a.cEN_Participants.Verification_Code)))
                .ForMember(p => p.Verification_Code, x => x.MapFrom(a => a.cEN_Participants.Verification_Code))
                .ForMember(p => p.Business_Name, x => x.MapFrom(a => a.cEN_Participants.Business_Name))
                .ForMember(p => p.Commercial_Business, x => x.MapFrom(a => a.cEN_Participants.Commercial_Business))
                .ForMember(p => p.Dte_Reception_Email, x => x.MapFrom(a => a.cEN_Participants.Dte_Reception_Email))
                .ForMember(p => p.Bank_Account, x => x.MapFrom(a => a.cEN_Participants.Bank_Account))
                .ForMember(p => p.bank, x => x.MapFrom(a => a.cEN_Participants.bank))
                .ForMember(p => p.BanksName, x => x.MapFrom(a => a.cEN_Participants.CEN_banks.Name))
                .ForMember(p => p.Commercial_address, x => x.MapFrom(a => a.cEN_Participants.Commercial_address))
                .ForMember(p => p.Postal_address, x => x.MapFrom(a => a.cEN_Participants.Postal_address))
                .ForMember(p => p.Manager, x => x.MapFrom(a => a.cEN_Participants.Manager))
                .ForMember(p => p.Pay_Contact_First_Name, x => x.MapFrom(a => a.cEN_Participants.Pay_Contact_First_Name))
                .ForMember(p => p.Pay_contact_last_name, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_last_name))
                .ForMember(p => p.Pay_contact_address, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_address))
                .ForMember(p => p.Pay_contact_phones, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_phones))
                .ForMember(p => p.Pay_contact_email, x => x.MapFrom(a => a.cEN_Participants.Pay_contact_email))
                .ForMember(p => p.Bills_contact_last_name, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_last_name))
                .ForMember(p => p.Bills_contact_first_name, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_first_name))
                .ForMember(p => p.Bills_contact_address, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_address))
                .ForMember(p => p.Bills_contact_phones, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_phones))
                .ForMember(p => p.Bills_contact_email, x => x.MapFrom(a => a.cEN_Participants.Bills_contact_email))
                .ForMember(p => p.Created_ts, x => x.MapFrom(a => a.cEN_Participants.Created_ts))
                .ForMember(p => p.Updated_ts, x => x.MapFrom(a => a.cEN_Participants.Updated_ts));

            CreateMap<REACT_CEN_Participants, RutDto>()
                .ForMember(p => p.label, x => x.MapFrom(a => String.Concat(a.Rut, '-', a.Verification_Code)));
            CreateMap<REACT_TRGNS_Datos_Facturacion, InstruccionesDTO>()
                .ForMember(p => p.ID, x => x.MapFrom(a => a.ID))
                .ForMember(p => p.id_instruccions, x => x.MapFrom(a => a.id_instructions))
                .ForMember(p => p.Estado_emision, x => x.MapFrom(a => a.Estado_emision))
                .ForMember(p => p.Tipo_instruccion, x => x.MapFrom(a => a.tipo_instructions))
                .ForMember(p => p.CEN_billing_status_type_name, x => x.MapFrom(a => a.CEN_billing_status_type.Name))
                .ForMember(p => p.Estado_recepcion, x => x.MapFrom(a => a.Estado_recepcion))
                .ForMember(p => p.TRGNS_dte_reception_status_name, x => x.MapFrom(a => a.TRGNS_dte_reception_status.Name))
                .ForMember(p => p.Estado_pago, x => x.MapFrom(a => a.Estado_pago))
                .ForMember(p => p.CEN_payment_status_type_name, x => x.MapFrom(a => a.CEN_payment_status_type.Name))
                .ForMember(p => p.Estado_aceptacion, x => x.MapFrom(a => a.Estado_aceptacion))
                .ForMember(p => p.CEN_dte_acceptance_status_name, x => x.MapFrom(a => a.CEN_dte_acceptance_status.Name))
                .ForMember(p => p.Fecha_emision, x => x.MapFrom(a => a.Fecha_emision))
                .ForMember(p => p.Fecha_recepcion, x => x.MapFrom(a => a.Fecha_recepcion))
                .ForMember(p => p.Fecha_pago, x => x.MapFrom(a => a.Fecha_pago))
                .ForMember(p => p.Fecha_aceptacion, x => x.MapFrom(a => a.Fecha_aceptacion))
                .ForMember(p => p.NombreAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Business_Name))
                .ForMember(p => p.Acreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.ID))
                .ForMember(p => p.RutAcreedor, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_creditor.Rut,'-',a.CEN_instruction.Participants_creditor.Verification_Code)))
                .ForMember(p => p.NombreDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Business_Name))
                .ForMember(p => p.Deudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.ID))
                .ForMember(p => p.RutDeudor, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_debtor.Rut, '-', a.CEN_instruction.Participants_debtor.Verification_Code)))
                .ForMember(p => p.Concepto, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_concept))
                .ForMember(p => p.Glosa, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_natural_key))
                .ForMember(p => p.MontoNeto, x => x.MapFrom(a => a.CEN_instruction.Amount))
                .ForMember(p => p.MontoBruto, x => x.MapFrom(a => a.CEN_instruction.Amount_Gross))
                .ForMember(p => p.period, x => x.MapFrom(a => a.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows.period))
                .ForMember(p => p.Carta, x => x.MapFrom(a => a.CEN_instruction.cEN_Payment_Matrices.Letter_code))
                .ForMember(p => p.Fecha_carta, x => x.MapFrom(a => a.CEN_instruction.cEN_Payment_Matrices.Publish_date))
                .ForMember(p => p.CodigoRef, x => x.MapFrom(a => a.CEN_instruction.cEN_Payment_Matrices.Reference_code));

            CreateMap<REACT_TRGNS_Datos_Facturacion, NominasBciDto>()
              .ForMember(p => p.ID, x => x.MapFrom(a => a.ID))
              .ForMember(p => p.id_instruccions, x => x.MapFrom(a => a.id_instructions))
              .ForMember(p => p.Glosa, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_natural_key))
              .ForMember(p => p.rutDeudor, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_debtor.Rut, '-', a.CEN_instruction.Participants_debtor.Verification_Code)))
              .ForMember(p => p.rutAcreedor, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_creditor.Rut, '-', a.CEN_instruction.Participants_creditor.Verification_Code)))
              .ForMember(p => p.nombreAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Business_Name))
              .ForMember(p => p.nombreDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Business_Name))
              .ForMember(p => p.cuentaBancoAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Bank_Account))
              .ForMember(p => p.correoDteAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Dte_Reception_Email))
              .ForMember(p => p.valorNeto, x => x.MapFrom(a => a.CEN_instruction.Amount_Gross))
              .ForMember(p => p.folio, x => x.MapFrom(a => a.Folio))
              .ForMember(p => p.sBifAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.CEN_banks.Sbif))
              .ForMember(p => p.fechaDesconformidad, x => x.MapFrom(a => a.CEN_nonconformities.created_ts));


            CreateMap<REACT_TRGNS_Datos_Facturacion, sFiltros>()
                //.ForMember(p => p.RutAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Rut))
                //.ForMember(p => p.RutDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Rut))
                .ForMember(p => p.label, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_natural_key));
            //.ForMember(p => p.NombreAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Business_Name))
            //.ForMember(p => p.NombreDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Business_Name));
            CreateMap<REACT_TRGNS_Datos_Facturacion, sFiltrosRutCreditor>()       
                .ForMember(p => p.label, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_creditor.Rut,'-', a.CEN_instruction.Participants_creditor.Verification_Code)));

            CreateMap<REACT_TRGNS_Datos_Facturacion, sFiltrosRutDeudor>()
                .ForMember(p => p.label, x => x.MapFrom(a => String.Concat(a.CEN_instruction.Participants_debtor.Rut, '-', a.CEN_instruction.Participants_debtor.Verification_Code)));
            CreateMap<REACT_TRGNS_Datos_Facturacion, sFiltrosNameCreditor>()
                .ForMember(p => p.label, x => x.MapFrom(a =>  a.CEN_instruction.Participants_creditor.Business_Name));
            CreateMap<REACT_TRGNS_Datos_Facturacion, sFiltrosNameDebtor>()
                .ForMember(p => p.label, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Business_Name));
            //CreateMap<REACT_TRGNS_Datos_Facturacion, probando>()
            //    .ForMember(p => p.prueba, x => x.MapFrom(a => a.CEN_instruction.Amount));
            //CreateMap<REACT_CEN_billing_windows, BillingWindowsDto>()
            //    .ForMember(p => p.Natural_key, x => x.MapFrom(a => a.natural_key))
            //    .ForMember(p => p.billing_type, x => x.MapFrom(a => a.billing_type))
            //    .ForMember(p => p.periods, x => x.MapFrom(a => a.periods))
            //    .ForMember(p => p.created_ts, x => x.MapFrom(a => a.created_ts))
            //    .ForMember(p => p.updated_ts, x => x.MapFrom(a => a.created_ts))
            //    .ForMember(p => p.period, x => x.MapFrom(a => a.period));
            // RELACIONADO A DTO DE BILLING WINDOWS QUE NO SE USA
            // EN EL CONTROLER DE COMBO BOX
            CreateMap<REACT_CEN_nonconformities, NonconformitiesDto>()
                .ForMember(d => d.Id, x => x.MapFrom(a => a.ID))
                .ForMember(d => d.auxiliary_data, x => x.MapFrom(a => a.auxiliary_data))
                .ForMember(d => d.created_ts, x => x.MapFrom(a => a.created_ts))
                .ForMember(d => d.updated_ts, x => x.MapFrom(a => a.updated_ts))
                .ForMember(d => d.is_open, x => x.MapFrom(a => a.is_open))
                .ForMember(d => d.description, x => x.MapFrom(a => a.description))
                .ForMember(d => d.file, x => x.MapFrom(a => a.file))
                .ForMember(d => d.reported_by_creditor, x => x.MapFrom(a => a.reported_by_creditor))
                .ForMember(d => d.historial_file, x => x.MapFrom(a => a.historial_file))
                .ForMember(d => d.closed_date, x => x.MapFrom(a => a.closed_date))
                .ForMember(d => d.jira_key, x => x.MapFrom(a => a.jira_key))
                .ForMember(d => d.process_rad_number, x => x.MapFrom(a => a.process_rad_number))
                .ForMember(d => d.id_case, x => x.MapFrom(a => a.id_case))
                .ForMember(d => d.instruction, x => x.MapFrom(a => a.instruction))
                .ForMember(d => d.reason, x => x.MapFrom(a => a.reason));




            CreateMap<REACT_CEN_billing_windows, Concepto>()
                .ForMember(p => p.label, x => x.MapFrom(a => a.natural_key));

            CreateMap<REACT_TRGNS_Datos_Facturacion, Datos_Facturacion_DTO>();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuarios, UsuariosDto>().ReverseMap();

        }
    }
}
