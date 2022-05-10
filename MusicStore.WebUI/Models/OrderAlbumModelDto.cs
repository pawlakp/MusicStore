using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.WebUI.Models
{
    public class OrderAlbumModelDto
    {
        [Display(Name = "Tytuł")]
        public string Name { get; set; }
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Display(Name = "Wartość całkowita")]
        public decimal TotalValue { get; set; }
        [Display(Name = "Adres klienta")]
        public string ClientMail { get; set; }
        [Display(Name = "Imie")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

    }
}