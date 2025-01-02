namespace LoginAppApi.Models
{
    public class Access
    {
        public int AccessId { get; set; }       // Primary Key
        public int ScreenId { get; set; }       // Foreign Key from Screens table
        public string ScreenName { get; set; }  // Name of the screen
        public string Role { get; set; }        // Role associated with access
        public bool FullAccess { get; set; }    // Full access flag
        public bool ReadOnly { get; set; }      // Read-only access flag
        public bool HideShow { get; set; }      // Flag for hiding or showing the screen
    }
}
