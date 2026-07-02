using ClinicBooking.Application.Appointments;
using ClinicBooking.Application.Doctors;
using ClinicBooking.Application.Patients;
using ClinicBooking.Application.Specialties;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AppointmentService>();
        services.AddScoped<DoctorService>();
        services.AddScoped<PatientService>();
        services.AddScoped<SpecialtyService>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }

}