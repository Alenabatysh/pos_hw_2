using System.Text.Json;
using Domain.Rubricators;
using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

public sealed class JsonRubricatorRepository : IRubricatorRepository
{
    private readonly string _filePath;

    public JsonRubricatorRepository(string filePath)
    {
        _filePath = filePath;
    }

    public Rubricator? Find()
    {
        if (!File.Exists(_filePath))
        {
            return new Rubricator();
        }

        var json = File.ReadAllText(_filePath);
        var dbModel = JsonSerializer.Deserialize<RubricatorDbModel>(json);

        if (dbModel == null)
        {
            return new Rubricator();
        }

        var rubricator = new Rubricator();
        foreach (var c in dbModel.Criteria)
        {
            var criterion = new Criterion(
                new CriterionId(c.Id),
                c.Name,
                c.MaxScore,
                new Weight(c.Weight)
            );
            rubricator.AddCriterion(criterion);
        }

        return rubricator;
    }

    public void Save(Rubricator rubricator)
    {
        var dbModel = new RubricatorDbModel
        {
            Criteria = rubricator.Criteria.Select(c => new CriterionDbModel
            {
                Id = c.Id.Value,
                Name = c.Name,
                MaxScore = c.MaxScore,
                Weight = c.Weight.Value
            }).ToList()
        };

        var json = JsonSerializer.Serialize(dbModel, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}