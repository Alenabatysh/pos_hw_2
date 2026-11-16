namespace Application.Interfaces;

public interface IReportImporter
{
    IReadOnlyList<string[]> Import(string filePath);
}