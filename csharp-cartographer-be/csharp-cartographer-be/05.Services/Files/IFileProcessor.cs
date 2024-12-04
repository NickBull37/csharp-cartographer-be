using csharp_cartographer_be._03.Models.Files;

namespace csharp_cartographer_be._05.Services.Files
{
    public interface IFileProcessor
    {
        FileData ReadInTestFileData(string fileName);

        FileData ReadInFileData(string fileName, string sourceCode);
    }
}
