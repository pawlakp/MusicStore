using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class AlbumListViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<AlbumAllDetails> AlbumsWithArtists { get; set; }
        public string CurrentGenre { get; set; }

    }

}