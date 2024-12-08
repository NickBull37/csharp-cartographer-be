using csharp_cartographer_be._03.Models.Artifacts;
using csharp_cartographer_be._07.Controllers.Artifacts.DtoModels;

namespace csharp_cartographer_be._06.Workflows.Artifacts
{
    public interface IGenerateArtifactWorkflow
    {
        Artifact ExecGenerateDemoArtifact(string fileName);

        Artifact ExecGenerateArtifact(GenerateArtifactDto requestDto);
    }
}
