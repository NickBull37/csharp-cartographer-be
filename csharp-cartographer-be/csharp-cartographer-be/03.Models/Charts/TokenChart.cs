using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace csharp_cartographer_be._03.Models.Charts
{
    public class TokenChart
    {
        public string Label { get; set; } = string.Empty;

        public int Level { get; set; }

        public List<string> Facts { get; set; } = [];

        public List<string> Insights { get; set; } = [];

        public List<int> HighlightIndices { get; set; } = [];

        [JsonIgnore]
        public List<SyntaxToken> Tokens { get; set; } = [];
    }
}
