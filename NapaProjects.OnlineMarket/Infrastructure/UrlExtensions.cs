

namespace NapaProjects.OnlineMarket.Infrastructure;

public static class UrlExtensions
{
    public static string PathAndQuery(this HttpContext context) => context.Request.QueryString.HasValue
                ? context.Request.Path + context.Request.QueryString.ToString() : context.Request.Path;

    public static bool isAdmin(this ClaimsPrincipal User) =>
        User.Identity.IsAuthenticated && User.IsInRole(AppRoles.AdminRole.Name);

    public static bool isUser(this ClaimsPrincipal User) =>
        User.Identity.IsAuthenticated && User.IsInRole(AppRoles.UserRole.Name);

    public static string ToContent(this IList<string> source)
    {
        if (source.Count == 0) return "";
        string s = "";
        for (int i = 0; i < source.Count() - 1; i++) s += source[i] + ", ";
        s += source[source.Count - 1];
        return s;
    }
}
