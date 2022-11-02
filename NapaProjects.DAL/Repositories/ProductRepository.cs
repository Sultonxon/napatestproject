


namespace NapaProjects.DAL.Repositories;

public class ProductRepository: IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> Products => _context.Products
        .Include(p => p.ProductHistories)
        .Include(p => p.Category).ToList();

    public int Count => _context.Products.Count();

    public int Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();

        _context.ProductHistory.Add(new ProductHistory
        {
            Name = product.Name,
            RelatedCategoryId = product.CategoryId,
            Price = product.Price,
            RelatedProductId = product.Id,
            Date = DateTime.Now,
            State = StateHistory.Created
        });
        _context.SaveChanges();
        return product.Id;
    }

    public bool Delete(int id)
    {
        var entity = _context.Products.FirstOrDefault(p => p.Id == id);
        if (entity == null) return false;
        _context.ProductHistory.Add(new ProductHistory
        {
            Name = entity.Name,
            RelatedCategoryId = entity.CategoryId,
            RelatedProductId = entity.Id,
            Price = entity.Price,
            Date = DateTime.Now,
            State = StateHistory.Removed
        });
        _context.Products.Remove(entity);
        _context.SaveChanges();
        return true;
    }

    public Product Get(int id) => _context.Products
        .Include(p => p.ProductHistories)
        .Include(p => p.Category)
        .FirstOrDefault(x => x.Id == id) ?? new Product();

    public IEnumerable<Product> Get(int stage, int take) =>
                _context.Products.Skip((stage - 1) * take).Take(take);

    public int Update(Product product)
    {
        var entitiy = _context.Products.FirstOrDefault(x => x.Id == product.Id);

        if (entitiy is not null)
        {
            entitiy.Name = product.Name;
            entitiy.CategoryId = product.CategoryId;
            entitiy.Price = product.Price;

            _context.ProductHistory.Add(new ProductHistory
            {
                Name = product.Name,
                RelatedCategoryId = product.CategoryId,
                RelatedProductId = product.Id,
                Price = product.Price,
                Date = DateTime.Now,
                State = StateHistory.Updated
            }) ;

            _context.SaveChanges();
            return product.Id;
        }

        return 0;
    }
}
