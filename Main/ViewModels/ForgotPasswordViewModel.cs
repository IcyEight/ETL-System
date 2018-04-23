﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public ForgotPasswordViewModel(){}
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
