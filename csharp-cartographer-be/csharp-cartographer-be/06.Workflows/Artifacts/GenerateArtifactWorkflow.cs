using csharp_cartographer_be._02.Utilities.Logging;
using csharp_cartographer_be._03.Models.Artifacts;
using csharp_cartographer_be._03.Models.Files;
using csharp_cartographer_be._05.Services.Charts;
using csharp_cartographer_be._05.Services.Files;
using csharp_cartographer_be._05.Services.Highlighting;
using csharp_cartographer_be._05.Services.Tags;
using csharp_cartographer_be._05.Services.Tokens;
using csharp_cartographer_be._07.Controllers.Artifacts.DtoModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace csharp_cartographer_be._06.Workflows.Artifacts
{
    public class GenerateArtifactWorkflow : IGenerateArtifactWorkflow
    {
        private readonly IFileProcessor _fileProcessor;
        private readonly INavTokenGenerator _navTokenGenerator;
        private readonly ISyntaxHighlighter _syntaxHighlighter;
        private readonly ITokenChartGenerator _tokenChartGenerator;
        private readonly ITokenChartWizard _tokenChartWizard;
        private readonly ITokenTagGenerator _tokenTagGenerator;

        public GenerateArtifactWorkflow(
            IFileProcessor fileProcessor,
            INavTokenGenerator navTokenGenerator,
            ISyntaxHighlighter syntaxHighlighter,
            ITokenChartGenerator tokenChartGenerator,
            ITokenChartWizard tokenChartWizard,
            ITokenTagGenerator tokenTagGenerator)
        {
            _fileProcessor = fileProcessor;
            _navTokenGenerator = navTokenGenerator;
            _syntaxHighlighter = syntaxHighlighter;
            _tokenChartGenerator = tokenChartGenerator;
            _tokenChartWizard = tokenChartWizard;
            _tokenTagGenerator = tokenTagGenerator;
        }

        public Artifact ExecGenerateDemoArtifact(string fileName)
        {
            /*
             *   Steps to generate an artifact:
             * 
             *   1. Read in source code from demo file path & generate FileData.
             *   2. Generate SyntaxTree with FileData.
             *   3. Generate CompilationUnit with SyntaxTree.
             *   4. Generate SemanticModel with CompilationUnit & SyntaxTree.
             *   5. Use SyntaxTree & SemanticModel to generate NavTokens for the demo artifact.
             *   6. Generate TokenTags.
             *   7. Generate TokenCharts.
             *   8. Add TokenCharts highlight indices for highlighting in the code viewer.
             *   9. Add TokenTags definitions & insights.
             *   10. Add syntax highlighting for all NavTokens (should be last step in workflow).
             *   11. Build artifact.
             *   12. Return artifact.
             *   
             */

            // 1. Read in source code from demo file path & generate FileData.
            FileData fileData = _fileProcessor.ReadInTestFileData(fileName);

            // 2. Generate SyntaxTree with FileData.
            var syntaxTree = CSharpSyntaxTree.ParseText(fileData.Content);

            // 3. Generate CompilationUnit with SyntaxTree.
            var compilationUnit = CSharpCompilation.Create("ArtifactCompilation")
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            // 4. Generate SemanticModel with CompilationUnit & SyntaxTree.
            var semanticModel = compilationUnit.GetSemanticModel(syntaxTree);

            // 5. Use SyntaxTree & SemanticModel to generate NavTokens for the demo artifact.
            var navTokens = _navTokenGenerator.GenerateNavTokens(semanticModel, syntaxTree);

            // 6. Generate TokenTags.
            _tokenTagGenerator.GenerateTokenTags(navTokens);

            // 7. Generate TokenCharts.
            _tokenChartGenerator.GenerateTokenCharts(navTokens);

            // 8. Add TokenCharts highlight indices for highlighting in the code viewer.
            _tokenChartWizard.AddHighlightValuesToNavTokenCharts(navTokens);

            // 9. Add TokenCharts definitions & insights.
            _tokenChartWizard.AddFactsAndInsightsToNavTokenCharts(navTokens);

            // 10. Add syntax highlighting for all NavTokens (should be last step in workflow).
            _syntaxHighlighter.AddSyntaxHighlightingToNavTokens(navTokens);

            TokenLogger.LogTokenList(navTokens);

            // 11. Build artifact.
            Artifact artifact = new()
            {
                CreatedDate = DateTime.Now,
                ArtifactType = "Model Class",
                Title = fileData.FileName,
                NavTokens = navTokens,
            };

            // 12. Return artifact.
            return artifact;
        }

        public Artifact ExecGenerateArtifact(GenerateArtifactDto requestDto)
        {
            // 1. Read in source code from demo file path & generate FileData.
            FileData fileData = _fileProcessor.ReadInFileData(requestDto);

            // 2. Generate SyntaxTree with FileData.
            var syntaxTree = CSharpSyntaxTree.ParseText(fileData.Content);

            // 3. Generate CompilationUnit with SyntaxTree.
            var compilationUnit = CSharpCompilation.Create("ArtifactCompilation")
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            // 4. Generate SemanticModel with CompilationUnit & SyntaxTree.
            var semanticModel = compilationUnit.GetSemanticModel(syntaxTree);

            // 5. Use SyntaxTree & SemanticModel to generate NavTokens for the demo artifact.
            var navTokens = _navTokenGenerator.GenerateNavTokens(semanticModel, syntaxTree);

            // 6. Generate TokenTags.
            _tokenTagGenerator.GenerateTokenTags(navTokens);

            // 7. Generate TokenCharts.
            _tokenChartGenerator.GenerateTokenCharts(navTokens);

            // 8. Add TokenCharts highlight indices for highlighting in the code viewer.
            _tokenChartWizard.AddHighlightValuesToNavTokenCharts(navTokens);

            // 9. Add TokenCharts definitions & insights.
            _tokenChartWizard.AddFactsAndInsightsToNavTokenCharts(navTokens);

            // 10. Add syntax highlighting for all NavTokens (should be last step in workflow).
            _syntaxHighlighter.AddSyntaxHighlightingToNavTokens(navTokens);

            TokenLogger.LogTokenList(navTokens);

            // 11. Build artifact.
            Artifact artifact = new()
            {
                CreatedDate = DateTime.Now,
                ArtifactType = "Model Class",
                Title = fileData.FileName,
                NavTokens = navTokens,
            };

            // 12. Return artifact.
            return artifact;
        }
    }
}
