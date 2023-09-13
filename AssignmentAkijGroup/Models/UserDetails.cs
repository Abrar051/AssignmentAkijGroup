using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AssignmentAkijGroup.Models
{
    public class UserDetails : IValidatableObject
    {

        [Required(ErrorMessage = "The User field is required.")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters long.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username must contain only letters and numbers.")]
        public string User { get; set; }

        [Required(ErrorMessage = "The PassWord field is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string PassWord { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(User) && !System.Text.RegularExpressions.Regex.IsMatch(User, "^[a-zA-Z0-9]*$"))
            {
                yield return new ValidationResult("Username must contain only letters and numbers.", new[] { nameof(User) });
            }
        }



    }


}
