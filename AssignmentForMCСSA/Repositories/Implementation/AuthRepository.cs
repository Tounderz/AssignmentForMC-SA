using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AssignmentForMCСSA.Repositories.Implementation
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;
        private readonly ILanguage _language;

        public AuthRepository(
            UserManager<ApplicationUserModel> userManager,
            SignInManager<ApplicationUserModel> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILanguage language)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _language = language;
        }
        public async Task<StatusModel> LoginAsync(LoginDto dto)
        {
            var status = new StatusModel();
            var user = await _userManager.FindByNameAsync(dto.Login);
            if (user == null)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("InvalidLogin").Value;
                return status;
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("InvalidPassword").Value;
                return status;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, true, true);
            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.StatusMessage = _language.GetKey("LoginSuccessfully").Value;
            }
            else if (signInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("UserIsLockedOut").Value;
            }
            else
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("ErrorOnLoggingIn").Value;
            }

            return status;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
