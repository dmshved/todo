using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record RegisterDto(
    [Required, EmailAddress] 
    string Email,
    
    [Required, MinLength(11)] 
    string Password
);