using Domain.Rubricators;

namespace Domain.Services;

public interface ITotalScoreStrategy
{
    TotalScore Calculate(IReadOnlyList<(Score Score, Criterion Criterion)> items);
}