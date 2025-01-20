using System.Text.Json.Serialization;
using System.Text.Json;
using Data.DatabaseContext;
using Core.Interfaces.Repositories;
using Data.Repositories;
using Core.Services;
using Core.Interfaces.Services;
using Core.Interfaces.Auth;
using Api.Middleware;
using Core.Utils;
using Microsoft.OpenApi.Models;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        ServicesConfig(builder.Services);
        AppConfig.GeneralAuthConfig(builder.Services);
        AppConfig.DocsConfig(builder.Services);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .WithOrigins("https://b2b-web-lime.vercel.app", "http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "B2B Api",
                Version = "v1"
            });

            c.EnableAnnotations();
        });

        var app = builder.Build();

        app.UseCors("AllowAll");

        //app.UseMiddleware<TokenMiddleware>();
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }

    private static void ServicesConfig(IServiceCollection services)
    {
        services.Configure<AzureOptions>(AppConfig.GetSection(nameof(AzureOptions)));
        services.Configure<JwtOptions>(AppConfig.GetSection(nameof(JwtOptions)));
        services.Configure<MailerOptions>(AppConfig.GetSection(nameof(MailerOptions)));

        DIConfig(services);

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        AppConfig.DbContextConfig<B2BDbContext>(services);
    }

    private static void DIConfig(IServiceCollection services)
    {
        services.AddSingleton<IDbContextConfigurer<B2BDbContext>, B2BDbContextConfigurer>();
        services.AddScoped<PerformerToUserPipe>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISocialRepository, SocialsRepository>();
        services.AddScoped<IGenreRepository, GenresRepository>();
        services.AddScoped<IPerformerRepository, PerformerRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<INewsRespository, NewsRepository>();
        services.AddScoped<IRatingRepository, RatingsRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IGenreRepository, GenresRepository>();
        services.AddScoped<ITrackService, TrackService>();
        services.AddScoped<IGenreService, GenresService>();
        services.AddScoped<IPerformerService, PerformerService>();
        services.AddScoped<IProducerService, ProducerService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<INewsService, NewsService>();

    }
}
