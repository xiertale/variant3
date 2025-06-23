using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using variant3.Models;

public static class DbInitializer
{
    public static void Initialize(Variant3Context context)
    {
        context.Database.EnsureCreated();

        // Проверяем, есть ли уже роли
        if (!context.Roles.Any())
        {
            var roles = new Role[]
            {
            new Role { Name = "Admin", AccessRights = "Full" },
            new Role { Name = "User", AccessRights = "Limited" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        // Проверяем, есть ли администратор
        if (!context.Users.Any(u => u.Role.Name == "Admin"))
        {
            var admin = new User
            {
                Login = "admin",
                Password = "admin123", // В реальном приложении нужно хешировать!
                Name = "Admin",
                Surname = "Admin",
                Phone = "+1234567890",
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                Role = context.Roles.FirstOrDefault(r => r.Name == "Admin")
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
