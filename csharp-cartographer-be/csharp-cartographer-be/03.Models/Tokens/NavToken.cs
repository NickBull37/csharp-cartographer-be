﻿using csharp_cartographer_be._02.Utilities.Helpers;
using csharp_cartographer_be._03.Models.Charts;
using csharp_cartographer_be._03.Models.Tags;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text.Json.Serialization;

namespace csharp_cartographer_be._03.Models.Tokens
{
    /// <summary>
    ///     A model definition for the NavToken class.
    /// </summary>
    public class NavToken
    {
        /// <summary>The unique identifier for a NavToken.</summary>
        public Guid ID { get; set; }

        /// <summary>The index value for the token in the list of NavTokens.</summary>
        public int Index { get; set; }

        /// <summary>The text string value of the token.</summary>
        public string Text { get; }

        /// <summary>The Roslyn SyntaxKind of the token.</summary>
        [JsonIgnore]
        public SyntaxKind Kind { get; set; }

        /// <summary>The Roslyn SyntaxKind of the token as a string.</summary>
        public string RoslynKind { get; set; }

        /// <summary>The TextSpan of the token text.</summary>
        [JsonIgnore]
        public TextSpan Span { get; set; }

        /// <summary>A list of the tokens leading trivia strings.</summary>
        public List<string> LeadingTrivia { get; set; } = [];

        /// <summary>A list of the tokens trailing trivia strings.</summary>
        public List<string> TrailingTrivia { get; set; } = [];

        /// <summary>The Roslyn generated SyntaxToken.</summary>
        [JsonIgnore]
        public SyntaxToken RoslynToken { get; set; }

        /// <summary>The Roslyn generated SyntaxNode of the token's parent.</summary>
        [JsonIgnore]
        public SyntaxNode? Parent { get; set; }

        /// <summary>The Roslyn generated SyntaxNode of the token's grand parent.</summary>
        [JsonIgnore]
        public SyntaxNode? GrandParent { get; set; }

        /// <summary>The Roslyn generated SyntaxNode of the token's great grand parent.</summary>
        [JsonIgnore]
        public SyntaxNode? GreatGrandParent { get; set; }

        /// <summary>The Roslyn generated SyntaxKind of the token's parent as a string.</summary>
        public string? ParentNodeKind { get; set; }

        /// <summary>The Roslyn generated SyntaxKind of the token's grand parent as a string.</summary>
        public string? GrandParentNodeKind { get; set; }

        /// <summary>The Roslyn generated SyntaxKind of the token's great grand parent as a string.</summary>
        public string? GreatGrandParentNodeKind { get; set; }

        /// <summary>The token's semantic data.</summary>
        [JsonIgnore]
        public SemanticData? SemanticData { get; set; }

        /// <summary>A list of references to this token in the source code file.</summary>
        public List<string> References { get; set; } = [];

        /// <summary>The color the token will be highlighted in the UI.</summary>
        public string? HighlightColor { get; set; }

        /// <summary>A list of token tags attached to the token.</summary>
        public List<TokenTag> Tags { get; set; } = [];

        /// <summary>A list of ancestor nodes & data attached to the token.</summary>
        public List<TokenChart> Charts { get; set; } = [];

        /// <summary>Constructor for NavToken model.</summary>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <param name="semanticModel">The semantic model generated from the source code.</param>
        /// <param name="syntaxTree">The syntax tree generated from the source code.</param>
        /// <param name="index">The index of the token in the list.</param>
        public NavToken(SyntaxToken roslynToken, SemanticModel semanticModel, SyntaxTree syntaxTree, int index)
        {
            ID = Guid.NewGuid();
            Index = index;

            #region Lexical (token) data
            Text = roslynToken.Text;
            Kind = roslynToken.Kind();
            RoslynKind = roslynToken.Kind().ToString();
            Span = roslynToken.Span;
            LeadingTrivia = GetLeadingTrivia(roslynToken);
            TrailingTrivia = GetTrailingTrivia(roslynToken);
            RoslynToken = roslynToken;
            Parent = GetAncestorNode(roslynToken, 1);
            GrandParent = GetAncestorNode(roslynToken, 2);
            GreatGrandParent = GetAncestorNode(roslynToken, 3);
            #endregion

            #region Syntax data
            ParentNodeKind = GetAncestorNodeKind(roslynToken, 1);
            GrandParentNodeKind = GetAncestorNodeKind(roslynToken, 2);
            GreatGrandParentNodeKind = GetAncestorNodeKind(roslynToken, 3);
            #endregion

            #region Semantic data
            SemanticData = GetSemanticData(semanticModel, roslynToken.Parent);
            #endregion

            #region Contextual data
            References = GetContextualData(semanticModel, syntaxTree, roslynToken);
            #endregion
        }

        /// <summary>Gets the token's leading trivia.</summary>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <returns>A list of leading trivia strings.</returns>
        private static List<string> GetLeadingTrivia(SyntaxToken roslynToken)
        {
            if (!roslynToken.HasLeadingTrivia)
            {
                return [];
            }

            List<string> leadingTriviaStrings = [];

            foreach (var trivia in roslynToken.LeadingTrivia)
            {
                if (trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                {
                    leadingTriviaStrings.AddRange(GetLeadingSingleLineDocumentationCommentTrivia(trivia.ToString()));
                    continue;
                }
                else if (trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
                {
                    leadingTriviaStrings.AddRange(GetLeadingMultilineCommentTrivia(trivia.ToString()));
                    continue;
                }

                leadingTriviaStrings.Add(trivia.ToString());

                if (trivia.IsKind(SyntaxKind.RegionDirectiveTrivia)
                    || trivia.IsKind(SyntaxKind.EndRegionDirectiveTrivia))
                {
                    leadingTriviaStrings.Add(SyntaxFactory.EndOfLine("\r\n").ToString());
                }
            }

            return leadingTriviaStrings;
        }

        /// <summary>Splits a SingleLineDocumentationCommentTrivia trivia into multiple strings.</summary>
        /// <param name="triviaString">The trivia string that needs to be split.</param>
        /// <returns>A leading single-line documentation comment trivia split into a list of strings.</returns>
        private static List<string> GetLeadingSingleLineDocumentationCommentTrivia(string triviaString)
        {
            List<string> triviaToAdd = [];
            triviaString = "///" + triviaString;

            if (StringHelpers.CountOccurrences(triviaString, "///") == 1)
            {
                triviaToAdd.Add(triviaString);
                triviaToAdd.Add(SyntaxFactory.EndOfLine("\r\n").ToString());
            }
            if (StringHelpers.CountOccurrences(triviaString, "///") > 1)
            {
                var newStrings = triviaString.Split("\r\n");

                var count = 1;
                var numOfStrings = newStrings.Length;
                foreach (var newString in newStrings)
                {
                    // handle scenarios where comments have extra spaces
                    if (StringHelpers.HasSequentialSpaces(newString))
                    {
                        var spacesString = StringHelpers.PullSequentialSpaces(newString);
                        triviaToAdd.Add(spacesString);
                    }

                    triviaToAdd.Add(newString.Trim());
                    if (count < numOfStrings)
                    {
                        // add additional line break trivia for multi-line comments
                        triviaToAdd.Add(SyntaxFactory.EndOfLine("\r\n").ToString());
                    }
                    count++;
                }
            }
            return triviaToAdd;
        }

        /// <summary>Splits a MultilineCommentTrivia trivia into multiple strings.</summary>
        /// <param name="triviaString">The trivia string that needs to be split.</param>
        /// <returns>A list of leading multi-line comment trivia strings.</returns>
        private static List<string> GetLeadingMultilineCommentTrivia(string triviaString)
        {
            List<string> triviaToAdd = [];
            var newStrings = triviaString.Split("\r\n");

            var count = 1;
            var numOfStrings = newStrings.Length;
            foreach (var newString in newStrings)
            {
                // check if string has sequential spaces
                // if so, cut them and create a new space trivia with them
                if (StringHelpers.HasSequentialSpaces(newString))
                {
                    var spacesString = StringHelpers.PullSequentialSpaces(newString);
                    triviaToAdd.Add(spacesString);
                }

                // add new trivia strings
                triviaToAdd.Add(newString.Trim());
                if (count < numOfStrings)
                {
                    triviaToAdd.Add(SyntaxFactory.EndOfLine("\r\n").ToString());
                }
                count++;
            }
            return triviaToAdd;
        }

        /// <summary>Gets the token's trailing trivia.</summary>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <returns>A list of trailing trivia strings.</returns>
        private static List<string> GetTrailingTrivia(SyntaxToken roslynToken)
        {
            if (!roslynToken.HasTrailingTrivia)
            {
                return [];
            }

            List<string> trailingTriviaStrings = [];
            foreach (var trivia in roslynToken.TrailingTrivia)
            {
                // handle trailing trivia that contains "\n" instead of "\r\n"
                if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                {
                    trailingTriviaStrings.Add(SyntaxFactory.EndOfLine("\r\n").ToString());
                    continue;
                }
                trailingTriviaStrings.Add(trivia.ToString());
            }
            return trailingTriviaStrings;
        }

        /// <summary>Returns the SyntaxKind of the ancestor node at the specified level.</summary>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <param name="level">The number of ancestors to climb in the syntax tree.</param>
        /// <returns>The ancestor node kind if found; otherwise, <c>null</c>.</returns>
        private static string? GetAncestorNodeKind(SyntaxToken roslynToken, int level)
        {
            var ancestor = GetAncestorNode(roslynToken, level);
            return ancestor?.Kind().ToString();
        }

        /// <summary>Returns the ancestor SyntaxNode at the specified level.</summary>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <param name="level">The number of ancestors to climb in the syntax tree.</param>
        /// <returns>The ancestor node if found; otherwise, <c>null</c>.</returns>
        private static SyntaxNode? GetAncestorNode(SyntaxToken roslynToken, int level)
        {
            SyntaxNode? currentNode = roslynToken.Parent;
            for (int i = 1; i < level && currentNode is not null; i++)
            {
                currentNode = currentNode.Parent;
            }
            return currentNode;
        }

        /// <summary>Gets data for NavToken properites related to C# semantics.</summary>
        /// <param name="semanticModel">The semantic model generated from the source code.</param>
        /// <param name="node">The node the semantic data is reffering to.</param>
        /// <returns>The semantic data for the passed in node.</returns>
        private static SemanticData? GetSemanticData(SemanticModel semanticModel, SyntaxNode? node)
        {
            if (node is null)
            {
                return null;
            }

            SemanticData semanticData = new();
            var symbolInfo = semanticModel.GetSymbolInfo(node);

            if (symbolInfo.Symbol != null)
            {
                semanticData.Symbol = symbolInfo.Symbol;
                semanticData.SymbolName = symbolInfo.Symbol.Name;
                semanticData.SymbolKind = symbolInfo.Symbol.Kind.ToString();
                semanticData.ContainingType = symbolInfo.Symbol.ContainingType?.ToString() ?? null;
                semanticData.ContainingNamespace = symbolInfo.Symbol.ContainingNamespace?.ToString() ?? null;
            }

            var typeInfo = semanticModel.GetTypeInfo(node);
            if (typeInfo.Type != null)
            {
                semanticData.TypeName = typeInfo.Type.Name;
                semanticData.TypeKind = typeInfo.Type.Kind.ToString();
                semanticData.IsNullable = typeInfo.Nullability.FlowState == NullableFlowState.MaybeNull;
            }

            return semanticData;
        }

        /// <summary>Gets the contextual data for the passed in syntax token.</summary>
        /// <param name="semanticModel">The semantic model generated from the source code.</param>
        /// <param name="syntaxTree">The syntax tree generated from the source code.</param>
        /// <param name="roslynToken">The SyntaxToken generated by the Roslyn code analysis library.</param>
        /// <returns>A list of symbol references for the passed in syntax token.</returns>
        private static List<string> GetContextualData(SemanticModel semanticModel, SyntaxTree syntaxTree, SyntaxToken roslynToken)
        {
            if (roslynToken.Parent is not IdentifierNameSyntax identifierNode)
            {
                return [];
            }

            var root = syntaxTree.GetRoot();
            var symbol = semanticModel.GetSymbolInfo(identifierNode).Symbol;

            if (symbol is null)
            {
                return [];
            }

            var identifierNodes = root.DescendantNodes().OfType<IdentifierNameSyntax>();

            // find all nodes where the symbol matches
            return identifierNodes
                .Where(node =>
                {
                    var nodeSymbol = semanticModel.GetSymbolInfo(node).Symbol;
                    return SymbolEqualityComparer.Default.Equals(nodeSymbol, symbol);
                })
                .Select(reference => reference.ToString())
                .ToList();
        }
    }
}
