using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Tags
{
    public interface ITokenTagWizard
    {
        void AddFactsAndInsightsToNavTokenTags(List<NavToken> navTokens);
    }
}
