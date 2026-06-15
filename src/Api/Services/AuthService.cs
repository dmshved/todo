using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using ToDo.Api.Dtos;
using ToDo.Api.Exceptions;
using ToDo.Api.Helpers;
using ToDo.Api.Identity;
using ToDo.Api.Services.Abstractions;

namespace ToDo.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    
    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Result<AuthResponseDto>.Fail(StatusCodes.Status401Unauthorized, "Invalid Username or Password");
        
        List<Claim> claims =
        [
            new (ClaimTypes.NameIdentifier, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email),
        ];

        var token = _tokenService.GenerateAccessToken(claims);

        var response = new AuthResponseDto(Token: token);

        return Result<AuthResponseDto>.Success(StatusCodes.Status200OK, response);
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email, 
            Email = dto.Email
        };
        
        var identityResult = await _userManager.CreateAsync(user, dto.Password);

        if (! identityResult.Succeeded)
        {
            var errors = string.Join(" | ", identityResult.Errors.Select(e => e.Description));

            throw new BadRequestException(errors);
        }

        List<Claim> claims =
        [
            new (ClaimTypes.NameIdentifier, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email),
        ];
        
        var token = _tokenService.GenerateAccessToken(claims);

        var response = new AuthResponseDto(Token: token);

        return Result<AuthResponseDto>.Success(StatusCodes.Status200OK, response);
    }
}