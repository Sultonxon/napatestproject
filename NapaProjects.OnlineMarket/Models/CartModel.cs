
namespace NapaProjects.OnlineMarket.Models;

public class CartModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [ValidateNever]
    public string UserName { get; set; }

    public IEnumerable<OrderModel> Orders { get; set; }

    public CartModel(Cart cart, IEnumerable<Order> orders)
    {
        Id = cart.Id;
        UserId = cart.AppUserId;
        UserName = cart.AppUser.UserName;
        Orders = orders.Select(x => new OrderModel(x)).ToList();
    }
}
