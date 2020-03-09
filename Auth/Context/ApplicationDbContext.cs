using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            IdentityRole adminRol = new IdentityRole()
            {
                Id = "18804585-8c97-4c86-9f8a-6e147b2a3c53",
                Name = "admin",
                NormalizedName = "admin"
            };

            builder.Entity<IdentityRole>().HasData(adminRol);

            base.OnModelCreating(builder);
        }
    }
}
