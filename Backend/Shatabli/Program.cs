using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shatabli.API;
using Shatabli.Core.Application;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Infrastructure;
using Shatabli.Infrastructure.Context;
using Shatabli.Infrastructure.Data;
using Shatabli.Infrastructure.Middleware;
using Shatabli.Infrastructure.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shatabli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Add JSON options with camelCase naming policy
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddCoreApplicationService();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddTransient<IClaimsService, ClaimsService>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSecretKeyHere_MustBe32CharactersOrMore!";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "ShatabliAPI",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "ShatabliClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient<IStorageService, CloudinaryService>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(5);
            });


            builder.Services.AddScoped<IPathProvider, WebPathProvider>();

            builder.Services.AddCors();

            builder.Services.AddTransient<IDesignBackgroundJobService, DesignBackgroundJobService>();

            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Database Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplictionDbContext>();
                    await DatabaseSeeder.SeedAsync(context, builder.Configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "❌ An error occurred while seeding the database.");
                }
            }

            // ✅ IMPORTANT: Add Global Exception Handler BEFORE everything else
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHangfireDashboard();


            app.MapControllers();

            await app.RunAsync();
        }
    }
}
