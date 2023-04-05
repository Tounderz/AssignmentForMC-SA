using AssignmentForMCСSA.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssignmentForMCСSA.Data.Db
{
    public class AppDbContext : IdentityDbContext<ApplicationUserModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
