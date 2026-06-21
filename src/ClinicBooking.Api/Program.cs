using ClinicBooking.Application;
using ClinicBooking.Infrastructure;
using ClinicBooking.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DoctorOnly", policy =>
        policy.RequireRole("Doctor"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});
var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();
app.MapOpenApi();
app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    using var  scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
    await SeedData.SeedDevelopmentData(db, CancellationToken.None);
}

app.Run();

partial class Program;