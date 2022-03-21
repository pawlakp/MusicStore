using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public int LabelId { get; set; }
        public string Year { get; set; }
        public int GenreId { get; set; }
        public int CountryId { get; set; }
        public decimal Price { get; set; }
        

    }
  
}
