using DonationHub.API.Extensions;
using DonationHub.Application;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json;
//your-256-bit-secret-for-testing-only-change-me

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();



builder.Services.AddOptionsConfiguration(builder.Configuration);

builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        var value = authHeader.ToString();
        if (value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = value["Bearer ".Length..].Trim();
            Console.WriteLine("===== RAW JWT =====");
            Console.WriteLine(token);

            try
            {
                var parts = token.Split('.');
                if (parts.Length >= 2)
                {
                    var headerJson = DecodeBase64Url(parts[0]);
                    var payloadJson = DecodeBase64Url(parts[1]);

                    Console.WriteLine("===== HEADER =====");
                    Console.WriteLine(Pretty(headerJson));

                    Console.WriteLine("===== PAYLOAD =====");
                    Console.WriteLine(Pretty(payloadJson));

                    // Print well-known claims nicely
                    using var doc = JsonDocument.Parse(payloadJson);
                    var root = doc.RootElement;
                    foreach (var claim in new[]
                    {
                        "iss", "aud", "sub", "name", "preferred_username",
                        "email", "exp", "nbf", "iat", "jti", "scp", "roles"
                    })
                    {
                        if (root.TryGetProperty(claim, out var v))
                            Console.WriteLine($"  {claim,-20} = {v}");
                    }

                    // Expiry check (human-readable)
                    if (root.TryGetProperty("exp", out var expEl) && expEl.TryGetInt64(out var exp))
                    {
                        var expiresAt = DateTimeOffset.FromUnixTimeSeconds(exp);
                        var remaining = expiresAt - DateTimeOffset.UtcNow;
                        Console.WriteLine($"  expires at           = {expiresAt:u}");
                        Console.WriteLine($"  remaining            = {remaining}");
                        if (remaining <= TimeSpan.Zero)
                            Console.WriteLine("  !! TOKEN IS EXPIRED !!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to decode JWT: {ex.Message}");
            }
        }
    }
    else
    {
        Console.WriteLine("No Authorization header present.");
    }

    await next();
});

// ---- helpers (put at bottom of Program.cs or in a static class) ----

static string DecodeBase64Url(string input)
{
    var s = input.Replace('-', '+').Replace('_', '/');
    switch (s.Length % 4)
    {
        case 2: s += "=="; break;
        case 3: s += "="; break;
    }
    return Encoding.UTF8.GetString(Convert.FromBase64String(s));
}

static string Pretty(string json)
{
    using var doc = JsonDocument.Parse(json);
    return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
}

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
