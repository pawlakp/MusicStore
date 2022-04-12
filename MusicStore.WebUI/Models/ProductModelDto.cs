using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class ProductModelDto
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public int LabelId { get; set; }
        public string Year { get; set; }
        public int Genre { get; set; }
        public int Country { get; set; }
        public decimal Price { get; set; }
        public int GraphicId { get; set; }
        public string ArtistName { get; set; }
        public int NumberOfSongs { get; set; }
        public List<Genre> Genres { get; set; }

    }
}