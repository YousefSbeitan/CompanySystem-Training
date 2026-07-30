using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;


public class RegisterDto
{
    [Required(
        ErrorMessage = "Username is required.")]
    [StringLength(
        100,
        ErrorMessage = "Username must not exceed 100 characters.")]
    [Display(
        Name = "Username")]
    public string Username { get; set; } = string.Empty;


    [Required(
        ErrorMessage = "Password is required.")]
    [StringLength(
        100,
        MinimumLength = 6,
        ErrorMessage = "Password must be between 6 and 100 characters.")]
    [Display(
        Name = "Password")]
    public string Password { get; set; } = string.Empty;


    [Display(
        Name = "Department")]
    public int? DepartmentId { get; set; }


    [Required(
        ErrorMessage = "Phone number is required.")]
    [Phone(
        ErrorMessage = "Invalid phone number.")]
    [StringLength(
        20,
        ErrorMessage = "Phone number must not exceed 20 characters.")]
    [Display(
        Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;
}



public class LoginDto
{
    [Required(
        ErrorMessage = "Username is required.")]
    [Display(
        Name = "Username")]
    public string Username { get; set; } = string.Empty;


    [Required(
        ErrorMessage = "Password is required.")]
    [Display(
        Name = "Password")]
    public string Password { get; set; } = string.Empty;
}



public class AuthResponseDto
{
    public string UserId { get; set; } = string.Empty;


    public string Username { get; set; } = string.Empty;


    public string Role { get; set; } = string.Empty;


    public List<string> Permissions { get; set; } = new();


    public string AccessToken { get; set; } = string.Empty;


    public string RefreshToken { get; set; } = string.Empty;


    public DateTime AccessTokenExpiresAt { get; set; }


    public DateTime RefreshTokenExpiresAt { get; set; }
}



public class RefreshTokenRequestDto
{
    [Required(
        ErrorMessage = "Refresh token is required.")]
    [Display(
        Name = "Refresh Token")]
    public string RefreshToken { get; set; } = string.Empty;
}