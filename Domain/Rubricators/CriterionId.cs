using Domain.Common;

namespace Domain.Rubricators;

public sealed record CriterionId
{
    public Guid Value { get; }

    public CriterionId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Criterion Id cannot be empty");
        }
        Value = value;
    }

    public static CriterionId New() => new(Guid.NewGuid());
}