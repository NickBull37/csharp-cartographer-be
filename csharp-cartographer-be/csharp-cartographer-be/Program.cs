using csharp_cartographer_be._01.Configuration.CartographerConfig;
using csharp_cartographer_be._03.Models;
using csharp_cartographer_be._05.Services.Charts;
using csharp_cartographer_be._05.Services.Files;
using csharp_cartographer_be._05.Services.Highlighting;
using csharp_cartographer_be._05.Services.Tags;
using csharp_cartographer_be._05.Services.Tokens;
using csharp_cartographer_be._06.Workflows.Artifacts;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure strongly typed settings object
builder.Services.Configure<CartographerConfig>(builder.Configuration.GetSection("CartographerConfig"));

// Set custom file size limit for incoming requests
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100000; // 100 KB limit (in bytes)
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

// configure DI for csharp-cartographer DbContext
builder.Services.AddDbContext<CartographerDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("CartographerDbContext")));

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
