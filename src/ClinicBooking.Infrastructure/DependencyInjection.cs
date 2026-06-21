using ClinicBooking.Application.Absractions;
using ClinicBooking.Application.Appointments;
using ClinicBooking.Infrastructure.Persistence;
using ClinicBooking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClinicDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Clinic"));
        });

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}