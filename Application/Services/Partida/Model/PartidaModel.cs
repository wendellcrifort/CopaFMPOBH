using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Partida.Model
{
    public class PartidaModel : IMapFrom<Domain.Entities.Partida>
    {        
        public int? IdPartida { get; set; }
        public int IdMandante { get; set; }        
        public int IdVisitante { get; set; }    
        public string? Data { get; set; }
        public string? Hora { get; set; }
        public int? GolsMandante { get; set; }
        public int? GolsVisitante { get; set; }
        public int Rodada { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Partida, PartidaModel>(MemberList.Destination)                
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DataPartida))
                .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.HoraPartida))
                .ForMember(dest => dest.Rodada, opt => opt.MapFrom(src => src.Rodada))
                .ForMember(dest => dest.IdMandante, opt => opt.MapFrom(src => src.IdTimeMandante))
                .ForMember(dest => dest.IdVisitante, opt => opt.MapFrom(src => src.IdTimeVisitante))
                .ReverseMap();

        }
    }
}
