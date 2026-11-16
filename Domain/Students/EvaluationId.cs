using Domain.Common;

namespace Domain.Students;

public sealed record EvaluationId
{
    public Guid Value { get; }

    public EvaluationId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Evaluation Id cannot be empty");
        }
        Value = value;
    }
    public static EvaluationId New() => new(Guid.NewGuid());
}
