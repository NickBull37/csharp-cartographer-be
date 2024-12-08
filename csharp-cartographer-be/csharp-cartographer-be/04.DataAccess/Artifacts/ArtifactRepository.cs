using csharp_cartographer_be._02.Utilities.ActionResponse;
using csharp_cartographer_be._02.Utilities.Logging;
using csharp_cartographer_be._03.Models;
using csharp_cartographer_be._03.Models.Artifacts;

namespace csharp_cartographer_be._04.DataAccess.Artifacts
{
    public class ArtifactRepository : IArtifactRepository
    {
        private readonly CartographerDbContext _dbContext;

        public ArtifactRepository(CartographerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResponse> CreateArtifactDatabaseRecord(Artifact artifact)
        {
            try
            {
                _dbContext.Artifacts.Add(artifact);
                await _dbContext.SaveChangesAsync();
                return new PassingAR();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex, "ArtifactRepository - CreateArtifactDatabaseRecord()");
                return new FailingAR("Failure occured attempting to save artifact record to database.");
            }
        }
    }
}
