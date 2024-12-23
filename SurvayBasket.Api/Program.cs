using Busniss;
using Core.Contracts.Validators;
using Core.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Scalar.AspNetCore;
using SurvayBasket.Api.MiddleWars;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddKeyedScoped<IOS, Mac>("mac");
//builder.Services.AddKeyedScoped<IOS, Windos>("windos");
//builder.Services.AddTransient<CustomMiddleware>();

builder.Services.AddScoped<IPollService, PollServices>();
//builder.Services.AddMapster();
var mapsterConf = new TypeAdapterConfig();
mapsterConf.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IMapper>(new Mapper(mapsterConf));

//  add fluent validation
// old approach
//builder.Services.AddScoped<IValidator<CreatePoll>, CreatePollValidator>();
builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblies(new[] { Assembly.GetAssembly(typeof(Core.Contracts.Validators.CreatePollValidator)) });

var app = builder.Build();

#region middleware
//var logger = app.Services.GetRequiredService<ILogger<Program>>();
//app.Use(async (context, next) =>
//{
//    logger.LogInformation("Request received");
//    await next();
//    logger.LogInformation("Response sent");

//});
//app.UseMiddleware<CustomMiddleware>();
//app.UseCustomMiddleware();
// Configure the HTTP request pipeline. 
#endregion
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
