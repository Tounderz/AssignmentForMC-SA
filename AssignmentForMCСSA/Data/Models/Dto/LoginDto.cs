using AssignmentForMCСSA.Repositories.Abstract;
using System.ComponentModel.DataAnnotations;

namespace AssignmentForMCСSA.Data.Models.Dto
{
    public class LoginDto
    {
        [StringLength(50, ErrorMessage = "LoginIsTooShort", MinimumLength = 4)]
        [Required(ErrorMessage = "LoginIsTooShort")]
        public string Login { get; set; }

        [StringLength(50, ErrorMessage = "PasswordIsTooShort", MinimumLength = 5)]
        [Required(ErrorMessage = "PasswordIsTooShort")]
        public string Password { get; set; }
    }
}
