using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Concrete;
using MusicStore.WebUI.Models;  

namespace MusicStore.WebUI.Controllers
{
    public class AlbumController : Controller
    {
       private IProductsRepository repository;
        public int PageSize = 4;
        //public actionresult index()
        //{
        //    return view();
        //}
        public AlbumController(IProductsRepository albumRepository)
        {
            this.repository = albumRepository;
        }

        public ViewResult List(int page = 1 )
        {
            AlbumListViewModel model = new AlbumListViewModel
            {
                
                AlbumsWithArtists = repository.AlbumsWithArtist.OrderBy(p => p.album.ArtistId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.AlbumsWithArtist.Count()
                },
               
               
                
            };
           
                    
            return View(model);
        }

       
    }
}

