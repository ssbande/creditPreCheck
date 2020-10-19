using System.ComponentModel.DataAnnotations.Schema;

namespace creditPreCheck.Models
{
    public class Eligibility
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CardId { get; set; }
        public bool isEligible { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
    }
}