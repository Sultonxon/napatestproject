
namespace NapaProjects.DAL.Repositories
{
    public class HistoryRepository: IHistoryRepository
    {
        private readonly AppDbContext _context;

        public HistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductHistory> ProductHistory => _context.ProductHistory.ToList();

        public int Count => _context.ProductHistory.Count();

        public ProductHistory Get(int id) => _context.ProductHistory.Find(id);

        public IEnumerable<ProductHistory> GetBy(int stage, int take) => _context.ProductHistory
                        .Skip((stage - 1) * take).Take(take);
        
    }
}
