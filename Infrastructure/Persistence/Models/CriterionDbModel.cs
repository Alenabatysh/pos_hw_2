namespace Infrastructure.Persistence.Models;

public class CriterionDbModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double MaxScore { get; set; }
    public double Weight { get; set; }
}