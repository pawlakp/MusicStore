using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class Adress
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int ApartamentNumber { get; set; }
    }
}
