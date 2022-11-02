

namespace NapaProjects.DAL.Repositories;

public class CartRepository: ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public bool AddToCart(Order order, int userId)
    {
        Cart cart = _context.Carts.FirstOrDefault(c => c.AppUserId == userId);
        if(cart == null)
        {
            cart = _context.Carts.Add(new Cart { AppUserId = userId }).Entity;
        }
        order.CartId = cart.Id;
        _context.Orders.Add(order);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteFromCart(int orderId)
    {
        _context.Orders.Remove(new Order { Id = orderId });
        _context.SaveChanges();
        return true;
    }

    public Cart Get(int cartId) => _context.Carts.Include(c => c.Orders).FirstOrDefault(x => x.Id == cartId);

    public IEnumerable<Cart> GetAll() => _context.Carts.ToList();

    public float GetAmount(int cartId) => _context.Carts.Any(x => x.Id == cartId) ?
        _context.Carts.Sum(x => x.Orders.Sum(y => y.Product.Price * y.Quantity)) : 0;

    public Cart GetByUser(int userId) 
    {
        if (_context.Carts.Any(x => x.AppUserId == userId))
            return _context.Carts
                .Include(x => x.Orders)
                .Include(x => x.AppUser)
                .First(x => x.AppUserId == userId);
        else 
        {
            var newCart = new Cart { AppUserId = userId, Id = 0 };
            _context.Carts.Add(newCart);
            _context.SaveChanges();
            return _context.Carts
                .Include(c => c.Orders)
                .Include(x => x.AppUser)
                .First(x => x.Id == newCart.Id);
        }
     }

    public IEnumerable<Order> Orders(int cartId) => _context.Orders.Where(x => x.CartId == cartId)
        .Include(x => x.Product).Include(x => x.Cart).ToList();
    
}
