using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class ProductModelDto
    {
        public int AlbumId { get; set; }
        [Required(ErrorMessage = "Proszę podać tytuł")]
        [Display(Name = "Tytuł")]
        public string AlbumName { get; set; }
        [Display(Name = "Wytwórnia")]
        public int LabelId { get; set; }
        [Required(ErrorMessage = "Proszę podać rok")]
        [Display(Name = "Rok")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Proszę podać gatunek")]
        [Display(Name = "Gatunek")]
        public int Genre { get; set; }
        [Required(ErrorMessage = "Proszę wybrać kraj pochodzenia")]
        [Display(Name = "Kraj")]
        public int Country { get; set; }
        [Required(ErrorMessage = "Proszę podać cenę")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        public int GraphicId { get; set; }
        [Required(ErrorMessage = "Proszę podać wykonawcę")]
        [Display(Name = "Wykonawca")]
        public string ArtistName { get; set; }
        [Required(ErrorMessage = "Proszę podać liczbę piosenek")]
        [Display(Name = "Liczba piosenek")]
        public int NumberOfSongs { get; set; }
        public int ArtistId { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Label> Labels { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public bool Wishlist { get; set; }


    }
}