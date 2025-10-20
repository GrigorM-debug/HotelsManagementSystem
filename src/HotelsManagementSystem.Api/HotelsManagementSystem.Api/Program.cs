using HotelsManagementSystem.Api.Extensions;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddProblemDetailsConfiguration();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiConfiguration();
builder.Services.AddCorsConfiguration();
// Added Rate Limiting configuration
builder.Services.AddRateLimitingConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Hotels Management System API Docs")
            .WithTheme(ScalarTheme.Default)
            .WithOpenApiRoutePattern("/openapi/v1.json");
    });
}
else
{
    app.UseExceptionHandler("/error");
}

//Apply migrations at application startup
await DatabaseExtension.ApplyMigrationsAsync(app);
//Seed initial users data
await DatabaseExtension.SeedUsersAndRoles(app);

app.UseHttpsRedirection();

//app.UseStaticFiles();

app.UseCors(ServiceExtensions.MyAllowSpecificOrigins);

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
