using System.ComponentModel.DataAnnotations;

namespace API_Service_Auth.Models.Requests;

public class LoginRequest
{
    [Required]
    public string? UserName { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [Required]
    public string? TenantCode { get; set; } // Menambahkan property TenantCode
}
