using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Partida.Model
{
    public class EventosPartidaViewModel : IMapFrom<Domain.Entities.EventosPartida>
    {
        public int Id { get; set; }
        public int IdPartida { get; set; }
        public int IdJogador { get; set; }
        public int IdTime { get; set; }
        public int IdGoleiro { get; set; }
        public string? DescricaoEvento { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.EventosPartida, EventosPartidaViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdPartida, opt => opt.MapFrom(src => src.IdPartida))
                .ForMember(dest => dest.IdJogador, opt => opt.MapFrom(src => src.IdJogador))
                .ForMember(dest => dest.IdTime, opt => opt.MapFrom(src => src.IdTime))
                .ForMember(dest => dest.IdGoleiro, opt => opt.MapFrom(src => src.IdGoleiro))
                .ForMember(dest => dest.DescricaoEvento, opt => opt.MapFrom(src => src.DescricaoEvento))
                .ReverseMap();

        }
    }
}
