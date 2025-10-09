using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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

        public bool IsApproved { get; set; } = false; // Wurde der Schüler von einem Admin freigeschaltet?

        public bool IsAdmin { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation
        public ICollection<MottoSuggestion> Suggestions { get; set; } = new List<MottoSuggestion>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        public string GetNickNameOrName(bool isAdmin = false) {
            return (Nickname == null || Nickname == "" || isAdmin) ? FullName : Nickname;
        }
    }
}
