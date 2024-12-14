namespace csharp_cartographer_be._01.Configuration.CartographerConfig
{
    public class CartographerConfig
    {
        /// <summary>
        /// A boolean flag that colors all unhighlighted tokens red when enabled.
        /// </summary>
        public bool ShowUnhighlightedTokens { get; set; }

        public string ChatGptUrl { get; set; }

        public string ChatGptPrompt { get; set; }
    }
}
