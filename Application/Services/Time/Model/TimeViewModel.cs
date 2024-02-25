using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Time.Model
{
    public class TimeViewModel : IMapFrom<Domain.Entities.Time>
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Grupo { get; set; }
        public int Pontos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsFeitos { get; set; }
        public int GolsSofridos { get; set; }
        public int SaldoGols { get; set; }
        public int CartoesAmarelos { get; set; }
        public int CartoesVermelhos { get; set; }
        public string? Escudo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Time, TimeViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Grupo, opt => opt.MapFrom(src => src.Grupo))
                .ForMember(dest => dest.Pontos, opt => opt.MapFrom(src => src.Pontos))
                .ForMember(dest => dest.Vitorias, opt => opt.MapFrom(src => src.Vitorias))
                .ForMember(dest => dest.Empates, opt => opt.MapFrom(src => src.Empates))
                .ForMember(dest => dest.Derrotas, opt => opt.MapFrom(src => src.Derrotas))
                .ForMember(dest => dest.GolsFeitos, opt => opt.MapFrom(src => src.GolsFeitos))
                .ForMember(dest => dest.GolsSofridos, opt => opt.MapFrom(src => src.GolsSofridos))
                .ForMember(dest => dest.SaldoGols, opt => opt.MapFrom(src => src.SaldoGols))
                .ForMember(dest => dest.CartoesAmarelos, opt => opt.MapFrom(src => src.CartoesAmarelos))
                .ForMember(dest => dest.CartoesVermelhos, opt => opt.MapFrom(src => src.CartoesVermelhos))
                .ForMember(dest => dest.Escudo, opt => opt.MapFrom(src => src.Escudo))
                .ReverseMap();

        }
    }
}
