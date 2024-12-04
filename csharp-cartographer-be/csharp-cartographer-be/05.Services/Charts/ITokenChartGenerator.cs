using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Charts
{
    public interface ITokenChartGenerator
    {
        void GenerateTokenCharts(List<NavToken> navTokens);
    }
}
