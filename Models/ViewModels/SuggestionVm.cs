namespace AbiWebsite.Models.ViewModels {
    public class SuggestionVm {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Vote> UpVotes { get; set; }
        public IEnumerable<Vote> DownVotes { get; set; }
        public int Score { get; set; }
        public int UserVote { get; set; }
    }
}
