using Application.Services.Jogador;
using Application.Services.Partida;
using Application.Services.Time;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddCustomServices();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ITimeService, TimeService>();
            services.AddScoped<IJogadorService, JogadorService>();
            services.AddScoped<IPartidaService, PartidaService>();            

            return services;
        }
    }
}
