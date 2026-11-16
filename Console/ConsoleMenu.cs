using Application.UseCases.Evaluations;
using Application.UseCases.Reports;
using Application.UseCases.Rubricators;
using Application.UseCases.Students;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace PosConsole;

public class ConsoleMenu
{
    private readonly IServiceProvider _provider;

    public ConsoleMenu(IServiceProvider provider) => _provider = provider;

    public void Run()
    {
        while (true)
        {
            System.Console.Clear();
            System.Console.WriteLine("=== POS MODULE ===");
            System.Console.WriteLine("1. Add student");
            System.Console.WriteLine("2. Show all students");
            System.Console.WriteLine("3. Add criterion");
            System.Console.WriteLine("4. Show criteria (rubricator)");
            System.Console.WriteLine("5. Assign evaluation to student");
            System.Console.WriteLine("6. Show student evaluations");
            System.Console.WriteLine("7. Calculate total score of student");
            System.Console.WriteLine("8. Export totals to CSV");
            System.Console.WriteLine("9. Import evaluations from CSV");
            System.Console.WriteLine("0. Exit");
            System.Console.Write("\nChoice: ");

            var choice = System.Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": ShowStudents(); break;
                    case "3": AddCriterion(); break;
                    case "4": ShowRubricator(); break;
                    case "5": AddEvaluation(); break;
                    case "6": ShowEvaluations(); break;
                    case "7": CalculateTotal(); break;
                    case "8": ExportTotals(); break;
                    case "9": ImportEvaluations(); break;
                    case "0": return;
                    default: System.Console.WriteLine("Invalid input!"); System.Console.ReadKey(); break;
                }
            }
            catch (DomainException ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"\nDOMAIN ERROR: {ex.Message}");
                System.Console.ResetColor();
                System.Console.WriteLine("Press any key...");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"\nSYSTEM ERROR: {ex.Message}");
                System.Console.ResetColor();
                System.Console.WriteLine("Press any key...");
                System.Console.ReadKey();
            }
        }
    }

    private void AddStudent()
    {
        System.Console.Write("Student name: ");
        var name = System.Console.ReadLine() ?? "";

        var useCase = _provider.GetRequiredService<AddStudentUseCase>();
        var result = useCase.Execute(name);

        System.Console.WriteLine($"Student added! ID: {result.Id}");
        System.Console.ReadKey();
    }

    private void ShowStudents()
    {
        var useCase = _provider.GetRequiredService<GetAllStudentsUseCase>();
        var students = useCase.Execute();

        if (students.Count == 0)
        {
            System.Console.WriteLine("No students found.");
        }
        foreach (var s in students)
        {
            System.Console.WriteLine($"ID: {s.Id} | Name: {s.Name}");
        }

        System.Console.ReadKey();
    }

    private void AddCriterion()
    {
        System.Console.Write("Criterion name: ");
        var name = System.Console.ReadLine() ?? "";
        System.Console.Write("Max score: ");
        var max = double.Parse(System.Console.ReadLine() ?? "0");
        System.Console.Write("Weight (0-1): ");
        var weight = double.Parse(System.Console.ReadLine() ?? "0");

        var useCase = _provider.GetRequiredService<AddCriterionUseCase>();
        var result = useCase.Execute(name, max, weight);

        System.Console.WriteLine($"Criterion '{result.Name}' added! ID: {result.Id}");
        System.Console.ReadKey();
    }

    private void ShowRubricator()
    {
        var useCase = _provider.GetRequiredService<GetRubricatorUseCase>();
        var criteria = useCase.Execute();

        if (criteria.Count == 0)
        {
            System.Console.WriteLine("No criteria found.");
        }
        foreach (var c in criteria)
        {
            System.Console.WriteLine($"ID: {c.Id} | {c.Name} | Max: {c.MaxScore} | Weight: {c.Weight}");
        }

        System.Console.ReadKey();
    }

    private void AddEvaluation()
    {
        System.Console.Write("Student ID: ");
        var studentId = Guid.Parse(System.Console.ReadLine() ?? "");
        System.Console.Write("Criterion ID: ");
        var criterionId = Guid.Parse(System.Console.ReadLine() ?? "");
        System.Console.Write("Score: ");
        var score = double.Parse(System.Console.ReadLine() ?? "0");
        System.Console.Write("Comment: ");
        var comment = System.Console.ReadLine();

        var useCase = _provider.GetRequiredService<AddEvaluationUseCase>();
        useCase.Execute(studentId, criterionId, score, comment);

        System.Console.WriteLine("Evaluation assigned!");
        System.Console.ReadKey();
    }

    private void ShowEvaluations()
    {
        System.Console.Write("Student ID: ");
        var studentId = Guid.Parse(System.Console.ReadLine() ?? "");

        var useCase = _provider.GetRequiredService<GetStudentEvaluationsUseCase>();
        var evals = useCase.Execute(studentId);

        if (evals.Count == 0)
        {
            System.Console.WriteLine("No evaluations found for this student.");
        }
        foreach (var e in evals)
        {
            System.Console.WriteLine($"Criterion: {e.CriterionId} | Score: {e.Score} | Comment: {e.Comment}");
        }

        System.Console.ReadKey();
    }

    private void CalculateTotal()
    {
        System.Console.Write("Student ID: ");
        var studentId = Guid.Parse(System.Console.ReadLine() ?? "");

        var useCase = _provider.GetRequiredService<CalculateTotalUseCase>();
        var result = useCase.Execute(studentId);

        System.Console.WriteLine($"\nStudent: {result.StudentName} | Total score: {result.TotalScore}");
        System.Console.ReadKey();
    }

    private void ExportTotals()
    {
        System.Console.Write("File path (e.g., report.csv): ");
        var path = System.Console.ReadLine() ?? "report.csv";

        var useCase = _provider.GetRequiredService<ExportTotalsUseCase>();
        useCase.Execute(path);

        System.Console.WriteLine("Export completed!");
        System.Console.ReadKey();
    }

    private void ImportEvaluations()
    {
        System.Console.Write("CSV file path (e.g., import.csv): ");
        var path = System.Console.ReadLine() ?? "import.csv";

        var useCase = _provider.GetRequiredService<ImportCsvEvaluationsUseCase>();
        useCase.Execute(path);

        System.Console.WriteLine("Import completed!");
        System.Console.ReadKey();
    }
}