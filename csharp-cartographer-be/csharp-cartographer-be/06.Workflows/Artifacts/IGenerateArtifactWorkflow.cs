using csharp_cartographer_be._03.Models.Artifacts;

namespace csharp_cartographer_be._06.Workflows.Artifacts
{
    public interface IGenerateArtifactWorkflow
    {
        Artifact ExecGenerateDemoArtifact(string fileName);

        Artifact ExecGenerateArtifact(string fileName);
    }
}
