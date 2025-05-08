using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed roles (use, admin, superadmin)
            var adminRoleId = "b57876f5-5cae-481f-a945-d93c8e914125";
            var SuperAdminRoleId = "b728f2e8-3c36-4984-983e-d5478afa25d0";
            var userRoleId = "115674ce-d2e2-488f-bfd6-0dee7a398e15";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = SuperAdminRoleId,
                    ConcurrencyStamp = SuperAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);


            // Seed SuperAdminUser
            var superAdminId = "8fdc734e-7b7e-4cc9-85c4-3db6731ae6d8";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@bloggie.com",
                Email = "superadmin@bloggie.com",
                NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
                NormalizedUserName = "superadmin@bloggie.com",
                ConcurrencyStamp = superAdminId,
                SecurityStamp = superAdminId,
                Id = superAdminId
            };

            superAdminUser.PasswordHash = "$2y$10$BV5GoMkzdW01MR2833HctuYPkNVzgq9cirRcsevuVCX5X1/CQHwQe";
                //new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "SuperAdmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);


            // Add all the roles to superadminuser
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = SuperAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
