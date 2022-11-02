
namespace NapaProjects.DAL.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public float Price { get; set; }

    public IEnumerable<ProductHistory> ProductHistories { get; set; }
}
