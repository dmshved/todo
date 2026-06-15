using ToDo.Api.Data;
using ToDo.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.UseOpenApi();

app.UseHttpsRedirection();

// Keep the order please
app.UseAuthentication();
app.UseAuthorization();

app.ApplyCorsConfig();
app.MapControllers();

app.Run();