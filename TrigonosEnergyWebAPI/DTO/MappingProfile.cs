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

            CreateMap<TRGNS_Datos_Facturacion, InstruccionesDTO>()
                .ForMember(p => p.id_instruccions, x => x.MapFrom(a => a.id_instructions))
                .ForMember(p => p.Estado_emision, x => x.MapFrom(a => a.Estado_emision))
                .ForMember(p => p.CEN_billing_status_type_name, x => x.MapFrom(a => a.CEN_billing_status_type.Name))
                .ForMember(p => p.Estado_recepcion, x => x.MapFrom(a => a.Estado_recepcion))
                .ForMember(p => p.TRGNS_dte_reception_status_name, x => x.MapFrom(a => a.TRGNS_dte_reception_status.Name))
                .ForMember(p => p.TRGNS_dte_reception_status_name, x => x.MapFrom(a => a.TRGNS_dte_reception_status.Name));
        }
    }
}
