using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class Adress
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Display(Name = "Miejscowość")]
        public string City { get; set; }
        [Display(Name = "Miasto")]
        public string Town { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string PostCode { get; set; }
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Display(Name = "Numer domu")]
        public int HouseNumber { get; set; }
        [Display(Name = "Numer lok.")]
        public int ApartamentNumber { get; set; }
        [Display(Name = "Województwo")]
        public string State { get; set; }
        [Display(Name = "Kraj")]
        public string Country { get; set; }

    }
}
