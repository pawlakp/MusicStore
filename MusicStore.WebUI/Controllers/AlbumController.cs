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
    public class AlbumController : Controller
    {
       private IProductsRepository repository;
        public int PageSize = 4;
   
        public AlbumController(IProductsRepository albumRepository)
        {
            this.repository = albumRepository;
        }

        public async Task<ViewResult> List(int page = 1)
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

    }
}

