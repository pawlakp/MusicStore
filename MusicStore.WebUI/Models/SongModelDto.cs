using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class SongModelDto
    {
        public List<Song> SongList { get; set; }
        public int AlbumId { get; set; }
        public int ArtistId { get; set; }

    }
}