using Application.Common.Mappings;
using Application.Services.Time.Model;
using AutoMapper;

namespace Application.Services.Partida.Model
{
    public class PartidaViewModel : IMapFrom<Domain.Entities.Partida>
    {
        public int IdPartida { get; set; }
        public string? DataPartida { get; set; }
        public string? HoraPartida { get; set; }
        public TimeViewModel TimeMandante { get; set; } = new TimeViewModel();
        public TimeViewModel TimeVisitante { get; set; } = new TimeViewModel();
        public int Rodada { get; set; }
        public bool EmAndamento { get;set; }
        public bool PartidaFinalizada { get; set; }
        public int GolsTimeMandante { get; set; }
        public int GolsTimeVisitante { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Partida, PartidaViewModel>(MemberList.Destination)
                .ForMember(dest => dest.IdPartida, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DataPartida, opt => opt.MapFrom(src => src.DataPartida))
                .ForMember(dest => dest.HoraPartida, opt => opt.MapFrom(src => src.HoraPartida))
                .ForMember(dest => dest.Rodada, opt => opt.MapFrom(src => src.Rodada))
                .ForMember(dest => dest.TimeMandante, opt => opt.MapFrom(src => src.TimeMandante))
                .ForMember(dest => dest.TimeVisitante, opt => opt.MapFrom(src => src.TimeVisitante))
                .ForMember(dest => dest.EmAndamento, opt => opt.MapFrom(src => src.EmAndamento))
                .ForMember(dest => dest.PartidaFinalizada, opt => opt.MapFrom(src => src.PartidaFinalizada))
                .ForMember(dest => dest.GolsTimeMandante, opt => opt.MapFrom(src => src.GolsTimeMandante))
                .ForMember(dest => dest.GolsTimeVisitante, opt => opt.MapFrom(src => src.GolsTimeVisitante))
                .ReverseMap();

        }
    }
}
