using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Data.Models.View;
using AssignmentForMCСSA.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Resources;

namespace AssignmentForMCСSA.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccount _account;
        private ILanguage _language;

        public AccountController(IAccount account, ILanguage language)
        {
            _account = account;
            _language = language;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _account.GetAccounts();
            if (accounts == null)
            {
                ViewBag.Title = _language.GetKey("AccountsAreNull").Value;
                return View("Error", "Home");
            }

            return View(accounts);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([Bind()]RegistrationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var status = await _account.RegisterAsync(dto);
            if (status.StatusCode != 1)
            {
                ViewBag.Title = status.StatusMessage;
                return View(dto);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var account = await _account.GetAccountById(id);
            if (account == null)
            {
                ViewBag.Title = _language.GetKey("NotAccount").Value;
                return View("Error", "Home");
            }

            return View(account);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(RegistrationDto dto)
        {
            var status = await _account.EditAsync(dto);
            if (status.StatusCode != 1)
            {
                ViewBag.Title = status.StatusMessage;
                return View(dto);
            }

            return RedirectToAction(nameof(GetAccounts));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var account = await _account.GetAccountById(id);
            if (account == null)
            {
                ViewBag.Title = _language.GetKey("NotAccount").Value;
                return View("Error", "Home");
            }

            return View(account);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(RegistrationDto dto)
        {
            var status = await _account.DeleteAsync(dto.Id);
            if (status.StatusCode != 1)
            {
                ViewBag.Title = status.StatusMessage;
                return RedirectToAction(nameof(GetAccounts));
            }

            return RedirectToAction(nameof(GetAccounts));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var account = await _account.GetAccountById(id);
            if (account == null)
            {
                ViewBag.Title = _language.GetKey("NotAccount").Value;
                return View();
            }

            var changePasswordDto = new ChangePasswordDto()
            {
                Id = id
            };

            return View(changePasswordDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var status = await _account.ChangePassword(dto);
            if (status.StatusCode != 1)
            {
                ViewBag.Title = status.StatusMessage;
                return View(dto);
            }

            return RedirectToAction(nameof(GetAccounts));
        }
    }
}
