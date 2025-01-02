namespace LoginAppApi.Models
{
    public class Screen
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }  // Name of the screen (e.g., 'Dashboard', 'Reports')
        public string Role { get; set; }  // Role that can view this screen (e.g., 'Admin', 'User')
        public bool IsVisible { get; set; }  // Whether the screen is visible for the role
    }

}
