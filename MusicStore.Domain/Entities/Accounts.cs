using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Domain.Entities
{
    public class Accounts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Proszę podać login")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Proszę podać hasło")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Display(Name = "Administator")]
        public bool IsAdmin { get; set; }
        [Display(Name = "Czy wymagana jest zmiana hasła?")]
        public bool IsPasswordChangeRequired { get; set; }
        [Display(Name = "Czy użytkownik jest usunięty?")]
        public bool IsDeleted { get; set; }
    }
}
