using csharp_cartographer_be._05.Services.Charts;
using csharp_cartographer_be._05.Services.Files;
using csharp_cartographer_be._05.Services.Highlighting;
using csharp_cartographer_be._05.Services.Tags;
using csharp_cartographer_be._05.Services.Tokens;
using csharp_cartographer_be._06.Workflows.Artifacts;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 62914560; // 30 MB limit (in bytes)
});

// configure DI for csharp-cartographer services
builder.Services.AddScoped<IFileProcessor, FileProcessor>();
builder.Services.AddScoped<INavTokenGenerator, NavTokenGenerator>();
builder.Services.AddScoped<ISyntaxHighlighter, SyntaxHighlighter>();
builder.Services.AddScoped<ITokenChartGenerator, TokenChartGenerator>();
builder.Services.AddScoped<ITokenChartWizard, TokenChartWizard>();
builder.Services.AddScoped<ITokenTagGenerator, TokenTagGenerator>();

// configure DI for csharp-cartographer workflows
builder.Services.AddScoped<IGenerateArtifactWorkflow, GenerateArtifactWorkflow>();

var app = builder.Build();

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
