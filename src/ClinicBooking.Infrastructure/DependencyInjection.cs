using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Appointments;
using ClinicBooking.Application.Doctors;
using ClinicBooking.Application.Patients;
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
        services.AddScoped<IPatientRepository, PatientRepository>();

        services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPatientQueryService, PatientQueryService>();
        services.AddScoped<IDoctorQueryService, DoctorQueryService>();

        return services;
    }
}