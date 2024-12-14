namespace csharp_cartographer_be._08.Providers.ChatGpt.DtoModels
{
    public record Message
    {
        public string Role { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;
    }
}
