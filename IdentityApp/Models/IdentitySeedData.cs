using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";

    public static async Task IdentityTestUser(IApplicationBuilder app)
    {
    using var scope = app.ApplicationServices.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        await context.Database.MigrateAsync();
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    var user = await userManager.FindByNameAsync(adminUser);

    if (user == null)
    {
        user = new AppUser
        {
            UserName = adminUser,
            FullName = "Berkay Özcan",
            Email = "berkayozcan@hotmail.com",
            PhoneNumber = "05535528438",
        };
        var result = await userManager.CreateAsync(user, adminPassword);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

    }
}