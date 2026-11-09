using DonationHub.API;
using DonationHub.API.Extensions;
using DonationHub.Application;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddOptionsConfiguration(builder.Configuration);

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.Services.SeedDatabaseAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
