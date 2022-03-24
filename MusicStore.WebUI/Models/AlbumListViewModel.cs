using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class AlbumListViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<Album> Albums { get; set; }
       
        public IEnumerable<Artist> Artist { get; set; }
        public IEnumerable<Genre> Genre { get; set; }

        public IEnumerable<AlbumAllDetails> AlbumsWithArtists { get; set; }

        public Task<IEnumerable<AlbumAllDetails>> GetAlbumsWithArtists {get; set; }
        public Task<IEnumerable<Genre>> GetGenreAsync { get; set; }





    }

}