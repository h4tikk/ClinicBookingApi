using ClinicBooking.Application.Appointments;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AppointmentService>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }

}