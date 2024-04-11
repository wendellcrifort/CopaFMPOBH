using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Partida.Model
{
    public class SumulaViewModel : IMapFrom<Domain.Entities.Sumula>
    {
        public int IdSumula { get; set; }
        public int IdPartida { get; set; }
        public string Partida { get; set; }
        public string Arquivo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Sumula, SumulaViewModel>(MemberList.Destination)
                .ForMember(dest => dest.IdSumula, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IdPartida, opt => opt.MapFrom(src => src.IdPartida))
                .ForMember(dest => dest.Arquivo, opt => opt.MapFrom(src => src.Arquivo))
                .ForMember(dest => dest.Partida, opt => opt.MapFrom(src => $"{src.Partida!.TimeMandante!.Nome} vs {src.Partida!.TimeVisitante!.Nome}"))                
                .ReverseMap();

        }
    }
}
