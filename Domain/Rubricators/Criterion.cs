using Domain.Common;

namespace Domain.Rubricators;

public sealed class Criterion : Entity<CriterionId>
{
    public string Name { get; }
    public double MaxScore { get; }
    public Weight Weight { get; }

    public Criterion(CriterionId id, string name, double maxScore, Weight weight)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Criterion name cannot be empty");
        }

        if (maxScore <= 0)
        {
            throw new DomainException("Max score must be positive");
        }

        Name = name;
        MaxScore = maxScore;
        Weight = weight;
    }
}