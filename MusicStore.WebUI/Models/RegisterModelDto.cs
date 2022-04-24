using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MusicStore.WebUI.Models
{
    public class RegisterModelDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Proszę podać login")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Proszę podać hasło")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string RepeatPassword { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
    }
}