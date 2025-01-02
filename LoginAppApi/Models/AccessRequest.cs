namespace LoginAppApi.Models
{
    public class AccessRequest
    {
        public string Role { get; set; }
        public List<AccessData> AccessData { get; set; }
    }
}
