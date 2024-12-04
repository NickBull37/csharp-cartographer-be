﻿using csharp_cartographer_be._01.Configuration.ReservedText;
using csharp_cartographer_be._02.Utilities.Charts;
using csharp_cartographer_be._03.Models.Tokens;
using Microsoft.CodeAnalysis.CSharp;

namespace csharp_cartographer_be._05.Services.Highlighting
{
    public class SyntaxHighlighter : ISyntaxHighlighter
    {
        public void AddSyntaxHighlightingToNavTokens(List<NavToken> navTokens)
        {
            AddReservedTextHighlighting(navTokens);

            AddLiteralHighlighting(navTokens);

            AddIdentifierHighlighting(navTokens);

            AddIdentifierReferenceHighlighting(navTokens);

            AddHighlightingThatNeedsSurroundingTokens(navTokens);
        }

        private static void AddReservedTextHighlighting(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                // default keyword can be blue or purple
                if (token.Text == "default" && token.Tags[1].Label == "DefaultSwitchLabel")
                {
                    token.HighlightColor = "color-purple";
                    continue;
                }
                else if (token.Text == "default" && token.Tags[1].Label == "DefaultLiteralExpression")
                {
                    token.HighlightColor = "color-blue";
                    continue;
                }

                foreach (var element in ReservedTextElements.ElementList)
                {
                    if (token.Text.Equals(element.Text))
                    {
                        token.HighlightColor = element.HighlightColor;
                    }
                }
            }
        }

        private static void AddLiteralHighlighting(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                if (HighlightColorAlreadySet(token))
                {
                    continue;
                }

                // string literals
                if (token.Kind == SyntaxKind.StringLiteralToken)
                {
                    token.HighlightColor = "color-orange";
                }
                // numeric literals (integer, decimal, float)
                if (token.Kind == SyntaxKind.NumericLiteralToken
                    || token.Kind.ToString() == "DecimalLiteralToken"
                    || token.Kind.ToString() == "FloatLiteralToken")
                {
                    token.HighlightColor = "color-light-green";
                }
            }
        }

        private static void AddIdentifierHighlighting(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                if (HighlightColorAlreadySet(token))
                {
                    continue;
                }

                // using directive identifier
                if (ChartNavigator.IsUsingDirective(token))
                {
                    token.HighlightColor = "color-white";
                    continue;
                }

                // interface delclaration identifier
                if (ChartNavigator.IsInterfaceDeclaration(token))
                {
                    token.HighlightColor = "color-light-green";
                    continue;
                }

                // class delclaration identifier
                if (ChartNavigator.IsClassDeclaration(token))
                {
                    token.HighlightColor = "color-green";
                    continue;
                }

                // constructor delclaration identifier
                if (ChartNavigator.IsConstructorDeclaration(token))
                {
                    token.HighlightColor = "color-green";
                    continue;
                }

                // method delclaration identifier
                if (ChartNavigator.IsMethodDeclaration(token))
                {
                    token.HighlightColor = "color-yellow";
                    continue;
                }

                // variable delclaration identifier
                if (ChartNavigator.IsVariableDeclaration(token))
                {
                    token.HighlightColor = "color-light-blue";
                    continue;
                }

                // parameter identifier
                if (ChartNavigator.IsParameter(token))
                {
                    token.HighlightColor = "color-light-blue";
                    continue;
                }

                // parameter type identifier
                if (ChartNavigator.IsParameterType(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
                    continue;
                }

                // field identifier
                if (ChartNavigator.IsField(token))
                {
                    token.HighlightColor = "color-white";
                    continue;
                }

                // property identifier
                if (ChartNavigator.IsProperty(token))
                {
                    token.HighlightColor = "color-white";
                    continue;
                }

                // namespace delclaration identifier
                if (ChartNavigator.IsNamespaceDeclaration(token))
                {
                    token.HighlightColor = "color-white";
                    continue;
                }

                // type argument identifiers
                if (ChartNavigator.IsTypeArgument(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
                    continue;
                }

                // base types
                if (ChartNavigator.IsBaseType(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
                    continue;
                }

                // expressions
                if (ChartNavigator.IsExpression(token))
                {
                    token.HighlightColor = "color-yellow";
                    continue;
                }

                // data types
                if (ChartNavigator.IsDataType(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
                    continue;
                }

                // attributes
                if (ChartNavigator.IsAttribute(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
                    continue;
                }

                // name colons
                if (ChartNavigator.IsNameColon(token))
                {
                    token.HighlightColor = "color-light-blue";
                    continue;
                }
            }
        }

        private static void AddIdentifierReferenceHighlighting(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                // parameter refs
                if (ChartNavigator.IsParameter(token))
                {
                    UpdateParameterReferences(navTokens, token.Index);
                }

                // variable refs
                if (ChartNavigator.IsVariableDeclaration(token))
                {
                    UpdateVariableReferences(navTokens, token.Index);
                }

                // foreach var refs
                if (ChartNavigator.IsForEachVariable(token))
                {
                    token.HighlightColor = "color-light-blue";
                    UpdateForEachVariableReferences(navTokens, token.Index);
                }
            }
        }

        private static void UpdateVariableReferences(List<NavToken> tokens, int startIndex)
        {
            var parameterName = tokens[startIndex].Text;
            int zeroCount = 1;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == parameterName && zeroCount > 0)
                {
                    tokens[i].HighlightColor = "color-light-blue";
                }
                if (tokens[i].Text == "{")
                {
                    zeroCount++;
                }
                if (tokens[i].Text == "}")
                {
                    zeroCount--;
                }
                if (zeroCount is 0)
                {
                    break;
                }
            }
        }

        private static void UpdateForEachVariableReferences(List<NavToken> tokens, int startIndex)
        {
            var parameterName = tokens[startIndex].Text;
            int zeroCount = 0;
            bool blockOpened = false;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == parameterName && zeroCount > 0)
                {
                    tokens[i].HighlightColor = "color-light-blue";
                }
                if (tokens[i].Text == "{")
                {
                    blockOpened = true;
                    zeroCount++;
                }
                if (tokens[i].Text == "}")
                {
                    zeroCount--;
                }
                if (blockOpened && zeroCount is 0)
                {
                    break;
                }
            }
        }

        private static void UpdateParameterReferences(List<NavToken> tokens, int startIndex)
        {
            var parameterName = tokens[startIndex].Text;
            int zeroCount = 0;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                // interface param - nothing to hightlight
                if (tokens[i].Text == ";" && zeroCount == 0)
                {
                    break;
                }

                // expression-bodied method
                if (tokens[i].Text == "=>" && zeroCount == 0)
                {
                    UpdateExpressionBodiedParameterRefs(tokens, parameterName, i);
                    break;
                }

                // block-bodied method
                if (tokens[i].Text == "{" && zeroCount == 0)
                {
                    UpdateBlockBodiedParameterRefs(tokens, parameterName, i);
                    break;
                }
            }
        }

        private static void UpdateExpressionBodiedParameterRefs(List<NavToken> tokens, string parameterName, int startIndex)
        {
            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == parameterName)
                {
                    tokens[i].HighlightColor = "color-light-blue";
                }
                if (tokens[i].Text == ";")
                {
                    break;
                }
            }
        }

        private static void UpdateBlockBodiedParameterRefs(List<NavToken> tokens, string parameterName, int startIndex)
        {
            var zeroCount = 1;
            var blockClosed = false;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == "{")
                {
                    zeroCount++;
                }
                if (tokens[i].Text == "}")
                {
                    zeroCount--;
                    if (zeroCount == 0)
                    {
                        blockClosed = true;
                    }
                }
                if (tokens[i].Text == parameterName && !blockClosed)
                {
                    tokens[i].HighlightColor = "color-light-blue";
                }
            }

        }

        private static bool HighlightColorAlreadySet(NavToken token)
        {
            if (!string.IsNullOrEmpty(token.HighlightColor))
            {
                return true;
            }
            return false;
        }

        // TODO: Move to helper, dupped in tokentag wizard
        private static string GetClassOrInterfaceColor(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 2)
            {
                return "color-red";
            }

            if (char.IsUpper(text[0])
                && char.IsUpper(text[1])
                && text.StartsWith('I'))
            {
                return "color-light-green";
            }

            return "color-green";
        }

        private static void AddHighlightingThatNeedsSurroundingTokens(List<NavToken> navTokens)
        {

            for (int i = 0; i < navTokens.Count; i++)
            {
                var token = navTokens[i];

                if (HighlightColorAlreadySet(token))
                {
                    continue;
                }

                if (!ChartNavigator.IsInvocation(token))
                {
                    //token.HighlightColor = "color-red"; // unhighlighted tokens
                    continue;
                }

                var nextToken = navTokens[i + 1];

                // method invocations
                if (nextToken.Text == "(")
                {
                    token.HighlightColor = "color-yellow";
                }

                // static class invocations
                if (nextToken.Text == "." && token.SymbolKind != "Field")
                {
                    token.HighlightColor = "color-green";
                }
            }
        }
    }
}
