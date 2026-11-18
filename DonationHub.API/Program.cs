using DonationHub.API;
using DonationHub.API.Extensions;
using DonationHub.API.Middleware;
using DonationHub.Application;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOptionsConfiguration(builder.Configuration);

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration, 
    builder.Environment);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp", policy =>
    {
        policy.WithOrigins("https://localhost:3000") 
              .AllowAnyMethod()
              .AllowAnyHeader();
              
    });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
   
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    await app.Services.SeedDatabaseAsync(lifetime.ApplicationStopping);
}

app.UseExceptionHandler();

app.UseCors("AllowFrontendApp");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


