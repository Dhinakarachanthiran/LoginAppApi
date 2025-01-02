using System.ComponentModel.DataAnnotations;

namespace LoginAppApi.Models
{
    public class Role
    {

        public int RoleId { get; set; }  // Unique ID for the Role, auto-incremented in the database

        [Required]
        [StringLength(255)]
        public string RoleTitle { get; set; }  // The title or name of the role (e.g., Admin, User, etc.)

        public string RolePermissions { get; set; }  // The list of permissions (e.g., "Read, Write, Delete")

        public DateTime CreatedAt { get; set; }  // Timestamp of when the role was created
    }
}
