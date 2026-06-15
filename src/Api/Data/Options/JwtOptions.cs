using System.ComponentModel.DataAnnotations;

namespace ToDo.Api.Data.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    [Required(AllowEmptyStrings = false)]
    public string Issuer { get; set; } = string.Empty;
    
    [Required(AllowEmptyStrings = false)]
    public string Audience { get; set; } = string.Empty;
    [Range(2, 60)]
    public int Expiration { get; set; }
    
    public string SigningKey { get; set; } = string.Empty;
}