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
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; } 
    }
}
