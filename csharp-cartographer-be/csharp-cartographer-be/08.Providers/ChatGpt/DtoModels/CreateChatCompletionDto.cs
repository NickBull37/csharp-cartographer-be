namespace csharp_cartographer_be._08.Providers.ChatGpt.DtoModels
{
    public record CreateChatCompletionDto
    {
        public string Model { get; init; } = "gpt-4o-mini";

        public decimal Temperature { get; init; } = 0.7m;

        public List<Message> Messages { get; init; } = [];
    }
}
