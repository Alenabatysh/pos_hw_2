using Domain.Common;

namespace Domain.Rubricators;

public sealed class Rubricator : IAggregateRoot
{
    private readonly List<Criterion> _criteria = new();

    public IReadOnlyList<Criterion> Criteria => _criteria;

    public Rubricator() { }

    public void AddCriterion(Criterion criterion)
    {
        if (criterion == null)
        {
            throw new DomainException("Criterion cannot be null");
        }
        if (_criteria.Any(el => el.Id == criterion.Id))
        {
            throw new DomainException("Criterion with this Id already exists");
        }
        _criteria.Add(criterion);
    }

    public Criterion? GetCriterion(CriterionId id)
    {
        Criterion? answer = _criteria.FirstOrDefault(el => el.Id == id);
        return answer;
    }
}