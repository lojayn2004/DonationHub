using DonationHub.API.Extensions;
using DonationHub.Application;
using Scalar.AspNetCore;
//your-256-bit-secret-for-testing-only-change-me

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddOptionsConfiguration(builder.Configuration);

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.Services.SeedDatabaseAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
