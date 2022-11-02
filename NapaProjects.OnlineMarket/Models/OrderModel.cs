namespace NapaProjects.OnlineMarket.Models;

public class OrderModel
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    [ValidateNever]
    public string ProductName { get; set; }

    public float Price { get; set; }

    public int CartId { get; set; }
    public int Quantity { get; set; }

    public OrderModel(Order order)
    {
        Id = order.Id;
        ProductId = order.ProductId;
        ProductName = order.Product.Name;
        Quantity = order.Quantity;
        CartId = order.CartId;
        Price = order.Product.Price;
    }
}
