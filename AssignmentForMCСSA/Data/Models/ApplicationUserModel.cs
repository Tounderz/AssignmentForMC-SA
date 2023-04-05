using Microsoft.AspNetCore.Identity;

namespace AssignmentForMCСSA.Data.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Img { get; set; }
    }
}
