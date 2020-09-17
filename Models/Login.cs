using System.ComponentModel.DataAnnotations;

namespace justAsk.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Username should be longer than 4 characters")]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be longer than 5 characters")]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}