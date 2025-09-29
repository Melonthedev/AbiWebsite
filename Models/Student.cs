using System.ComponentModel.DataAnnotations;

namespace AbiWebsite.Models {
    public class Student {
        public int Id { get; set; }

        // Basisinfos
        [Required(ErrorMessage = "Name kann nicht leer sein.")]
        public string FullName { get; set; } = string.Empty;
        public string? Nickname { get; set; }
        [Required(ErrorMessage = "Tutor muss ausgewählt werden.")]
        public string Tutor { get; set; } 
        public string? Traits { get; set; } // Eigenschaften (frei oder kommagetrennt)

        // Login / Auth (optional)
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } // falls du keine externe Auth nutzt

        // Navigation
        public ICollection<MottoSuggestion> Suggestions { get; set; } = new List<MottoSuggestion>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
