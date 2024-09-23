using Common.Constants;
using Common.Dtos;
using Common.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure.Services;
public class UserPreferencesMiddleware
{
    private readonly RequestDelegate _next;

    public UserPreferencesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserPreferences userPreferences)
    {
        userPreferences.LanguageId = "id";
        var languageId = context.Request.Headers["Language-id"].ToString();
        if (!string.IsNullOrWhiteSpace(languageId))
        {
            try
            {
                userPreferences.LanguageId = languageId;
            }
            catch (CultureNotFoundException)
            {
                userPreferences.LanguageId = "id";
            }

            var culture = new CultureInfo(CultureInfoConstant.Dict.GetValueOrDefault(userPreferences.LanguageId, "id-ID"));
            culture.NumberFormat.NumberDecimalSeparator = ".";
            culture.NumberFormat.CurrencyDecimalSeparator = ".";
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
        userPreferences.OrganizationId = context.Request.Headers["Organization-id"].ToString();


        await _next(context);
    }
}
