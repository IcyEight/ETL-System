using System;
using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel(){}
        public UserProfileViewModel(string firstName, string lastName, string email){
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
        [Required(ErrorMessage = "First name can't be empty")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name can't be empty")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress]
        public string Email { get; set; } 
    }
}
