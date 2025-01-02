namespace LoginAppApi.Models
{
    public class AccessData
    {
        public string ScreenName { get; set; }
        public string ScreenId { get; set; } // ScreenId as string from the request
        public bool HideShow { get; set; }
        public bool FullAccess { get; set; }
        public bool Read { get; set; }
    }
}
