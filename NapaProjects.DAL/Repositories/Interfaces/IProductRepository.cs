

namespace NapaProjects.DAL.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> Products { get; }

    int Count { get; }

    Product Get(int id);

    IEnumerable<Product> Get(int stage, int take);

    int Update(Product product);

    int Create(Product product);

    bool Delete(int id);

}
