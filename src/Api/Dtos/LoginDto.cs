using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Dtos;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);