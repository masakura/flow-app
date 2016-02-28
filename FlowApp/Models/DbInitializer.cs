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

            context.Users.Add(User("{aa45d108-5f7a-4be3-8e1e-b1e7b5453b62}", "manager1@example.com"));
            context.Users.Add(User("{618f5ee0-c487-442c-9912-f8bd017294ea}", "manager2@example.com"));
            context.Users.Add(User("{77d5a8b4-3ceb-41b9-9134-7f6a596f174d}", "manager3@example.com"));
            context.Users.Add(User("{ae53b5b5-d217-44d8-89e9-d761e87028b8}", "manager4@example.com"));
            context.Users.Add(User("{6cbfb82a-3773-4ff5-b993-f2443679ac91}", "manager5@example.com"));

            context.SaveChanges();
        }

        private static ApplicationUser User(string id, string userName)
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = userName,
                Email = userName,
                PasswordHash = new PasswordHasher().HashPassword("password"),
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }
    }
}