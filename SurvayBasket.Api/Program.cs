using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Register the AppDbContext with the dependency injection container
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.IOCServices(builder.Configuration);
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
app.UseRouting();

app.UseAuthentication(); // Required for authentication
app.UseAuthorization();
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

