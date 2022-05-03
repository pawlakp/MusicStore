using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Models
{
    public class AssignAlbumModelDto
    {
        public List<Accounts> ClientsList { get; set; }
        public List<Album> AlbumsList { get; set; }
        [Display(Name = "Album")]
        public int albumId { get; set; }
        [Display(Name = "Użytkownik")]
        public int clientId { get; set; }
    }
}