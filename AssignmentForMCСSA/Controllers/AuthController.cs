using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentForMCСSA.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        public IActionResult Login()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _auth.LoginAsync(dto);
            if (result.StatusCode == 1)
                return RedirectToAction("GetProducts", "Products");
            else
            {
                ViewBag.Title = result.StatusMessage;
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            await _auth.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
