using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Challenger.Application.Configss;
using Challenger.Domain.Interfaces;
using Challenger.Infrastructure.Context;
using Challenger.Infrastructure.Percistence.Repositoryes;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Challenger.Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddDBContext(this IServiceCollection services, ConnectionSettings configuration)
        {
            return services.AddDbContext<CGContext>(options =>
            {
                var connectionString = configuration.MotoGridDb;
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), my =>
                {
                    my.EnableRetryOnFailure();
                });
            });
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<IPatioRepository, PatioRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddInfra(this IServiceCollection services,Settings settings)
        {
            services.AddDBContext(settings.ConnectionStrings);
            services.AddRepositories();
            return services;
        }
    }
}
