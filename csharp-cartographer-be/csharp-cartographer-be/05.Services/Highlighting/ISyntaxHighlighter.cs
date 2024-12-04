using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Highlighting
{
    public interface ISyntaxHighlighter
    {
        void AddSyntaxHighlightingToNavTokens(List<NavToken> navTokens);
    }
}
