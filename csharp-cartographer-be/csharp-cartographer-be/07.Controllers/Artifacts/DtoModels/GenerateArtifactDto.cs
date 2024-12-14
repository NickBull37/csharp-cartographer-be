using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace csharp_cartographer_be._07.Controllers.Artifacts.DtoModels
{
    public record GenerateArtifactDto
    {
        public Guid ArtifactId { get; init; } = Guid.NewGuid();

        [Required]
        [StringLength(100, ErrorMessage = "File name cannot exceed 100 characters.")]
        [JsonPropertyName("fileName")]
        public string FileName { get; init; } = default!;

        [Required]
        [JsonPropertyName("fileContent")]
        public string FileContent { get; init; } = default!;
    }
}
