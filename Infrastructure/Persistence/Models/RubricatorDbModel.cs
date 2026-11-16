namespace Infrastructure.Persistence.Models;

public class RubricatorDbModel
{
    public List<CriterionDbModel> Criteria { get; set; } = new();
}