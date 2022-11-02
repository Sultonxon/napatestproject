

namespace NapaProjects.DAL.Models;

public class ProductHistory
{
    [Key]
    public int Id { get; set; }
    
    public int? RelatedProductId { get; set; }

    public StateHistory State { get; set; }

    public string Name { get; set; }

    public int? RelatedCategoryId { get; set; }

    public float Price { get; set; }

    public DateTime Date { get; set; }
}

public enum StateHistory
{
    Created, Updated, Removed
}