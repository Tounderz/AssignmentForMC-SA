using AssignmentForMCСSA.Data.Db;
using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Repositories.Abstract;
using AssignmentForMCСSA.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8603
#pragma warning disable CS8601
#pragma warning disable CS8604

namespace AssignmentForMCСSA.Repositories.Implementation
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISaveImg _saveImg;
        private readonly AppDbContext _context;
        private readonly ILanguage _language;

        public AccountRepository(
            UserManager<ApplicationUserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            ISaveImg saveImg,
            AppDbContext context,
            ILanguage language)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _saveImg = saveImg;
            _context = context;
            _language = language;
        }

        public async Task<List<ApplicationUserModel>> GetAccounts()
        {
            var accounts = await _context.Users.ToListAsync();
            if (accounts == null || accounts.Count <= 0)
            {
                return null;
            }

            return accounts;
        }

        public async Task<RegistrationDto> GetAccountById(string id)
        {
            var account = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
            if (account == null)
            {
                return null;
            }

            var registerModel = new RegistrationDto()
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Login = account.UserName,
                Img = null
            };

            return registerModel;
        }

        public async Task<StatusModel> RegisterAsync(RegistrationDto dto)
        {
            var status = new StatusModel();
            var userExists = await _userManager.FindByNameAsync(dto.Login);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("UserAlreadyExist").Value;
                return status;
            }

            var user = new ApplicationUserModel()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Login,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Img = dto.Img != null ? await _saveImg.SaveImg(dto.Img) : string.Empty
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("UserCreationFailed").Value;
                return status;
            }

            if (!await _roleManager.RoleExistsAsync(Roles.registered.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(Roles.registered.ToString()));


            if (await _roleManager.RoleExistsAsync(Roles.registered.ToString()))
            {
                await _userManager.AddToRoleAsync(user, Roles.registered.ToString());
            }

            status.StatusCode = 1;
            status.StatusMessage = _language.GetKey("RegisteredSuccessfully").Value;
            return status;
        }

        public async Task<StatusModel> EditAsync(RegistrationDto dto)
        {
            var status = new StatusModel();
            var user = await _userManager.FindByNameAsync(dto.Login);
            if (user == null)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("NotAccount").Value;
                return status;
            }

            user.FirstName = !string.IsNullOrEmpty(dto.FirstName) ? dto.FirstName : user.FirstName;
            user.LastName = !string.IsNullOrEmpty(dto.LastName) ? dto.LastName : user.LastName;
            user.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : user.Email;
            user.UserName = !string.IsNullOrEmpty(dto.Login) ? dto.Login : user.UserName;
            user.Img = dto.Img != null ? await _saveImg.SaveImg(dto.Img) : user.Img;
            
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.StatusMessage = _language.GetKey("UserUpdateError").Value;
                return status;
            }

            var currentRole = (await _userManager.GetRolesAsync(user))[0];
            if (currentRole != dto.Role)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            status.StatusCode = 1;
            status.StatusMessage = _language.GetKey("UpdatedSuccessfully").Value;
            return status;
        }

        public async Task<StatusModel> ChangePassword(ChangePasswordDto dto)
        {
            var status = new StatusModel();
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                status.StatusMessage = _language.GetKey("NotAccount").Value;
                status.StatusCode = 0;
                return status;
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (result.Succeeded)
            {
                status.StatusMessage = _language.GetKey("PasswordUpdated").Value;
                status.StatusCode = 1;
            }
            else
            {
                status.StatusMessage = _language.GetKey("PasswordUpdateError").Value;
                status.StatusCode = 0;
            }

            return status;
        }

        public async Task<StatusModel> DeleteAsync(string id)
        {
            var status = new StatusModel();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                status.StatusMessage = _language.GetKey("NotAccount").Value;
                status.StatusCode = 0;
                return status;
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                status.StatusMessage = "User successfully deleted.";
                status.StatusCode = 1;
            }
            else
            {
                status.StatusMessage = "User deletion error.";
                status.StatusCode = 0;
            }

            return status;
        }
    }
}
