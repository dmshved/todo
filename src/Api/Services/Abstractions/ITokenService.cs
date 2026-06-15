using System.Security.Claims;

namespace ToDo.Api.Services.Abstractions;

public interface ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
}