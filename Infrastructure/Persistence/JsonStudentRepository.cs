using System.Text.Json;
using Domain.Students;
using Domain.Rubricators;
using Infrastructure.Persistence.Models;

namespace Infrastructure.Persistence;

public sealed class JsonStudentRepository : IStudentRepository
{
    private readonly string _filePath;

    public JsonStudentRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IReadOnlyList<Student> FindAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Student>();
        }

        var json = File.ReadAllText(_filePath);
        var dbModels = JsonSerializer.Deserialize<List<StudentDbModel>>(json) ?? new();

        return dbModels.Select(MapToDomain).ToList();
    }

    public Student? FindById(StudentId id)
    {
        return FindAll().FirstOrDefault(s => s.Id == id);
    }

    public void Save(Student student)
    {
        var students = FindAll().ToList();
        var existingIndex = students.FindIndex(s => s.Id == student.Id);

        if (existingIndex >= 0)
        {
            students[existingIndex] = student;
        }
        else
        {
            students.Add(student);
        }

        SaveToFile(students);
    }

    public void Delete(StudentId id)
    {
        var students = FindAll().Where(s => s.Id != id).ToList();
        SaveToFile(students);
    }

    private void SaveToFile(List<Student> students)
    {
        var dbModels = students.Select(MapToDbModel).ToList();
        var json = JsonSerializer.Serialize(dbModels, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    private Student MapToDomain(StudentDbModel db)
    {
        var student = new Student(new StudentId(db.Id), new StudentName(db.Name));

        foreach (var evalDb in db.Evaluations)
        {
            var evaluation = new Evaluation(
                new EvaluationId(evalDb.Id),
                new CriterionId(evalDb.CriterionId),
                new Score(evalDb.Score),
                evalDb.Date,
                evalDb.Comment
            );

            student.AddEvaluation(evaluation);
        }
        return student;
    }

    private StudentDbModel MapToDbModel(Student domain)
    {
        return new StudentDbModel
        {
            Id = domain.Id.Value,
            Name = domain.Name.Value,
            Evaluations = domain.Evaluations.Select(e => new EvaluationDbModel
            {
                Id = e.Id.Value,
                CriterionId = e.CriterionId.Value,
                Score = e.Score.Value,
                Date = e.Date,
                Comment = e.Comment
            }).ToList()
        };
    }
}