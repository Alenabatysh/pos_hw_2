using Application.Dto;
using Domain.Common;
using Domain.Students;

namespace Application.UseCases.Evaluations;

public sealed class GetStudentEvaluationsUseCase
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentEvaluationsUseCase(
        IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
    }

    public IReadOnlyList<EvaluationDto> Execute(Guid studentId)
    {
        var student = _studentRepository.FindById(new StudentId(studentId));
        if (student == null)
        {
            throw new DomainException("Student not found");
        }

        var evaluations = student.Evaluations
            .Select(e => new EvaluationDto(e.Id.Value, e.CriterionId.Value, e.Score.Value, e.Date, e.Comment))
            .ToList();
        return evaluations;
    }
}