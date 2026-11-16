using Domain.Common;
using Domain.Rubricators;
using Domain.Students;

namespace Domain.Services;

public sealed class TotalScoreCalculator
{
    private readonly ITotalScoreStrategy _strategy;

    public TotalScoreCalculator(ITotalScoreStrategy strategy)
    {
        _strategy = strategy ?? throw new DomainException("Strategy cannot be null");
    }

    public TotalScore Calculate(Student student, Rubricator rubricator)
    {
        if (student == null || rubricator == null)
        {
            throw new DomainException("Student and Rubricator cannot be null");
        }

        var items = new List<(Score Score, Criterion Criterion)>();

        foreach (var evaluation in student.Evaluations)
        {
            var criterion = rubricator.GetCriterion(evaluation.CriterionId);

            if (criterion == null)
            {
                throw new DomainException($"Criterion {evaluation.CriterionId.Value} not found in rubricator");
            }

            items.Add((evaluation.Score, criterion));
        }

        return _strategy.Calculate(items);
    }
}