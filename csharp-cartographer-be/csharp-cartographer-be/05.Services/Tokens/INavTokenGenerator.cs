using csharp_cartographer_be._03.Models.Tokens;
using Microsoft.CodeAnalysis;

namespace csharp_cartographer_be._05.Services.Tokens
{
    public interface INavTokenGenerator
    {
        List<NavToken> GenerateNavTokens(SemanticModel semanticModel, SyntaxTree syntaxTree);
    }
}
