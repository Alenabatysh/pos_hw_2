namespace Application.Interfaces;

public interface IReportExporter
{
    void Export(string content, string filePath);
}