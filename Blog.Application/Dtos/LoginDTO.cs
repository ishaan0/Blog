using System.ComponentModel.DataAnnotations;

namespace Blog.Application.Dtos
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email adress format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
