﻿using csharp_cartographer_be._02.Utilities.Logging;
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
using System.Diagnostics;

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
            // Step 0. Read in source code from demo file path & generate FileData.
            FileData fileData = _fileProcessor.ReadInTestFileData(fileName);
            return GenerateArtifact(fileData);
        }

        public Artifact ExecGenerateUserArtifact(GenerateArtifactDto requestDto)
        {
            // Step 0. Read in source code from user uploaded file & generate FileData.
            FileData fileData = _fileProcessor.ReadInFileData(requestDto);
            return GenerateArtifact(fileData);
        }

        private Artifact GenerateArtifact(FileData fileData)
        {
            /*
             *   Steps to generate an artifact:
             * 
             *   1. Start stopwatch.
             *   2. Generate SyntaxTree with passed in FileData.
             *   3. Generate CompilationUnit with SyntaxTree.
             *   4. Generate SemanticModel with CompilationUnit & SyntaxTree.
             *   5. Use SyntaxTree & SemanticModel to generate NavTokens for the artifact.
             *   6. Generate TokenTags.
             *   7. Generate TokenCharts.
             *   8. Add TokenCharts highlight indices for highlighting in the code viewer.
             *   9. Add TokenTags definitions & insights.
             *   10. Add syntax highlighting for all NavTokens (should be last step in workflow).
             *   11. Stop stopwatch.
             *   12. Build & return artifact.
             *   
             */

            // Step 1. Start stopwatch.
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Step 2. Generate SyntaxTree with passed in FileData.
            var syntaxTree = CSharpSyntaxTree.ParseText(fileData.Content);

            // Step 3. Generate CompilationUnit with SyntaxTree.
            var compilationUnit = CSharpCompilation.Create("ArtifactCompilation")
                .AddSyntaxTrees(syntaxTree)
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

            // Step 4. Generate SemanticModel with CompilationUnit & SyntaxTree.
            var semanticModel = compilationUnit.GetSemanticModel(syntaxTree);

            // Step 5. Use SyntaxTree & SemanticModel to generate NavTokens for the artifact.
            var navTokens = _navTokenGenerator.GenerateNavTokens(semanticModel, syntaxTree);

            // Step 6. Generate TokenTags.
            _tokenTagGenerator.GenerateTokenTags(navTokens);

            // Step 7. Generate TokenCharts.
            _tokenChartGenerator.GenerateTokenCharts(navTokens);

            // Step 8. Add TokenCharts highlight indices for highlighting in the code viewer.
            _tokenChartWizard.AddHighlightValuesToNavTokenCharts(navTokens);

            // Step 9. Add TokenCharts definitions & insights.
            _tokenChartWizard.AddFactsAndInsightsToNavTokenCharts(navTokens);

            // Step 10. Add syntax highlighting for all NavTokens (should be last step in workflow).
            _syntaxHighlighter.AddSyntaxHighlightingToNavTokens(navTokens);

            TokenLogger.LogTokenList(navTokens);

            // Step 11. Stop stopwatch.
            stopwatch.Stop();

            // Step 12. Build & return artifact.
            return new Artifact(fileData.FileName, stopwatch.Elapsed, navTokens);
        }
    }
}
