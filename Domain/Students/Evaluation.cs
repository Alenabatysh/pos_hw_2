using Domain.Common;
using Domain.Rubricators;

namespace Domain.Students;

public sealed class Evaluation : Entity<EvaluationId>
{
    public CriterionId CriterionId { get; }
    public Score Score { get; private set; }
    public DateTime Date { get; }
    public string? Comment { get; }

    public Evaluation(EvaluationId id, CriterionId criterionId, Score score, DateTime date, string? comment)
        : base(id)
    {
        if (criterionId == null)
        {
            throw new DomainException("Criterion cannot be null");
        }
        if (score == null)
        {
            throw new DomainException("Score cannot be null");
        }

        CriterionId = criterionId;
        Score = score;
        Date = date;
        Comment = comment;
    }

    internal void ChangeScore(Score newScore)
    {
        if (newScore == null)
        {
            throw new DomainException("Score cannot be null");
        }

        Score = newScore;
    }
}