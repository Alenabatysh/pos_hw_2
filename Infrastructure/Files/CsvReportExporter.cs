using Application.Interfaces;

namespace Infrastructure.Files;

public sealed class CsvReportExporter : IReportExporter
{
    public void Export(string content, string filePath)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be empty", nameof(filePath));
        }

        File.WriteAllText(filePath, content);
    }
}