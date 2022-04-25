using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.WebUI.Models
{
    public class NewsModelDto
    {
        [Display(Name = "Liczba zamówień")]
        public int NumberOfOrders { get; set; }
        [Display(Name = "Liczba sprzedanych albumów")]
        public int NumberOfSaleAlbums { get; set; }
        [Display(Name = "Liczba albumów w ofercie")]
        public int NumberOfAlbums { get; set; }
        [Display(Name = "Wartość sprzedanych albumów")]
        public decimal MoneyEarned { get; set; }
        [Display(Name = "Liczba klientów")]
        public int NumberOfUsers { get; set; }
    }
}