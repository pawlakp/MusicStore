using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class SearchedListViewModel
    {
        public List<AlbumAllDetails> albumsList { get; set; }
        public List<Artist> artistsList { get; set; }
        public List<AlbumAllDetails> songsWithAlbum { get; set; }
        
        public string searchedItem { get; set; }
       

    }
}