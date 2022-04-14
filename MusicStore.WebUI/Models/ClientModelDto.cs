using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.WebUI.Models
{
    public class ClientModelDto
    {
        //Adres
        public int ClientId { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int ApartamentNumber { get; set; }

        //Klient 
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public int AccountId { get; set; }
    }
}