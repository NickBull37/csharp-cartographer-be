using csharp_cartographer_be._01.Configuration.CSharpElements;
using csharp_cartographer_be._03.Models.Tokens;

namespace csharp_cartographer_be._05.Services.Tags
{
    public class TokenTagWizard : ITokenTagWizard
    {
        public void AddFactsAndInsightsToNavTokenTags(List<NavToken> navTokens)
        {
            foreach (var token in navTokens)
            {
                foreach (var tag in token.Tags)
                {
                    foreach (var element in CSharpElements.ElementList)
                    {
                        if (tag.Label.Equals(element.Label))
                        {
                            tag.Facts = element.Facts;
                            tag.Insights = element.Insights;
                        }
                    }
                }
            }
        }
    }
}
