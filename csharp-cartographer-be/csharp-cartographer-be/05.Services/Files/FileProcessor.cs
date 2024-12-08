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

        public FileProcessor()
        {
        }

        public FileData ReadInTestFileData(string fileName)
        {
            string testFile = string.Empty;

            if (fileName == "Animal.cs")
            {
                testFile = _modelDefinitionDemoFilePath;
            }
            else if (fileName == "RoslynAnalyzer.cs")
            {
                testFile = _workflowDemoFilePath;
            }
            else if (fileName == "SyntaxHighlighter.cs")
            {
                testFile = _serviceDemoFilePath;
            }
            else if (fileName == "ArtifactRepository.cs")
            {
                testFile = _repositoryDemoFilePath;
            }
            else if (fileName == "ArtifactController.cs")
            {
                testFile = _controllerDemoFilePath;
            }
            else if (fileName == "StringHelpers.cs")
            {
                testFile = _helperClassDemoFilePath;
            }

            if (!string.IsNullOrEmpty(testFile) && !File.Exists(testFile))
            {
                Console.WriteLine("File does not exist.");
                throw new FileNotFoundException();
            }

            var fileLines = new List<string>();
            using (StreamReader sr = new(testFile))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    fileLines.Add(line);
                }
            }

            return new FileData
            {
                FileName = Path.GetFileName(testFile),
                FileLines = fileLines,
                Content = File.ReadAllText(testFile)
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
