using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class ClientLibrary
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AlbumId { get; set; }
    }
}
