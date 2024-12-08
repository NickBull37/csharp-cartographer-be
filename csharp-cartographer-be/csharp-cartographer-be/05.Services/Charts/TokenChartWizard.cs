﻿using csharp_cartographer_be._01.Configuration.CSharpElements;
using csharp_cartographer_be._03.Models.Charts;
using csharp_cartographer_be._03.Models.Tokens;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace csharp_cartographer_be._05.Services.Charts
{
    public class TokenChartWizard : ITokenChartWizard
    {
        public void AddFactsAndInsightsToNavTokenCharts(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                foreach (var chart in token.Charts)
                {
                    foreach (var element in CSharpElements.ElementList)
                    {
                        if (chart.Label.Equals(element.Label))
                        {
                            chart.Facts = element.Facts;
                            chart.Insights = element.Insights;
                            chart.Alias = element.Alias;
                        }
                    }
                }
            }
        }

        public void AddHighlightValuesToNavTokenCharts(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                if (token.Index == 90)
                {

                }

                var chartCount = 1;
                foreach (var chart in token.Charts)
                {
                    if (chart.Tokens.Count == 0 || chartCount == 1)
                    {
                        chart.HighlightIndices.Add(token.Index);
                    }
                    else
                    {
                        GetElementIndices(navTokens, chart);
                    }
                    chartCount++;
                }
            }
        }

        private static void GetElementIndices(List<NavToken> navTokens, TokenChart chart)
        {
            List<int> highlightIndices = [];

            // loop through roslyn tokens in chart and create list of strings
            var elementTextStrings = GetElementStrings(chart);

            if (elementTextStrings.Count == 0 || navTokens.Count < elementTextStrings.Count)
            {
                return;
            }


            for (int i = 0; i <= navTokens.Count - elementTextStrings.Count; i++)
            {
                bool isMatch = true;

                for (int j = 0; j < elementTextStrings.Count; j++)
                {
                    // if nav token strings don't match strings in chart, move on
                    if (navTokens[i + j].Text != elementTextStrings[j])
                    {
                        isMatch = false;
                        break;
                    }

                    // if strings do match, check that the spans match also in case strings appear more than once
                    if (chart.Tokens[0].FullSpan != navTokens[i].RoslynToken.FullSpan)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    for (int j = 0; j < elementTextStrings.Count; j++)
                    {
                        highlightIndices.Add(i + j);
                    }
                    break;
                }
            }

            chart.HighlightIndices = highlightIndices;
        }

        private static List<string> GetElementStrings(TokenChart chart)
        {
            List<string> elementStrings = [];

            // trim endOfFile token from list
            if (chart.Tokens.Last().IsKind(SyntaxKind.EndOfFileToken))
            {
                chart.Tokens.RemoveAt(chart.Tokens.Count - 1);
            }

            // trim extra semicolon token from list
            if (chart.Tokens.Last().IsKind(SyntaxKind.SemicolonToken) && !chart.Label.EndsWith("Declaration"))
            {
                chart.Tokens.RemoveAt(chart.Tokens.Count - 1);
            }

            // correction - add lamda to element strings
            if (chart.Label == "Arrow Expression Clause")
            {
                elementStrings.Add("=>");
            }

            foreach (var roslynToken in chart.Tokens)
            {
                elementStrings.Add(roslynToken.Text);
            }

            // Add to list of element strings if necessary
            AddSemiColonString(chart, elementStrings);

            return elementStrings;
        }

        private static void AddSemiColonString(TokenChart chart, List<string> elementStrings)
        {
            if (chart.Label == "UsingDirective"
                || chart.Label == "LocalDeclarationStatement"
                || chart.Label == "ExpressionStatement"
                || chart.Label == "ThrowStatement"
                || chart.Label == "ReturnStatement")
            {
                elementStrings.Add(";");
            }
        }
    }
}
