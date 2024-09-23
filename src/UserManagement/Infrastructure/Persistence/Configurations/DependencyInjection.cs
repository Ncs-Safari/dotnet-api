using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.ValueObjects;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var _dbSettings = new DatabaseSettings();
            configuration.Bind(DatabaseSettings.SectioName, _dbSettings);
            services.AddSingleton(_dbSettings);


            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();


            SqlMapper.AddTypeHandler(typeof(RefTablesJsonData), new JsonObjectTypeHandler());
            SqlMapper.AddTypeHandler(typeof(List<RefTablesJsonData>), new JsonObjectTypeHandler());
            return services;
        }
    }
}

