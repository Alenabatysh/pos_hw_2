using Application.Interfaces;

namespace Infrastructure.Files;

public sealed class CsvReportImporter : IReportImporter
{
    public IReadOnlyList<string[]> Import(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File path is not found", nameof(filePath));
        }

        List<string[]> result = new();
        string[] strings = File.ReadAllLines(filePath);
        for (int i = 0; i < strings.Length; i++)
        {
            if (string.IsNullOrEmpty(strings[i]))
            {
                continue;
            }
            result.Add(strings[i].Split(';'));
        }

        return result;
    }
}