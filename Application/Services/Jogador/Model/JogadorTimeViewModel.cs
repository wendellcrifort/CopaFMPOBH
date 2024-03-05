using Application.Common.Mappings;
using Application.Services.Time.Model;
using AutoMapper;

namespace Application.Services.Jogador.Model
{
    public class JogadorTimeViewModel : IMapFrom<Domain.Entities.Time>
    {
        public TimeViewModel? Time { get; set; }
        public List<JogadorViewModel>? Jogadores { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Time, JogadorTimeViewModel>(MemberList.Destination)
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Jogadores, opt => opt.MapFrom(src => src.Jogadores))
                .ReverseMap();
        }
    }
}
