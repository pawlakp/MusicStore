using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Concrete;
using MusicStore.WebUI.Models;
using System.Threading.Tasks;

namespace MusicStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
       private IProductsRepository repository;
        public int PageSize = 6;
   
        public HomeController(IProductsRepository albumRepository)
        {
            this.repository = albumRepository;
        }

        public async Task<ViewResult> Index(int page = 1)
        {
            var apiModel = await repository.GetAlbumWithArtistAsync();
            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists =  apiModel.OrderBy(p => p.album.ArtistId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = apiModel.Count()
                },
            };


            return View(model);
        }

        public async Task<ViewResult> FiltrByGenre(string genre, int page = 1)
        {
            var genres = await repository.AllGenreAsync();
            int genreid = genres.First(p=> p.Name.Contains(genre)).Id;
            IEnumerable<AlbumAllDetails> apiModel = await repository.GetFiltredAlbumAsync(genreid);

            AlbumListViewModel model = new AlbumListViewModel
            {

                AlbumsWithArtists = apiModel
                .OrderBy(p => p.album.ArtistId)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = apiModel.Count()
                },

                CurrentGenre = genre,

            };


            return View(model);
        }

        public async Task<ViewResult> AllGenre()
        {
            var apiModel = await repository.AllGenreAsync();
            return View(apiModel);
        }

        public async Task<ActionResult> GetAlbumDetail(int id)
        {
            var album = await repository.GetAlbumDetailsAsync(id);
            return View(album);


        }


        public async Task<FileContentResult> GetImage(int productId)
        {
            var list = await repository.AllAlbumAsync();
            Album album = list.FirstOrDefault(p=> p.Id == productId); 
            if(album != null)
            {
                return File( album.ImageData,album.ImageMimeType);
            }
            else
            {
                return null;
            }
        }


    }
}

