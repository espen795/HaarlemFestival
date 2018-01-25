using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaarlemFestival.Models
{
    public class ContactMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your full name.")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [RegularExpression("(^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$)", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Display(Name = "Subject")]
        [Required(ErrorMessage = "Please enter a subject")]
        public string Subject { get; set; }

        [Display(Name = "Regarding")]
        [Required(ErrorMessage = "Please select a Category.")]
        public QuestionCategory Regarding { get; set; }

        [Display(Name = "Question")]
        [Required(ErrorMessage = "Please enter a question.")]
        public string Question { get; set; }

        public bool Answered { get; set; }

        [Display(Name = "Answer")]
        public string Answer { get; set; }
    }
}