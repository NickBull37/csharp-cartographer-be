using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Tags
{
    public interface ITokenTagGenerator
    {
        void GenerateTokenTags(List<NavToken> navTokens);
    }
}
