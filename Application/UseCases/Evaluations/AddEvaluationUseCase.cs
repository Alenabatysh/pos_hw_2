using Application.Dto;
using Domain.Common;
using Domain.Factories;
using Domain.Rubricators;
using Domain.Students;

namespace Application.UseCases.Evaluations;

public sealed class AddEvaluationUseCase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IRubricatorRepository _rubricatorRepository;
    private readonly EvaluationFactory _factory;

    public AddEvaluationUseCase(
        IStudentRepository studentRepository,
        IRubricatorRepository rubricatorRepository,
        EvaluationFactory factory)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public EvaluationDto Execute(Guid studentId, Guid criterionId, double scoreValue, string? comment)
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

        var evaluation = _factory.Create(student, rubricator, new CriterionId(criterionId), scoreValue, comment);

        _studentRepository.Save(student);

        return new EvaluationDto(
            evaluation.Id.Value,
            evaluation.CriterionId.Value,
            evaluation.Score.Value,
            evaluation.Date,
            evaluation.Comment);
    }
}