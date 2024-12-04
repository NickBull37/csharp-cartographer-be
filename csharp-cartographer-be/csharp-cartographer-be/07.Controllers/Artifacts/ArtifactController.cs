using csharp_cartographer_be._06.Workflows.Artifacts;
using Microsoft.AspNetCore.Mvc;

namespace csharp_cartographer_be._07.Controllers.Artifacts
{
    [ApiController]
    [Route("[controller]")]
    public class ArtifactController : ControllerBase
    {
        private readonly IGenerateArtifactWorkflow _generateArtifactWorkflow;

        public ArtifactController(IGenerateArtifactWorkflow generateArtifactWorkflow)
        {
            _generateArtifactWorkflow = generateArtifactWorkflow;
        }

        #region GET Endpoints
        /// <summary>Generates and returns the requested demo artifact.</summary>
        [HttpGet]
        [Route("get-demo-artifact")]
        public async Task<IActionResult> GetDemoArtifact([FromQuery] string fileName)
        {
            try
            {
                var artifact = _generateArtifactWorkflow.ExecGenerateDemoArtifact(fileName);
                return Ok(artifact);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: 500, title: ex.Message);
            }
        }
        #endregion
    }
}
