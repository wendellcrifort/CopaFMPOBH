using Application.Common.Mappings;
using AutoMapper;

namespace Application.Services.Partida.Model
{
    public class SumulaModel : IMapFrom<Domain.Entities.Sumula>
    {
        public int IdPartida { get; set; }
        public string ArquivoSumula { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Sumula, SumulaModel>(MemberList.Destination)
                .ForMember(dest => dest.IdPartida, opt => opt.MapFrom(src => src.IdPartida))
                .ForMember(dest => dest.ArquivoSumula, opt => opt.MapFrom(src => src.Arquivo))                
                .ReverseMap();

        }
    }
}
