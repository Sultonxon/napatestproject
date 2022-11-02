namespace NapaProjects.OnlineMarket.Models;

public class HistoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
    public float Price { get; set; }
    public int CategoryId { get; set; }
    public StateHistory State { get; set; }
    public DateTime Date { get; set; }
    public string RelatedCategoryName { get; set; }


    public static explicit operator HistoryModel(ProductHistory history) => new HistoryModel
    {
        Id = history.Id,
        Name = history.Name,
        ProductId = history.RelatedProductId ?? 0,
        CategoryId = history.RelatedCategoryId ?? 0,
        Price = history.Price,
        State = history.State,
        Date = history.Date

    };
}
