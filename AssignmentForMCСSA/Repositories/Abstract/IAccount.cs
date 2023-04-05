using AssignmentForMCСSA.Data.Models.Dto;
using AssignmentForMCСSA.Data.Models;

namespace AssignmentForMCСSA.Repositories.Abstract
{
    public interface IAccount
    {
        Task<List<ApplicationUserModel>> GetAccounts();
        Task<RegistrationDto> GetAccountById(string id);
        Task<StatusModel> RegisterAsync(RegistrationDto dto);
        Task<StatusModel> EditAsync(RegistrationDto dto);
        Task<StatusModel> DeleteAsync(string id);
        Task<StatusModel> ChangePassword(ChangePasswordDto dto);
    }
}
