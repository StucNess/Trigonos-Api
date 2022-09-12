using AutoMapper;
using Core.Entities;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergy.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CEN_Participants, ParticipantesDTO>()
                .ForMember(p => p.BanksName, x => x.MapFrom(a => a.CEN_banks.Name));
            CreateMap<TRGNS_PROYECTOS, ParticipantesDTO>()
                .ForMember(p => p.Id, x => x.MapFrom(a => a.cEN_Participants.ID))
                .ForMember(p => p.Name, x => x.MapFrom(a => a.cEN_Participants.Name))
                .ForMember(p => p.Rut, x => x.MapFrom(a => a.cEN_Participants.Rut))
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

            CreateMap<TRGNS_Datos_Facturacion, InstruccionesDTO>()
                .ForMember(p => p.ID, x => x.MapFrom(a => a.ID))
                .ForMember(p => p.id_instruccions, x => x.MapFrom(a => a.id_instructions))
                .ForMember(p => p.Estado_emision, x => x.MapFrom(a => a.Estado_emision))
                .ForMember(p => p.CEN_billing_status_type_name, x => x.MapFrom(a => a.CEN_billing_status_type.Name))
                .ForMember(p => p.Estado_recepcion, x => x.MapFrom(a => a.Estado_recepcion))
                .ForMember(p => p.TRGNS_dte_reception_status_name, x => x.MapFrom(a => a.TRGNS_dte_reception_status.Name))
                .ForMember(p => p.Estado_pago, x => x.MapFrom(a => a.Estado_pago))
                .ForMember(p => p.CEN_payment_status_type_name, x => x.MapFrom(a => a.CEN_payment_status_type.Name))
                .ForMember(p => p.CEN_dte_acceptance_status_name, x => x.MapFrom(a => a.CEN_dte_acceptance_status.Name))
                .ForMember(p => p.Fecha_emision, x => x.MapFrom(a => a.Fecha_emision))
                .ForMember(p => p.Fecha_recepcion, x => x.MapFrom(a => a.Fecha_recepcion))
                .ForMember(p => p.Fecha_pago, x => x.MapFrom(a => a.Fecha_pago))
                .ForMember(p => p.Fecha_aceptacion, x => x.MapFrom(a => a.Fecha_aceptacion))
                .ForMember(p => p.NombreAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Business_Name))
                .ForMember(p => p.Acreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.ID))
                .ForMember(p => p.RutAcreedor, x => x.MapFrom(a => a.CEN_instruction.Participants_creditor.Rut))
                .ForMember(p => p.NombreDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Business_Name))
                .ForMember(p => p.Deudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.ID))
                .ForMember(p => p.RutDeudor, x => x.MapFrom(a => a.CEN_instruction.Participants_debtor.Rut))
                .ForMember(p => p.Concepto, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_concept))
                .ForMember(p => p.Glosa, x => x.MapFrom(a => a.CEN_instruction.Payment_matrix_natural_key))
                .ForMember(p => p.MontoNeto, x => x.MapFrom(a => a.CEN_instruction.Amount))
                .ForMember(p => p.MontoBruto, x => x.MapFrom(a => a.CEN_instruction.Amount_Gross));

            CreateMap<CEN_billing_windows, Concepto>()
                .ForMember(p => p.nombre, x => x.MapFrom(a => a.Natural_key));

            CreateMap<TRGNS_Datos_Facturacion, Datos_Facturacion_DTO>();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
                
        }
    }
}
