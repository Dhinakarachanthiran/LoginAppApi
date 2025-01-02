namespace LoginAppApi.Models
{
    public class TaskEntity
    {
        public int Id { get; set; } // Primary Key (Identity Column in SQL Server)
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }  // Date only, no time
        public DateTime?  EndDate { get; set; }    // Date only, no time
        public DateTime CreatedAt { get; set; }  // Will be auto-generated on create
    }
}
