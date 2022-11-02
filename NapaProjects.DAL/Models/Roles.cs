
namespace NapaProjects.DAL.Models;

public class AppRoles
{
    private static string Admin => "Admin";
    private static string User => "User";

    public static IdentityRole<int> AdminRole => new IdentityRole<int>(Admin);
    public static IdentityRole<int> UserRole => new IdentityRole<int>(User);

    public static bool IsFirstRunning { get; set; } = true;
}
