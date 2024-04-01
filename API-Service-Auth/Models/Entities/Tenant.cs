using System.ComponentModel.DataAnnotations;

namespace API_Service_Auth.Models.Entities;


public class Tenant
{
    public const string TenantIdField = "TenantId";
    public const string TenantNameField = "TenantName";
    public const string TenantCodeField = "TenantCode"; // Menambahkan property TenantCode
    public const string CreatedByField = "CreatedBy";
    public const string CreatedAtField = "CreatedAt";
    public const string ModifiedByField = "ModifiedBy";
    public const string ModifiedAtField = "ModifiedAt";
    public const string DeletedField = "Deleted";

    [Key]

    public Guid TenantId { get; set; }
    public string? TenantName { get; set; }
    public string? TenantCode { get; set; } // Menambahkan property TenantCode
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool Deleted { get; set; }
}

