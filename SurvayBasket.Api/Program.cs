using Scalar.AspNetCore;
using SurvayBasket.Api.Interfaces;
using SurvayBasket.Api.MiddleWars;
using SurvayBasket.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddKeyedScoped<IOS, Mac>("mac");
//builder.Services.AddKeyedScoped<IOS, Windos>("windos");
//builder.Services.AddTransient<CustomMiddleware>();

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
