using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.WebUI.Models
{
    public class OrderAlbumModelDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }

        public string ClientMail { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }

    }
}