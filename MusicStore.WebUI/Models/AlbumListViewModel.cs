using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class AlbumListViewModel
    {
        public IEnumerable<Album> Albums { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<Artist> Artist { get; set; }

        public IEnumerable<AlbumsWithArtist> AlbumsWithArtists { get; set; }

      


    }

}