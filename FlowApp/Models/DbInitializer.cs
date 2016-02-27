using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace FlowApp.Models
{
    internal sealed class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            context.Users.Add(new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "masakura@example.com",
                Email = "masakura@example.com",
                PasswordHash = new PasswordHasher().HashPassword("password"),
                SecurityStamp = Guid.NewGuid().ToString()
            });

            context.SaveChanges();
        }
    }
}