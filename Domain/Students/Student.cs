using Domain.Common;
using Domain.Rubricators;

namespace Domain.Students;

public sealed class Student : Entity<StudentId>, IAggregateRoot
{
    public StudentName Name { get; }

    private readonly List<Evaluation> _evaluations = new();

    public IReadOnlyList<Evaluation> Evaluations => _evaluations;

    public Student(StudentId id, StudentName name) : base(id)
    {
        if (name == null)
        {
            throw new DomainException("Name cannot be null");
        }

        Name = name;
    }

    public void AddEvaluation(Evaluation evaluation)
    {
        if (evaluation == null)
        {
            throw new DomainException("Evaluation cannot be null");
        }

        if (_evaluations.Any(el => el.CriterionId == evaluation.CriterionId))
        {
            throw new DomainException("Evaluation for this criterion already exists");
        }

        _evaluations.Add(evaluation);
    }

    public void RemoveEvaluation(EvaluationId evaluationId)
    {
        if (evaluationId == null)
        {
            throw new DomainException("Evaluation id cannot be null");
        }

        Evaluation? answer = _evaluations.FirstOrDefault(el => el.Id == evaluationId);
        if (answer == null)
        {
            throw new DomainException("Evaluation not found");
        }
        _evaluations.Remove(answer);
    }

    public Evaluation? GetEvaluation(EvaluationId evaluationId)
    {
        if (evaluationId == null)
        {
            throw new DomainException("Evaluation id cannot be null");
        }

        Evaluation? answer = _evaluations.FirstOrDefault(el => el.Id == evaluationId);
        return answer;
    }

    public void UpdateEvaluation(EvaluationId evaluationId, double newScoreValue, Rubricator rubricator)
    {
        if (evaluationId == null || rubricator == null)
        {
            throw new DomainException("Arguments cannot be null");
        }

        var evaluation = GetEvaluation(evaluationId);
        if (evaluation == null)
        {
            throw new DomainException("Evaluation not found");
        }

        var criterion = rubricator.GetCriterion(evaluation.CriterionId);
        if (criterion == null)
        {
            throw new DomainException("Criterion not found");
        }

        var newScore = new Score(newScoreValue);
        if (newScore.Value > criterion.MaxScore)
        {
            throw new DomainException("Score exceeds max score");
        }

        evaluation.ChangeScore(newScore);
    }
}