namespace AbiWebsite.Models {
    public class MottoSuggestion {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        // Wer hat es eingereicht
        public int StudentId { get; set; }
        public Student Student { get; set; } = default!;

        // Navigation
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
