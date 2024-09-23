using log4net.Config;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using UserManagement.Application.Configuration;
using UserManagement.Infrastructure.Services.Configurations;
using UserManagement.Infrastructure.Persistence.Configurations;
using UserManagement.Adm.Api.Configurations;
using UserManagement.Infrastructure.Services;
using Common.Exceptions;
using Common.Constants;
using System.Globalization;
XmlConfigurator.Configure(new FileInfo("log4net.config"));

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddPresentationLayer(builder.Configuration);
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.WebHost.UseUrls(DirectorySetting.BaseUrlProxy);
var app = builder.Build();
//app.AppConfig();

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
app.Run();
