using System.ComponentModel.DataAnnotations;

namespace MedicationManagementAPI.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "INVALID email format.")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 10 characters long.")]
        public string Password { get; set; }

    }
}

