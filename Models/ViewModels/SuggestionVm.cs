namespace AbiWebsite.Models.ViewModels {
    public class SuggestionVm {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public required IEnumerable<Vote> UpVotes { get; set; }
        public required IEnumerable<Vote> DownVotes { get; set; }
        public int Score { get; set; }
        public int UserVote { get; set; }
        public Student Student { get; set; } = default!;

    }
}
