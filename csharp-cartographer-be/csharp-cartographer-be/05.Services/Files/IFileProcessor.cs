using csharp_cartographer_be._03.Models.Files;
using csharp_cartographer_be._07.Controllers.Artifacts.DtoModels;

namespace csharp_cartographer_be._05.Services.Files
{
    public interface IFileProcessor
    {
        FileData ReadInTestFileData(string fileName);

        FileData ReadInFileData(GenerateArtifactDto requestDto);
    }
}
