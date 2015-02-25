using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kontakter.Models
{

    public class Contact
    {
        public Guid ContactId { get; set; }

        [Required(ErrorMessage = "Namn måste anges")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Namn måste vara 3-50 tkn")]
        [DisplayName("Namn")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Efternamn måste anges")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Efternamn måste vara 3-50 tkn")]
        [DisplayName("Efternamn")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email måste anges")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Efternamn måste vara 3-50 tkn")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Ange en giltid Email")]
        public string Email { get; set; }
    }
}



