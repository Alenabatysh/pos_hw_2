using Domain.Common;

namespace Domain.Students;

public sealed record StudentId
{
    public Guid Value { get; }

    public StudentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Student Id cannot be empty");
        }
        Value = value;
    }
    public static StudentId New() => new(Guid.NewGuid());
}
