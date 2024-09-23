using Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using UserManagement.Domain.Entities;

namespace UserManagement.Adm.Api.Controllers
{
    [Authorize]
    public class BaseApiController : Controller
    {
        protected User UserInfo()
        {
            var ret = HttpContext.Items["User"];
            var result = ret as User;
            result.Password = string.Empty;
            return result;
        }
        protected string Orgid()
        {
            string? ret = Request?.Headers["xOrgid"];
            return ret;
        }
    }
}