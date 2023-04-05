using AssignmentForMCСSA.Repositories.Abstract;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AssignmentForMCСSA.Controllers
{
    public class HomeController : Controller
    {
        private ILanguage _language;
        public HomeController(ILanguage language)
        {
            _language = language;
        }

        public IActionResult Error()
        {
            ViewBag.Title = _language.GetKey("ErrorMessage").Value;
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName, 
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)), 
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}