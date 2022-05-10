using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class AlbumDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Label { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public List<Song> Songs { get; set; }
    }
}
