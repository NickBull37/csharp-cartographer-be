namespace csharp_cartographer_be._03.Models.Files
{
    public class FileData
    {
        public string FileName { get; set; } = string.Empty;

        public List<string> FileLines { get; set; } = [];

        public string Content { get; set; } = string.Empty;
    }
}
