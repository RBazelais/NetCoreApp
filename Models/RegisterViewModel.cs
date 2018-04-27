using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApp.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Your name can only container letters")]
        [MinLength(3, ErrorMessage = "First name should be at least 3 characters long")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Your name can only container letters")]
        [MinLength(3, ErrorMessage = "Last name should be at least 3 characters long")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage="Please ensure that you have entered a valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters")]
        // [RegularExpression("^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[$@$!%*#?&]){8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain: one letter [A-Za-z], one number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Required]
        [Compare ("Password", ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters")]
        public string ConfirmPassword { get; set; }
    }
}