using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Register the AppDbContext with the dependency injection container
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region Serilog
#region define level in same file

//builder.Host.UseSerilog((contect, configuration) =>
//{
//    configuration
//        .MinimumLevel.Information()
//        .WriteTo.Console();

//});
#endregion
#region read from app.settings 
builder.Host.UseSerilog(
    (context, configuration)
        => configuration.ReadFrom.Configuration(context.Configuration));

#endregion
#endregion

builder.Services.IOCServices(builder.Configuration);
builder.Services.AddOutputCache(opt =>
{
    opt.AddPolicy("pollCach", x =>
    {

        x.Cache().Expire(TimeSpan.FromSeconds(120));
    });
});
var app = builder.Build();
app.UseSerilogRequestLogging();
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
app.UseRouting();

app.UseCors();
app.UseAuthentication(); // Required for authentication
app.UseAuthorization();
app.UseResponseCaching();

app.MapIdentityApi<ApplicationUser>();
app.Use(async (context, next) =>
{

    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var user = context.User.Identity;
    if (user == null || !user.IsAuthenticated)
    {
        logger.LogWarning("User is NOT authenticated in the pipeline.");
    }
    else
    {
        logger.LogInformation("User authenticated: {Name}", user.Name ?? "Anonymous");
    }
    await next();
});
app.MapControllers();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseExceptionHandler();

app.Run();

