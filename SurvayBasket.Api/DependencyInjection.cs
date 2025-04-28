using System.Text;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SurvayBasket.Api.Options;
using SurveyBasket.Api.Services.Auth;

namespace SurvayBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection IOCServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        #region mapster
        services.AddMapsterConfig();
        #endregion

        #region Fluent Validation
        services.AddFluentValidationConfig();
        #endregion

        services.AddScoped<IPollService, PollServices>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionServices, QuestionServices>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddAuthConfig(configuration);

        // options pattern
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();


        return services;
    }

    static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mapsterConf = new TypeAdapterConfig();
        mapsterConf.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mapsterConf));
        return services;
    }

    static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.Load("Core"));
        return services;
    }

    public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

        services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddApiEndpoints()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            var jwtSettings = configuration.GetSection("Jwt");

            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions?.Key!)),
                ValidIssuer = jwtOptions?.Issuer,
                ValidAudience = jwtOptions?.Audience
            };
        });



        return services;
    }
}
