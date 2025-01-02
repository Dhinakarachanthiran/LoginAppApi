using System.ComponentModel.DataAnnotations;

namespace LoginAppApi.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        // Validation to ensure Name is required
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string DepartmentName { get; set; }

        public string DepartmentDescription { get; set; }

        // Ensure CreatedAt is set to current time when creating a new Department
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
