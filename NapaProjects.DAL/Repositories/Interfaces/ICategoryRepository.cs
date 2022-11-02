
namespace NapaProjects.DAL.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> Categories { get; }

    IEnumerable<Product> Products(int id);

    Category Get(int id);

    int Create(Category category);

    int Update(Category category);

    bool Delete(int id);
}
