using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class AlbumDetails
    {

        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        public string Name { get; set; }
        [Display(Name = "Artysta")]
        public string Artist { get; set; }
        [Display(Name = "Wytwórnia")]
        public string Label { get; set; }
        [Display(Name = "Rok")]
        public int Year { get; set; }
        [Display(Name = "Gatunek")]
        public string Genre { get; set; }
        [Display(Name = "Kraj")]
        public string Country { get; set; }
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
      
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public List<Song> Songs { get; set; }
    }
}
