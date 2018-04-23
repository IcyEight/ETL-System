using System;
using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel() { }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
