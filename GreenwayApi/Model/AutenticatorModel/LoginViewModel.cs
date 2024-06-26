﻿using System.ComponentModel.DataAnnotations;

namespace GreenwayApi.Model.AutenticatorModel
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be valid")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
