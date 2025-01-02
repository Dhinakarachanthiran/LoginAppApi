using System.ComponentModel.DataAnnotations;

public class AddUser
{
    [Key] // Primary key annotation
    public int UserId { get; set; }

    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string UserDepartment { get; set; }
    public string WorkingStatus { get; set; }
    public DateTime JoiningDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Default to current date/time when created
   
    
}