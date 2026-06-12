using Application;
using Application.Movie;
using Domain.Movie;
using Infrastructure.Movie;
using Marten;
using Marten.Events.Projections;
using MovieApi.Application.Api;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
DotNetEnv.Env.Load();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IMarkerAssembly>());
builder.Services.AddMarten(options =>
{
    options.Connection(Environment.GetEnvironmentVariable("MARTEN_CONNECTION_STRING"));
    options.Projections.Add<MovieSingleStreamProjector>(ProjectionLifecycle.Inline);})
.UseLightweightSessions();

builder.Services.AddRefitClient<ITmdbApi>()
    .ConfigureHttpClient(c => 
    {
        // 1. Указываем базовый адрес
        c.BaseAddress = new Uri("https://api.themoviedb.org/3");
        
        // 2. Добавляем токен авторизации (TMDB v4 Bearer Token)
        c.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",Environment.GetEnvironmentVariable("TMDB_TOKEN") );
    });
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.MapControllers();



app.Run();


