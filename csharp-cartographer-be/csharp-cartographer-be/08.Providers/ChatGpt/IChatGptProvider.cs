namespace csharp_cartographer_be._08.Providers.ChatGpt
{
    public interface IChatGptProvider
    {
        Task<string> GetCodeAnalysis(string code, CancellationToken cancellationToken);
    }
}
