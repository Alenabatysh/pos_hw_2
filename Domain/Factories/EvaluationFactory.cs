using Domain.Common;
using Domain.Rubricators;
using Domain.Students;

namespace Domain.Factories;

public sealed class EvaluationFactory
{
    public Evaluation Create(
        Student student,
        Rubricator rubricator,
        CriterionId criterionId,
        double scoreValue,
        string? comment)
    {
        if (student == null || rubricator == null || criterionId == null)
        {
            throw new DomainException("Main information cannot be null");
        }

        var criterion = rubricator.GetCriterion(criterionId);
        if (criterion == null)
        {
            throw new DomainException("Criterion not found in rubricator");
        }

        var score = new Score(scoreValue);

        if (score.Value > criterion.MaxScore)
        {
            throw new DomainException("Score exceeds max score");
        }

        var evaluation = new Evaluation(EvaluationId.New(), criterionId, score, DateTime.UtcNow, comment);

       student.AddEvaluation(evaluation);

        return evaluation;
    }
}