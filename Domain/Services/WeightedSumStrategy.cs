using Domain.Rubricators;

namespace Domain.Services;

public sealed class WeightedSumStrategy : ITotalScoreStrategy
{
    public TotalScore Calculate(IReadOnlyList<(Score Score, Criterion Criterion)> items)
    {
        if (items == null || items.Count == 0)
        {
            return new TotalScore(0);
        }

        double total = 0;

        foreach (var item in items)
        {
            double normalizedScore = item.Score.Value / item.Criterion.MaxScore;
            total += normalizedScore * item.Criterion.Weight.Value * 10;
        }

        return new TotalScore(total);
    }
}