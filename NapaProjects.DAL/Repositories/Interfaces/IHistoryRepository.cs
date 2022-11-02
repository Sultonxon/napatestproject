using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapaProjects.DAL.Repositories;

public interface IHistoryRepository
{
    IEnumerable<ProductHistory> ProductHistory { get; }

    int Count { get; }

    IEnumerable<ProductHistory> GetBy(int stage, int take);

    ProductHistory Get(int id);


}
