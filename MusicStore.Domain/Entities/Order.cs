using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime? Data { get; set; }
        
        public string Name { get; set; }
        public decimal Price { get; set; }
       
    }
}
