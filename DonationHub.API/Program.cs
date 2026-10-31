using DonationHub.Application;
using DonationHub.Application.Behaviors;
using DonationHub.Domain.Models.User;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(new SoftDeleteInterceptor())
);

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// --------------------------- Add Options --------------------------
builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration("JwtOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddOptions<DonationOptions>()
    .BindConfiguration("DonationOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<SeedAdminOptions>()
    .BindConfiguration("AdminSeedOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.Configure<IdentityOptions>(options =>
{
 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;


    options.User.RequireUniqueEmail = true;
});
builder.Services.AddApplicationServices();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
