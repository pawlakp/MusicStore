using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;

namespace MusicStore.WebUI.Controllers
{
    public class AlbumController : Controller
    {
       private IAlbumRepository repository;
        //public actionresult index()
        //{
        //    return view();
        //}
        public AlbumController(IAlbumRepository albumRepository)
        {
            this.repository = albumRepository;
        }

        public ViewResult List()
        {
            return View(repository.Album);
        }
    }
}