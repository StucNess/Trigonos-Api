using AutoMapper;
using Core.Entities;

namespace TrigonosEnergy.DTO
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CEN_Participants, ParticipantesDTO>()
                .ForMember(p => p.BanksName, x => x.MapFrom(a => a.CEN_banks));
        }
    }
}
