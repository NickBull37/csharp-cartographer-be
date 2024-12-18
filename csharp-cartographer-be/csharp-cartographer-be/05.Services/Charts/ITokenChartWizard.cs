﻿using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Charts
{
    public interface ITokenChartWizard
    {
        void AddFactsAndInsightsToNavTokenCharts(List<NavToken> navTokens);

        void AddHighlightValuesToNavTokenCharts(List<NavToken> navTokens);
    }
}
