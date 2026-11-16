using Domain.Common;

namespace Domain.Rubricators;

public sealed record Weight
{
    public double Value { get; }

    public Weight(double value)
    {
        if (value <= 0 || value > 1)
        {
            throw new DomainException("Weight cannot be <= 0 or > 1");
        }
        Value = value;
    }
}
