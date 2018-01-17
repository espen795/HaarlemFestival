using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaarlemFestival.Models
{
    public class ContactMessage
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [RegularExpression("(^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$)", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Please select a Category.")]
        public QuestionCategory Regarding { get; set; }

        [Required(ErrorMessage = "Please enter a question.")]
        public string Question { get; set; }

        public bool Answered { get; set; }

        public string Answer { get; set; }
    }
}