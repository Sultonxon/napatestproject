
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NapaProjects.OnlineMarket.Models;

public record class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    [ValidateNever]
    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
    public float Price { get; set; }
    public bool CategoryCheck { get; set; }
    [ValidateNever]
    public string NewCategoryName { get; set; }
    [ValidateNever]
    public string NewCategoryDescription { get; set; }



    public static explicit operator ProductModel(Product product) => new ProductModel
    {
        Id = product.Id,
        Name = product.Name,
        CategoryId = product.CategoryId,
        CategoryName = product.Category.Name,
        Price = product.Price
    };

    public static implicit operator Product(ProductModel product) => new Product
    {
        Id = product.Id,
        Name = product.Name,
        CategoryId = product.CategoryId,
        Price = product.Price
    };
}
