using System.ComponentModel.DataAnnotations;

namespace API_Service_Auth.Models.Entities;

    
    public class UserMst
    {
         public const string UserNameField = "UserName";
         public const string PasswordField = "Password";
         public const string UserMst_TenantIdField = "UserMst_TenantId";
         public const string CreatedByField = "CreatedBy";
         public const string CreatedAtField = "CreatedAt";
         public const string ModifiedByField = "ModifiedBy";
         public const string ModifiedAtField = "ModifiedAt";
         public const string DeletedField = "Deleted";
         [Key]
         public string? UserName { get; set; }
         public string? Password { get; set; }
         public Guid UserMst_TenantId { get; set; }
         public string? CreatedBy { get; set; }
         public DateTime CreatedAt { get; set; }
         public string? ModifiedBy { get; set; }
         public DateTime? ModifiedAt { get; set; }
         public bool Deleted { get; set; }
    }
