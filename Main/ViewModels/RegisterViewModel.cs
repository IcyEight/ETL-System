using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel() { }
        [Required(ErrorMessage = "First name can't be empty"), MaxLength(256), Display(Name = "First Name")]
        public string Firstname
        {
            get;
            set;
        }
        [Required(ErrorMessage = "Last name can't be empty"), MaxLength(256), Display(Name = "Last Name")]
        public string Lastname
        {
            get;
            set;
        }
        [Required, EmailAddress, MaxLength(256), Display(Name = "Email Address")]
        public string Email
        {
            get;
            set;
        }
        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Password")]
        public string Password
        {
            get;
            set;
        }
        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The confirmation password does not match the password")]
        public string ConfirmPassword
        {
            get;
            set;
        }
    }
}
