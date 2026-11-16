using Domain.Common;
using Domain.Rubricators;
using Domain.Students;
using Domain.Services;
using Application.Interfaces;
using System.Text;

namespace Application.UseCases.Reports;

public sealed class ExportTotalsUseCase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IRubricatorRepository _rubricatorRepository;

    private readonly TotalScoreCalculator _calculator;
    private readonly IReportExporter _exporter;

    public ExportTotalsUseCase(IStudentRepository studentRepository, IRubricatorRepository rubricatorRepository,
        TotalScoreCalculator calculator, IReportExporter exporter)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
        _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
        _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
    }

    public void Execute(string filePath)
    {
        var students = _studentRepository.FindAll();

        var rubricator = _rubricatorRepository.Find();
        if (rubricator == null)
        {
            throw new DomainException("Rubricator not found");
        }

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("StudentId;Name;TotalScore");

        foreach (var student in students)
        {
            stringBuilder.AppendLine($"{student.Id.Value};{student.Name.Value};{_calculator.Calculate(student, rubricator).Value}");
        }

        _exporter.Export(stringBuilder.ToString(), filePath);
    }
}

