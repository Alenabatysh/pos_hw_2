using Domain.Common;
using Domain.Rubricators;
using Domain.Students;

namespace Application.UseCases.Evaluations;

public sealed class UpdateEvaluationUseCase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IRubricatorRepository _rubricatorRepository;

    public UpdateEvaluationUseCase(
        IStudentRepository studentRepository,
        IRubricatorRepository rubricatorRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
    }

    public void Execute(Guid studentId, Guid evaluationId, double newScoreValue)
    {
        var student = _studentRepository.FindById(new StudentId(studentId));
        if (student == null)
        {
            throw new DomainException("Student not found");
        }

        var rubricator = _rubricatorRepository.Find();
        if (rubricator == null)
        {
            throw new DomainException("Rubricator not found");
        }

        student.UpdateEvaluation(new EvaluationId(evaluationId), newScoreValue, rubricator);

        _studentRepository.Save(student);
    }
}

