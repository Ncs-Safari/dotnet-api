using System.Globalization;
using Common.Constants;
using Common.Exceptions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using UserManagement.Application.Configuration;
using UserManagement.Infrastructure.Services;

namespace UserManagement.Adm.Api.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddLocalization();

            services.AddCors();
            services.AddSwagger();

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

            return services;
        }
        private static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://s-one.swg.co.id", "https://s-one.swg.co.id/admin")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
        }
        private static void AddSwagger(this IServiceCollection services)
        {
            //services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Application Backend API",
                    Description = "List of APIs used in the application",
                    TermsOfService = new Uri("https://swg.co.id/#aboutus"),
                    Contact = new OpenApiContact
                    {
                        Name = "PT. Sawerigading Multi Kreasi",
                        Email = "sawerigading.it@gmail.com",
                        Url = new Uri("https://swg.co.id"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under OpenApiLicense",
                        Url = new Uri("https://swg.co.id/#jasa"),
                    }
                });
                c.EnableAnnotations();
                //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                var filePath = Path.Combine(AppContext.BaseDirectory, "Adm.Api.xml");
                c.IncludeXmlComments(filePath);
            });

        }
        public static WebApplication AppConfig(this WebApplication app)
        {
            app.UsePathBase("/api-admin");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v2/swagger.json", "Backend API v2");
                });
            }
            //must same with in CultureInfoConstant
            app.UseRequestLocalization(["id-ID", "en-US"]);
            app.UseMiddleware<UserPreferencesMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ValidationMiddleware>();
            app.UseCors();
            // app.UseHttpsRedirection();

            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.UseHttpMethodOverride();
            app.UseRouting();
            app.AppConfigStaticFile();
            return app;
        }
        private static void AppConfigStaticFile(this WebApplication app)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(DirectorySetting.PathFileUser),
                RequestPath = new PathString($"/{DirectorySetting.PathUrl}/{DirectorySetting.FileUser}")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(DirectorySetting.PathFileData),
                RequestPath = new PathString($"/{DirectorySetting.PathUrl}/{DirectorySetting.FileData}")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(DirectorySetting.PathFileApp),
                RequestPath = new PathString($"/{DirectorySetting.PathUrl}/{DirectorySetting.FileApp}")
            });
        }
    }
}