using AssignmentForMCСSA.Repositories.Abstract;
using AssignmentForMCСSA.Resources;
using System.ComponentModel.DataAnnotations;

namespace AssignmentForMCСSA.Data.Models.Dto
{
    public class ChangePasswordDto
    {
        public string Id { get; set; }

        [Required]
        public string? CurrentPassword { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "ErrorMessagePassword")]
        [Required(ErrorMessage = "PasswordIsTooShort")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "PasswordConfirmError")]
        [Compare("NewPassword")]
        public string? PasswordConfirm { get; set; }
    }
}
