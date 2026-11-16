using Application.Dto;
using Domain.Common;
using Domain.Factories;
using Domain.Rubricators;
using Domain.Students;

namespace Application.UseCases.Reports;

public abstract class BaseImportEvaluationsUseCase
{
    protected readonly IStudentRepository _studentRepository;
    protected readonly IRubricatorRepository _rubricatorRepository;
    protected readonly EvaluationFactory _factory;

    protected BaseImportEvaluationsUseCase(
        IStudentRepository studentRepository,
        IRubricatorRepository rubricatorRepository,
        EvaluationFactory factory)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void Execute(string filePath)
    {
        var parsedRows = ParseFile(filePath);

        var rubricator = _rubricatorRepository.Find();
        if (rubricator == null)
        {
            throw new DomainException("Rubricator is not configured");
        }

        foreach (var row in parsedRows)
        {
            var student = _studentRepository.FindById(new StudentId(row.StudentId));
            if (student == null)
            {
                throw new DomainException("Student not found");
            }

            _factory.Create(student, rubricator, new CriterionId(row.CriterionId), row.Score, row.Comment);
            _studentRepository.Save(student);
        }
    }

    protected abstract List<EvaluationImportDto> ParseFile(string filePath);
}