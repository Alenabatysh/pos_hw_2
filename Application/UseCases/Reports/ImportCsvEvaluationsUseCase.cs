using Application.Dto;
using Domain.Factories;
using Domain.Rubricators;
using Domain.Students;

namespace Application.UseCases.Reports;

public sealed class ImportCsvEvaluationsUseCase : BaseImportEvaluationsUseCase
{
    public ImportCsvEvaluationsUseCase(
        IStudentRepository studentRepository,
        IRubricatorRepository rubricatorRepository,
        EvaluationFactory factory)
        : base(studentRepository, rubricatorRepository, factory)
    {
    }

    protected override List<EvaluationImportDto> ParseFile(string filePath)
    {
        var result = new List<EvaluationImportDto>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(';');
            if (parts.Length < 3)
            {
                continue;
            }

            if (!Guid.TryParse(parts[0].Trim(), out var studentId))
            {
                continue;
            }
            if (!Guid.TryParse(parts[1].Trim(), out var criterionId))
            {
                continue;
            }
            if (!double.TryParse(parts[2].Trim(), out var score))
            {
                continue;
            }

            var comment = parts.Length > 3 ? parts[3].Trim() : null;

            result.Add(new EvaluationImportDto(studentId, criterionId, score, comment));
        }

        return result;
    }
}