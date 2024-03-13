using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Interfaces;

namespace Notes.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfiguration configuraion)
        {
            var connectionString = configuraion["DbConnection"];

            services.AddDbContext<NotesDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            services.AddScoped<INotesDbContext>(provider => provider.GetService<NotesDbContext>());
            return services;
        }
    }
}
