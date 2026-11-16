namespace Infrastructure.Persistence.Models;

public class StudentDbModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<EvaluationDbModel> Evaluations { get; set; } = new();
}