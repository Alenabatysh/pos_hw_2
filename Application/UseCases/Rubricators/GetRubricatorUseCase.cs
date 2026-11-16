using Application.Dto;
using Domain.Rubricators;

namespace Application.UseCases.Rubricators;

public sealed class GetRubricatorUseCase
{
    private readonly IRubricatorRepository _rubricatorRepository;

    public GetRubricatorUseCase(IRubricatorRepository rubricatorRepository)
    {
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
    }

    public IReadOnlyList<CriterionDto> Execute()
    {
        Rubricator? rubricator = _rubricatorRepository.Find();
        if (rubricator == null)
        {
            return new List<CriterionDto>();
        }

        var criteria = rubricator.Criteria;

        return criteria
            .Select(c => new CriterionDto(c.Id.Value, c.Name, c.MaxScore, c.Weight.Value))
            .ToList();
    }
}