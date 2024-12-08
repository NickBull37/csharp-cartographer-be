using csharp_cartographer_be._02.Utilities.ActionResponse;
using csharp_cartographer_be._03.Models.Artifacts;

namespace csharp_cartographer_be._04.DataAccess.Artifacts
{
    public interface IArtifactRepository
    {
        Task<ActionResponse> CreateArtifactDatabaseRecord(Artifact artifact);
    }
}
