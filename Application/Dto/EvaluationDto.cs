namespace Application.Dto;

public sealed record EvaluationDto(
    Guid Id,
    Guid CriterionId,
    double Score,
    DateTime Date,
    string? Comment);