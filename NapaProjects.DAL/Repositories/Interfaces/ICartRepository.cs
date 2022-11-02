
namespace NapaProjects.DAL.Repositories;

public interface ICartRepository
{
    IEnumerable<Cart> GetAll();

    Cart Get(int cartId);
    Cart GetByUser(int userId);

    IEnumerable<Order> Orders(int cartId);

    bool AddToCart(Order order, int userId);

    bool DeleteFromCart(int orderId);

    float GetAmount(int cartId);
}
