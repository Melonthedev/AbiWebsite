using System.ComponentModel.DataAnnotations;

namespace AbiWebsite.Models {
    public class Student {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string? Nickname { get; set; }
        [Required(ErrorMessage = "Tutor muss ausgewählt werden.")]
        public string Tutor { get; set; }

        [Required(ErrorMessage = "E-Mail kann nicht leer sein.")]
        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
        public string Email { get; set; }
        public int LoginCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsApproved { get; set; } = false;
        public bool IsAdmin { get; set; } = false;


        public ICollection<MottoSuggestion> Suggestions { get; set; } = [];
        public ICollection<Vote> Votes { get; set; } = [];

        public string GetFormattedName(bool isAdmin = false) {
            if (isAdmin) {
                return FullName + (Nickname == null || Nickname == "" ? "" : $" ({Nickname})");
            } else {
                return (Nickname == null || Nickname == "") ? FullName : Nickname;
            }
        }
    }
}
