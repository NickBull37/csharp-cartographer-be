using csharp_cartographer_be._01.Configuration.CartographerConfig;
using csharp_cartographer_be._01.Configuration.Enums;
using csharp_cartographer_be._01.Configuration.ReservedText;
using csharp_cartographer_be._02.Utilities.Charts;
using csharp_cartographer_be._03.Models.Tokens;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Options;

namespace csharp_cartographer_be._05.Services.Highlighting
{
    public class SyntaxHighlighter : ISyntaxHighlighter
    {
        private readonly CartographerConfig _config;

        public SyntaxHighlighter(IOptions<CartographerConfig> config)
        {
            _config = config.Value;
        }

        public void AddSyntaxHighlightingToNavTokens(List<NavToken> navTokens)
        {
            AddReservedTextHighlighting(navTokens);
            AddLiteralHighlighting(navTokens);
            AddIdentifierHighlighting(navTokens);
            AddIdentifierReferenceHighlighting(navTokens);
            AddHighlightingThatNeedsSurroundingTokens(navTokens);

            if (_config.ShowUnhighlightedTokens)
            {
                HighlightRemainingTokens(navTokens);
            }
        }

        private static void AddReservedTextHighlighting(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                if (HighlightColorAlreadySet(token))
                {
                    continue;
                }

                // default keyword can be blue or purple
                if (token.Text == "default")
                {
                    token.HighlightColor = GetDefaultKeywordColor(token);
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

                // numeric literals (integer, decimal, float)
                if (token.Kind == SyntaxKind.NumericLiteralToken)
                {
                    token.HighlightColor = "color-light-green";
                    continue;
                }
                // string & char literals
                if (token.Kind == SyntaxKind.StringLiteralToken
                    || token.Kind == SyntaxKind.CharacterLiteralToken)
                {
                    token.HighlightColor = "color-orange";
                    continue;
                }
                // interpolated strings
                if (ChartNavigator.IsInterpolatedStringStart(token)
                    || ChartNavigator.IsInterpolatedStringEnd(token)
                    || ChartNavigator.IsInterpolatedStringText(token))
                {
                    token.HighlightColor = "color-orange";
                    continue;
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

                // interface delclaration identifier
                if (ChartNavigator.IsInterfaceDeclaration(token))
                {
                    token.HighlightColor = "color-light-green";
                    continue;
                }

                // namespace delclaration identifier
                // using directive identifier
                // field identifier
                // property
                // property access
                if (ChartNavigator.IsNamespaceDeclaration(token)
                    || ChartNavigator.IsUsingDirective(token)
                    || ChartNavigator.IsField(token)
                    || ChartNavigator.IsProperty(token)
                    || ChartNavigator.IsPropertyAccess(token))
                {
                    token.HighlightColor = "color-white";
                    continue;
                }

                // constructor declaration identifier
                // class delclaration identifier
                // exceptions
                // declaration pattern
                // object creation identifiers
                // record declaration
                // catch declarations
                if (ChartNavigator.IsConstructorDeclaration(token)
                    || ChartNavigator.IsClassDeclaration(token)
                    || ChartNavigator.IsException(token)
                    || ChartNavigator.IsDeclarationPattern(token)
                    || ChartNavigator.IsObjectCreationIdentifier(token)
                    || ChartNavigator.IsRecordDeclaration(token)
                    || ChartNavigator.IsCatchDeclaration(token))
                {
                    token.HighlightColor = "color-green";
                    continue;
                }

                // method delclarations
                // expressions
                if (ChartNavigator.IsMethodDeclaration(token)
                    || ChartNavigator.IsExpression(token))
                {
                    token.HighlightColor = "color-yellow";
                    continue;
                }

                // variable delclarations
                // parameter identifier
                // exception identifiers
                // name colons
                // single var designations
                // for loop identifier
                if (ChartNavigator.IsVariableDeclaration(token)
                    || ChartNavigator.IsParameter(token)
                    || ChartNavigator.IsExceptionIdentifier(token)
                    || ChartNavigator.IsNameColon(token)
                    || ChartNavigator.IsSingleVarDesignation(token)
                    || ChartNavigator.IsForLoopIdentifier(token))
                {
                    token.HighlightColor = "color-light-blue";
                    continue;
                }

                // parameter type identifier
                // type argument identifiers
                // base types
                // data types
                // attributes
                if (ChartNavigator.IsParameterType(token)
                    || ChartNavigator.IsTypeArgument(token)
                    || ChartNavigator.IsBaseType(token)
                    || ChartNavigator.IsDataType(token)
                    || ChartNavigator.IsAttribute(token))
                {
                    token.HighlightColor = GetClassOrInterfaceColor(token.Text);
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
                    UpdateParameterIdentifierReferences(navTokens, token.Index);
                }

                // variable refs
                if (ChartNavigator.IsVariableDeclaration(token))
                {
                    UpdateRefsInContainingBlock(navTokens, token.Text, token.Index, "color-light-blue");
                }

                // foreach var refs
                if (ChartNavigator.IsForEachVariable(token))
                {
                    token.HighlightColor = "color-light-blue";
                    UpdateRefsInNextBlock(navTokens, token.Text, token.Index, "color-light-blue");
                }

                // exception identifier refs
                if (ChartNavigator.IsExceptionIdentifier(token))
                {
                    UpdateRefsInNextBlock(navTokens, token.Text, token.Index, "color-light-blue");
                }

                // single var designation refs
                if (ChartNavigator.IsSingleVarDesignation(token))
                {
                    UpdateRefsInContainingBlock(navTokens, token.Text, token.Index, "color-light-blue");
                }

                // for loop identifier refs
                if (ChartNavigator.IsForLoopIdentifier(token))
                {
                    UpdateForLoopIdentifierReferences(navTokens, token.Index);
                }
            }
        }

        private static void UpdateParameterIdentifierReferences(List<NavToken> tokens, int startIndex)
        {
            var parameterName = tokens[startIndex].Text;
            var bodyType = GetBodyStructure(tokens, startIndex);

            if (bodyType == BodyStructure.NoBody)  // interface
            {
                return;
            }
            if (bodyType == BodyStructure.BlockBody)
            {
                UpdateRefsInNextBlock(tokens, parameterName, startIndex, "color-light-blue");
            }
            else if (bodyType == BodyStructure.ExpressionBody)
            {
                UpdateRefsInExpressionBody(tokens, parameterName, startIndex, "color-light-blue");
            }
        }

        private static void UpdateForLoopIdentifierReferences(List<NavToken> tokens, int startIndex)
        {
            var identifierName = tokens[startIndex].Text;
            UpdateForLoopControlRefs(tokens, identifierName, startIndex, "color-light-blue");
            UpdateRefsInNextBlock(tokens, identifierName, startIndex, "color-light-blue");
        }

        private static void UpdateRefsInExpressionBody(List<NavToken> tokens, string reference, int startIndex, string color)
        {
            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == ";")
                {
                    break;
                }
                if (tokens[i].Text == reference)
                {
                    tokens[i].HighlightColor = color;
                }
            }
        }

        private static void UpdateRefsInNextBlock(List<NavToken> tokens, string reference, int startIndex, string color)
        {
            int blockDepth = 0;
            bool blockOpened = false;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == "{")
                {
                    blockDepth++;
                    blockOpened = true;
                }
                if (tokens[i].Text == "}")
                {
                    blockDepth--;
                    if (blockOpened && blockDepth == 0)
                    {
                        break;
                    }
                }
                if (blockOpened && blockDepth > 0 && tokens[i].Text == reference)
                {
                    tokens[i].HighlightColor = color;
                }
            }
        }

        private static void UpdateRefsInContainingBlock(List<NavToken> tokens, string reference, int startIndex, string color)
        {
            int blockDepth = 1;
            bool blockOpened = true;

            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == "{")
                {
                    blockDepth++;
                }
                if (tokens[i].Text == "}")
                {
                    blockDepth--;
                    if (blockOpened && blockDepth == 0)
                    {
                        break;
                    }
                }
                if (blockOpened && blockDepth > 0 && tokens[i].Text == reference)
                {
                    tokens[i].HighlightColor = color;
                }
            }
        }

        private static void UpdateForLoopControlRefs(List<NavToken> tokens, string reference, int startIndex, string color)
        {
            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == ")")
                {
                    break;
                }
                if (tokens[i].Text == reference)
                {
                    tokens[i].HighlightColor = color;
                }
            }
        }

        private static BodyStructure GetBodyStructure(List<NavToken> tokens, int startIndex)
        {
            for (int i = startIndex + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Text == ";")
                {
                    return BodyStructure.NoBody;
                }
                if (tokens[i].Text == "=>")
                {
                    return BodyStructure.ExpressionBody;
                }
                if (tokens[i].Text == "{")
                {
                    return BodyStructure.BlockBody;
                }
            }
            throw new Exception();
        }

        private static string GetDefaultKeywordColor(NavToken token) =>
            token.Text == "default" && token.Tags[1].Label == "DefaultSwitchLabel"
                ? "color-purple"
                : "color-blue";

        private static bool HighlightColorAlreadySet(NavToken token) =>
            !string.IsNullOrEmpty(token.HighlightColor);

        private static string GetClassOrInterfaceColor(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < 2)
            {
                return "color-red"; // color red for unidentified tokens
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
                bool onLastToken = token.Index == navTokens.Count - 1;
                var nextToken = onLastToken ? null : navTokens[i + 1];

                if (HighlightColorAlreadySet(token))
                {
                    continue;
                }

                // supposed to be for enum & static class member access
                if (nextToken != null
                    && ChartNavigator.IsMemberAccess(token)
                    && !ChartNavigator.IsInvocation(token)
                    && nextToken.Text == "."
                    && !token.Text.StartsWith('_'))
                {
                    token.HighlightColor = "color-green";
                }

                if (!ChartNavigator.IsInvocation(token))
                {
                    continue;
                }

                // method invocations
                if (nextToken.Text == "(")
                {
                    token.HighlightColor = "color-yellow";
                }

                // static class invocations
                if (nextToken.Text == "." && token.SemanticData?.SymbolKind != "Field")
                {
                    token.HighlightColor = "color-green";
                }
            }
        }

        private static void HighlightRemainingTokens(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                if (string.IsNullOrEmpty(token.HighlightColor))
                {
                    token.HighlightColor = "color-red";
                }
            }
        }
    }
}
