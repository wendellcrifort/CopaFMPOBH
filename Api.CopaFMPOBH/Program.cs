using Application.Hubs;
using Application.Services.Jogador;
using Application.Services.Partida;
using Application.Services.Time;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddScoped<IJogadorService, JogadorService>();
builder.Services.AddScoped<IPartidaService, PartidaService>();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddApplicationService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors((builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader()));

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<PlacarHub>("/placarHub"); // Mapeia o hub do SignalR
});

app.Run();
