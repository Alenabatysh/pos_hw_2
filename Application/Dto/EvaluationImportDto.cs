namespace Application.Dto;

public sealed record EvaluationImportDto(
    Guid StudentId,
    Guid CriterionId,
    double Score,
    string? Comment);