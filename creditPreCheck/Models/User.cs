using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using creditPreCheck.Utils;

namespace creditPreCheck.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "Date of birth should be in the past.")]
        [OldestDate(ErrorMessage = "Date of birth should be after January 1, 1900.")]
        public DateTime DoB { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Your annual income")]
        public decimal Income { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public string FullName
        {
            get
            {
                var fName = $"{this.FirstName.First().ToString().ToUpper()}{this.FirstName.Substring(1)}";
                var lName = $"{this.LastName.First().ToString().ToUpper()}{this.LastName.Substring(1)}";
                return $"{fName} {lName}";
            }
        }

        public virtual Eligibility Eligibility { get; set; }
    }
}
