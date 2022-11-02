
using NapaProjects.DAL.Repositories;

namespace NapaProjects.DAL.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> Categories => _context.Categories.Include(c => c.Products).ToList();

    public IEnumerable<Product> Products(int id) => _context.Products
        .Where(p => p.CategoryId == id)
        .Include(p => p.Category)
        .Include(p => p.ProductHistories);

    public Category Get(int id) => _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);

    public int Create(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category.Id;
    }

    public bool Delete(int id)
    {
        _context.Categories.Remove(new Category { Id = id });
        return _context.SaveChanges() > 0;
    }


    public int Update(Category category)
    {
        var entity = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
        if(entity is not null)
        {
            entity.Name = category.Name;
            entity.Description = category.Description;
            _context.SaveChanges();
            return category.Id;
        }
        return 0;
    }
}
