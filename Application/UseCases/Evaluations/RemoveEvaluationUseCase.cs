using Domain.Common;
using Domain.Students;

namespace Application.UseCases.Evaluations;

public sealed class RemoveEvaluationUseCase
{
    private readonly IStudentRepository _studentRepository;

    public RemoveEvaluationUseCase(
        IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
    }

    public void Execute(Guid studentId, Guid evaluationId)
    {
        var student = _studentRepository.FindById(new StudentId(studentId));
        if (student == null)
        {
            throw new DomainException("Student not found");
        }

        student.RemoveEvaluation(new EvaluationId(evaluationId));
        _studentRepository.Save(student);
    }
}
