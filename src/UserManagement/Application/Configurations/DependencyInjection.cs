using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using MediatR;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace UserManagement.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddCors();

            services.ConfigureCulture();
            services.ConfigureJson();
            services.ConfigureCorsPolicy();
            services.ConfigurePath(configuration);
            services.ConfigureIpWhitelist(configuration);
            services.ConfigureOrganization(configuration);
            return services;
        }
        public static void AddCors(this IServiceCollection services)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                      {
                          policy.WithOrigins("http://localhost:3001",
                                              "http://localhost:3002",
                                              "http://103.143.170.118:82"
                                              );
                      });
            });
        }
        public static void ConfigureCulture(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultureInfo = new CultureInfo("id-ID");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
                cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                var supportedCultures = new List<CultureInfo> { cultureInfo };

            });
        }
        public static void ConfigureJson(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddControllersWithViews().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
                o.JsonSerializerOptions.WriteIndented = true;
            });
        }
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        public static void ConfigurePath(this IServiceCollection services, ConfigurationManager configuration)
        {
            var directorySetting = new DirectorySetting();
            configuration.Bind(DirectorySetting.SectioName, directorySetting);
            services.AddSingleton(directorySetting);

            DirectorySetting.PathFileUser = $"{DirectorySetting.BasePath}/{DirectorySetting.FileUser}";
            if (!Directory.Exists(DirectorySetting.PathFileUser))
                Directory.CreateDirectory(DirectorySetting.PathFileUser);
            DirectorySetting.UrlFileUser = $"{DirectorySetting.BaseUrl}/{DirectorySetting.PathUrl}/{DirectorySetting.FileUser}";

            DirectorySetting.PathFileData = $"{DirectorySetting.BasePath}/{DirectorySetting.FileData}";
            if (!Directory.Exists(DirectorySetting.PathFileData))
                Directory.CreateDirectory(DirectorySetting.PathFileData);
            DirectorySetting.UrlFileData = $"{DirectorySetting.BaseUrl}/{DirectorySetting.PathUrl}/{DirectorySetting.FileData}";

            string tempFolder = $"{DirectorySetting.PathFileData}/temp";
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            DirectorySetting.PathFileApp = $"{DirectorySetting.BasePath}/{DirectorySetting.FileApp}";
            if (!Directory.Exists(DirectorySetting.PathFileApp))
                Directory.CreateDirectory(DirectorySetting.PathFileApp);
            DirectorySetting.UrlFileApp = $"{DirectorySetting.BaseUrl}/{DirectorySetting.PathUrl}/{DirectorySetting.FileApp}";

        }
        public static void ConfigureIpWhitelist(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<IpWhitelistOptions>(configuration.GetSection("IpWhitelist"));
        }
        public static void ConfigureOrganization(this IServiceCollection services, ConfigurationManager configuration)
        {
            var organizationSetting = new OrganizationSetting();
            configuration.Bind(OrganizationSetting.SectioName, organizationSetting);
            services.AddSingleton(organizationSetting);
        }
    }
}