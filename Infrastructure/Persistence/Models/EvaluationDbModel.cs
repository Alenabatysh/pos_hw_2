namespace Infrastructure.Persistence.Models;

public class EvaluationDbModel
{
    public Guid Id { get; set; }
    public Guid CriterionId { get; set; }
    public double Score { get; set; }
    public DateTime Date { get; set; }
    public string? Comment { get; set; }
}