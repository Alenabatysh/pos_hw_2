using Application.Dto;
using Domain.Rubricators;

namespace Application.UseCases.Rubricators;

public sealed class AddCriterionUseCase
{
    private readonly IRubricatorRepository _rubricatorRepository;

    public AddCriterionUseCase(IRubricatorRepository rubricatorRepository)
    {
        _rubricatorRepository = rubricatorRepository ?? throw new ArgumentNullException(nameof(rubricatorRepository));
    }

    public CriterionDto Execute(string name, double maxScore, double weight)
    {
        Rubricator? rubricator = _rubricatorRepository.Find();
        if (rubricator == null)
        {
            rubricator = new Rubricator();
        }

        Weight criterionWeight = new Weight(weight);
        Criterion criterion = new Criterion(CriterionId.New(), name, maxScore, criterionWeight);
        rubricator.AddCriterion(criterion);
        _rubricatorRepository.Save(rubricator);

        return new CriterionDto(criterion.Id.Value, criterion.Name, criterion.MaxScore, criterion.Weight.Value);
    }
}