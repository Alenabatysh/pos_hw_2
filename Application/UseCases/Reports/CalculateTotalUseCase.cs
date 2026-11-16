using Domain.Common;
using Domain.Rubricators;
using Domain.Students;
using Domain.Services;
using Application.Dto;

namespace Application.UseCases.Reports;

public sealed class CalculateTotalUseCase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IRubricatorRepository _rubricatorRepository;

    private readonly TotalScoreCalculator _calculator;

    public CalculateTotalUseCase(IStudentRepository studentRepository,
        IRubricatorRepository rubricatorRepository, TotalScoreCalculator calculator)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
        _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
    }

    public TotalScoreDto Execute(Guid studentId)
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

        TotalScore result = _calculator.Calculate(student, rubricator);
        return new TotalScoreDto(student.Id.Value, student.Name.Value, result.Value);
    }
}

