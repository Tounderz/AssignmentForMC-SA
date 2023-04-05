using AssignmentForMCСSA.Repositories.Implementation;
using AssignmentForMCСSA.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618

namespace AssignmentForMCСSA.Data.Models.Dto
{
    public class RegistrationDto
    {
        public string? Id { get; set; }

        [StringLength(25, ErrorMessage = "FirstNameIsTooShort", MinimumLength = 2)]
        [Required(ErrorMessage = "FirstNameIsTooShort")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "LastNameIsTooShort", MinimumLength = 2)]
        [Required(ErrorMessage = "LastNameIsTooShort")]
        public string LastName { get; set; }

        [StringLength(25, ErrorMessage = "EmailIsTooShort", MinimumLength = 5)]
        [Required(ErrorMessage = "EmailIsTooShort")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(25, ErrorMessage = "LoginIsTooShort", MinimumLength = 4)]
        [Required(ErrorMessage = "LoginIsTooShort")]
        public string Login { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "ErrorMessagePassword")]
        [Required(ErrorMessage = "ErrorMessagePassword")]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordConfirmError")]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        public IFormFile? Img { get; set; }

        public string? Role { get; set; }
    }
}
