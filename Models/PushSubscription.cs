namespace AbiWebsite.Models {
    public class PushSubscription {
        public int Id { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public string P256DH { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        public int? StudentId { get; set; }
    }
}