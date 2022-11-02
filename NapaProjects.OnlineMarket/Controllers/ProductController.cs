
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NapaProjects.OnlineMarket.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public IActionResult ProductList() => View(_productRepository.Products.Select(x => (ProductModel)x));

    public IActionResult CreateProduct(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.SelectList = new SelectList(_categoryRepository.Categories
            .Select(x => new { Id = x.Id, Name = x.Name }),"Id","Name");
        return View(new ProductModel());
    }

    [HttpPost]
    public IActionResult CreateProduct(ProductModel newProduct, string returnUrl)
    {
        Product product = newProduct;
        if (newProduct.CategoryCheck)
        {
            var catId = _categoryRepository.Create(new Category
            {
                Name = newProduct.NewCategoryName,
                Description = newProduct.NewCategoryDescription
            });
            product.CategoryId = catId;
        }

        _productRepository.Create(product);

        return Redirect(returnUrl);
    }
    [Authorize(Roles="Admin")]
    [HttpPost]
    public IActionResult DeleteProduct(int id, string returnUrl)
    {
        var result = _productRepository.Delete(id);
        if (!result)
        {
            ModelState.AddModelError("id", $"Product with Id {id} Not Found!");
        }
        return Redirect(returnUrl);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult UpdateProduct(int id, string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.SelectList = new SelectList(_categoryRepository.Categories
            .Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");
        var product = _productRepository.Get(id);
        if(product is null)
        {
            ModelState.AddModelError(nameof(id), "Product with Id {id} Not Found!");
            return Redirect(returnUrl);
        }
        return View((ProductModel)product);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult UpdateProduct(ProductModel product, string returnUrl)
    {
        if (product.CategoryCheck)
        {
            Console.WriteLine("\n\n\n********************************");
            Console.WriteLine("                  " + product.CategoryCheck);
            Console.WriteLine("*****************\n******************\n\n\n\n\n");
            if (string.IsNullOrEmpty(product.NewCategoryName)
                || string.IsNullOrWhiteSpace(product.NewCategoryName))
                ModelState.AddModelError(nameof(product.NewCategoryName),$"Name is required to new category");
            if (string.IsNullOrEmpty(product.NewCategoryDescription)
                || string.IsNullOrWhiteSpace(product.NewCategoryDescription))
                ModelState.AddModelError(nameof(product.NewCategoryDescription),
                            $"Description is required to new category");

        }
        else
        {
            if (product.CategoryId == 0)
                ModelState.AddModelError(nameof(product.CategoryId), "Category is not chosen");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.SelectList = new SelectList(_categoryRepository.Categories
                .Select(x => new { Id = x.Id, Name = x.Name }), "Id", "Name");
            
            return View((ProductModel)product);
        }

        Product updatedProduct = product;

        if (product.CategoryCheck)
        {
            var id = _categoryRepository.Create(new Category
            {
                Name = product.NewCategoryName,
                Description = product.NewCategoryDescription
            });
            updatedProduct.CategoryId = id;
        }
        
        _productRepository.Update(updatedProduct);
        return Redirect(returnUrl);
    }


}
