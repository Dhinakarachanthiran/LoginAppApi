namespace LoginAppApi.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientLocation { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
