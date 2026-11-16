using Domain.Common;

namespace Domain.Rubricators;

public sealed record Score
{
    public double Value { get; }

    public Score(double value)
    {
        if (value < 0)
        {
            throw new DomainException("Score cannot be < 0");
        }
        Value = value;
    }
}
