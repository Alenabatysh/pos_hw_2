using Application.Interfaces;
using Application.UseCases.Evaluations;
using Application.UseCases.Reports;
using Application.UseCases.Rubricators;
using Application.UseCases.Students;
using Domain.Factories;
using Domain.Rubricators;
using Domain.Services;
using Domain.Students;
using Infrastructure.Files;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace PosConsole;

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStudentRepository>(new JsonStudentRepository("students.json"));
        services.AddSingleton<IRubricatorRepository>(new JsonRubricatorRepository("rubricator.json"));
        services.AddSingleton<IReportExporter, CsvReportExporter>();
        services.AddSingleton<IReportImporter, CsvReportImporter>();

        services.AddSingleton<EvaluationFactory>();
        services.AddSingleton<ITotalScoreStrategy, WeightedSumStrategy>();
        services.AddSingleton<TotalScoreCalculator>();

        services.AddTransient<AddStudentUseCase>();
        services.AddTransient<GetAllStudentsUseCase>();
        services.AddTransient<AddCriterionUseCase>();
        services.AddTransient<GetRubricatorUseCase>();
        services.AddTransient<AddEvaluationUseCase>();
        services.AddTransient<GetStudentEvaluationsUseCase>();
        services.AddTransient<UpdateEvaluationUseCase>();
        services.AddTransient<RemoveEvaluationUseCase>();
        services.AddTransient<CalculateTotalUseCase>();
        services.AddTransient<ExportTotalsUseCase>();
        services.AddTransient<ImportCsvEvaluationsUseCase>();

        var provider = services.BuildServiceProvider();

        var menu = new ConsoleMenu(provider);
        menu.Run();
    }
}