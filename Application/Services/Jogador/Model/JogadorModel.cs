using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Jogador.Model
{
    public class JogadorModel : IMapFrom<Domain.Entities.Jogador>
    {
        public int IdTime { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Igreja { get; set; }
        public int Numero { get; set; }
        public bool EhGoleiro { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JogadorModel, Domain.Entities.Jogador>(MemberList.Destination)
                .ForMember(dest => dest.IdTime, opt => opt.MapFrom(src => src.IdTime))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Idade, opt => opt.MapFrom(src => src.Idade))
                .ForMember(dest => dest.Igreja, opt => opt.MapFrom(src => src.Igreja))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.EhGoleiro, opt => opt.MapFrom(src => src.EhGoleiro))
                .ReverseMap();

        }
    }
}
