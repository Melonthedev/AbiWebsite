namespace AbiWebsite.Models {
    public class Vote {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = default!;

        public int MottoSuggestionId { get; set; }
        public MottoSuggestion MottoSuggestion { get; set; } = default!;

        // Stimme: +1 = Daumen hoch, -1 = Daumen runter
        public int Value { get; set; }
    }
}
