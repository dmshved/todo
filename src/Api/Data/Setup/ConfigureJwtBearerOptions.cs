using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToDo.Api.Data.Options;

namespace ToDo.Api.Data.Setup;

public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly IOptions<JwtOptions> _options;

    public ConfigureJwtBearerOptions(IOptions<JwtOptions> options)
    {
        _options = options;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        JwtOptions jwtOptions = _options.Value;
        
        options.TokenValidationParameters = new TokenValidationParameters() 
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            
            ValidateLifetime = true,
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
        };
    }
}