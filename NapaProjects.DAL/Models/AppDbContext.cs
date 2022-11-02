
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace NapaProjects.DAL.Models;

public class AppDbContext: IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductHistory> ProductHistory { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        /*builder.Entity<ProductHistory>()
            .HasOne(p => p.CurrentProduct)
            .WithMany(p => p.ProductHistories)
            .HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.SetNull);*/
        
    }

    public static async Task InitializeOnMigration(IServiceProvider services)
    {
        UserManager<AppUser> userManager = services.GetService<UserManager<AppUser>>();
        RoleManager<IdentityRole<int>> roleManager = services
                            .GetService<RoleManager<IdentityRole<int>>>();

        await roleManager.CreateAsync(AppRoles.UserRole);
        await roleManager.CreateAsync(AppRoles.AdminRole);

        var result = await userManager.CreateAsync(new AppUser { UserName = "Admin", FirstName = "Admin", LastName = "Admin" }
        , "12345678");

        var user = await userManager.FindByNameAsync("Admin");
        await userManager.AddToRoleAsync(user, AppRoles.AdminRole.Name);
        await userManager.AddToRoleAsync(user, AppRoles.UserRole.Name);
        /*

        var adminRole = await roleManager.FindByNameAsync(AppRoles.AdminRole.Name);
        var userRole = await roleManager.FindByNameAsync(AppRoles.UserRole.Name);

        if(adminRole is null)
        {
            await roleManager.CreateAsync(AppRoles.AdminRole);

        }
        if(userRole is null)
        {
            await roleManager.CreateAsync(AppRoles.UserRole);
        }
        

        var admins = await userManager.GetUsersInRoleAsync(AppRoles.AdminRole.Name);
        Console.WriteLine(admins.Count);
        if(admins is null || admins.Count == 0)
        {
            var admin = new AppUser { FirstName = "Admin", LastName = "Admin", UserName = "Admin" };
            var result = await userManager.CreateAsync(admin, "12345678");
            Console.WriteLine(admin.Id);
            if (result.Succeeded)
            {
                await userManager
                    .AddToRoleAsync(await userManager.FindByNameAsync(admin.UserName), AppRoles.AdminRole.Name);
                await userManager
                    .AddToRoleAsync(await userManager.FindByNameAsync(admin.UserName), AppRoles.UserRole.Name);
            }
        }*/
    }
}
