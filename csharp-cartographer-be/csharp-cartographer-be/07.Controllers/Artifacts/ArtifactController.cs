﻿using csharp_cartographer_be._06.Workflows.Artifacts;
using csharp_cartographer_be._07.Controllers.Artifacts.DtoModels;
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
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Problem(
                    type: "Bad Request",
                    title: "Invalid file name",
                    detail: "The file name must match one of the existing demo files available.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {
                var artifact = _generateArtifactWorkflow.ExecGenerateDemoArtifact(fileName);
                return Ok(artifact);
            }
            catch (Exception ex)
            {
                return Problem(
                    type: "Internal Server Error",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        [HttpPost]
        [Route("generate-artifact")]
        public async Task<IActionResult> GenerateArtifact([FromBody] GenerateArtifactDto dto)
        {
            try
            {
                var artifact = _generateArtifactWorkflow.ExecGenerateUserArtifact(dto);

                return Ok(artifact);
            }
            catch (Exception ex)
            {
                return Problem(
                    type: "Internal Server Error",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
