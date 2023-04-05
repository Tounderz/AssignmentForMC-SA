using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Data.Models.Dto;

namespace AssignmentForMCСSA.Repositories.Abstract
{
    public interface IAuth
    {
        Task<StatusModel> LoginAsync(LoginDto dto);
        Task LogoutAsync();
    }
}
