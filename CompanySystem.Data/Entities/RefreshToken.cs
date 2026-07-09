using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Entities;

public class RefreshToken : TrackingEntity
{
    [Key]
    public int RefreshTokenId { get; set; }


    [Required]
    [MaxLength(500)]
    public string Token { get; set; } = string.Empty;


    [Required]
    public string UserId { get; set; } = string.Empty;


    public DateTime ExpiresAt { get; set; }


    public bool IsRevoked { get; set; } = false;
}