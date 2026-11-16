using Domain.Common;

namespace Domain.Students;

public sealed record StudentName
{
    public string Value { get; }

    public StudentName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Name cannot be empty");
        }
        Value = value.Trim();
    }
}
