using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Jogador.Model
{
    public class JogadorViewModel : IMapFrom<Domain.Entities.Jogador>
    {        
        public int Id { get; set; }        
        public int IdTime { get; set; }        
        public string Nome { get; set; }        
        public int Numero { get; set; }
        public int? GolsMarcados { get; set; } = 0;
        public int? GolsSofridos { get; set; } = 0;
        public bool EhGoleiro { get; set; } = false;
        public int? CartoesAmarelos { get; set; } = 0;
        public int? CartoesVemelhos { get; set; } = 0;
        public bool? Suspenso { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Jogador, JogadorViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdTime, opt => opt.MapFrom(src => src.IdTime))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))                
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.GolsMarcados, opt => opt.MapFrom(src => src.GolsMarcados))
                .ForMember(dest => dest.GolsSofridos, opt => opt.MapFrom(src => src.GolsSofridos))
                .ForMember(dest => dest.EhGoleiro, opt => opt.MapFrom(src => src.EhGoleiro))
                .ForMember(dest => dest.CartoesAmarelos, opt => opt.MapFrom(src => src.CartoesAmarelos))
                .ForMember(dest => dest.CartoesVemelhos, opt => opt.MapFrom(src => src.CartoesVemelhos))
                .ForMember(dest => dest.Suspenso, opt => opt.MapFrom(src => src.Suspenso))
                .ReverseMap();
        }
    }
}
