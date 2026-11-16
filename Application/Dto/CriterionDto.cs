namespace Application.Dto;

public sealed record CriterionDto(Guid Id, string Name, double MaxScore, double Weight);