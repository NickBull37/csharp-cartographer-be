using csharp_cartographer_be._03.Models.Files;

namespace csharp_cartographer_be._05.Services.Files
{
    public class FileProcessor : IFileProcessor
    {
        private readonly string _modelDefinitionDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\03.Models\Tokens\NavToken.cs";
        private readonly string _workflowDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\06.Workflows\Artifacts\GenerateArtifactWorkflow.cs";
        private readonly string _serviceDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\05.Services\Highlighting\SyntaxHighlighter.cs";
        private readonly string _repositoryDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\04.DataAccess\Artifacts\ArtifactRepository.cs";
        private readonly string _controllerDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\07.Controllers\Artifacts\ArtifactController.cs";
        private readonly string _helperClassDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\02.Utilities\Helpers\StringHelpers.cs";
        private readonly string _configDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\01.Configuration\CartographerConfig\CartographerConfig.cs";
        private readonly string _dtoDemoFilePath = @"C:\Users\nbuli\source\repos\csharp-cartographer-be\csharp-cartographer-be\csharp-cartographer-be\07.Controllers\Artifacts\Dtos\ArtifactDto.cs";

        public FileData ReadInTestFileData(string fileName)
        {
            var demoFile = fileName switch
            {
                "NavToken.cs" => _modelDefinitionDemoFilePath,
                "GenerateArtifactWorkflow.cs" => _workflowDemoFilePath,
                "SyntaxHighlighter.cs" => _serviceDemoFilePath,
                "ArtifactRepository.cs" => _repositoryDemoFilePath,
                "ArtifactController.cs" => _controllerDemoFilePath,
                "StringHelpers.cs" => _helperClassDemoFilePath,
                "CartographerConfig.cs" => _configDemoFilePath,
                "ArtifactDto.cs" => _dtoDemoFilePath,
                _ => string.Empty
            };

            var fileLines = new List<string>();
            using (StreamReader sr = new(demoFile))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    fileLines.Add(line);
                }
            }

            return new FileData
            {
                FileName = Path.GetFileName(demoFile),
                FileLines = fileLines,
                Content = File.ReadAllText(demoFile)
            };
        }

        public FileData ReadInFileData(string fileName, string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode))
            {
                Console.WriteLine("Source code is empty or null.");
                throw new ArgumentException("Source code cannot be null or empty.");
            }

            var fileLines = new List<string>();
            using (StringReader sr = new(sourceCode))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    fileLines.Add(line);
                }
            }

            return new FileData
            {
                FileName = fileName,
                FileLines = fileLines,
                Content = sourceCode
            };
        }
    }
}
