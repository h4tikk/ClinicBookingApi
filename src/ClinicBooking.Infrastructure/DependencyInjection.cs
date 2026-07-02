using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Appointments;
using ClinicBooking.Application.Doctors;
using ClinicBooking.Application.Patients;
using ClinicBooking.Application.Specialties;
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
        var connectionString = configuration.GetConnectionString("Clinic")
            ?? throw new InvalidOperationException("Clinic connection string not found");
        services.AddDbContext<ClinicDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        
        services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
        services.AddScoped<ISpecialtyQueryService, SpecialtyQueryService>();
        services.AddScoped<IPatientQueryService, PatientQueryService>();
        services.AddScoped<IDoctorQueryService, DoctorQueryService>();
        
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();


        return services;
    }
}