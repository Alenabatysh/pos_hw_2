using Domain.Common;

namespace Domain.Services;

public sealed record TotalScore
{
    public double Value { get; }

    public TotalScore(double value)
    {
        if (value < 0)
        {
            throw new DomainException("Total score cannot be negative");
        }
        Value = value;
    }
}