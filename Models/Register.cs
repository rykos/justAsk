using System.ComponentModel.DataAnnotations;

namespace justAsk.Models
{
    public class RegisterModel : LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
