using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace NoteKeeper.WebApi.Config;

public class CorsConfig(IWebHostEnvironment environment, IConfiguration configuration) : IConfigureOptions<CorsOptions>
{
    public void Configure(CorsOptions options)
    {
        if (environment.IsDevelopment())
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }
        else
        {
            string? origensPermitidasString = configuration["CORS_ALLOWED_ORIGINS"];

            if (string.IsNullOrWhiteSpace(origensPermitidasString))
            {
                throw new Exception("\"CORS_ALLOWED_ORIGINS\" não está configurado.");
            }

            string[]? origensPermitidas = origensPermitidasString
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.TrimEnd('/'))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(origensPermitidas)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        }
    }
}
