
namespace NapaProjects.DAL.Models;

public class Cart
{
    public int Id { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }

    public IEnumerable<Order> Orders { get; set; }

}
