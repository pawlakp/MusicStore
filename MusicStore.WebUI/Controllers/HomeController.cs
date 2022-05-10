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
using System.Web.Security;

namespace MusicStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
       private IProductsRepository repository;
        public int PageSize = 8;
   
        public HomeController(IProductsRepository albumRepository)
        {
            this.repository = albumRepository;
        }

        

        public async Task<ViewResult> Index(string sort, int page = 1, decimal price1=0, decimal price2=0)
        {
            ViewBag.sort = sort; ViewBag.price1 = price1; ViewBag.price2 = price2;
            var apiModel = await repository.GetAlbumWithArtistAsync();
            switch (sort)
            {

                case "dName":
                    apiModel = apiModel.OrderByDescending(x => x.album.Name);
                    break;

                case "dPrice":
                    apiModel = apiModel.OrderByDescending(x => x.album.Price);
                    break;

                case "Price":
                    apiModel = apiModel.OrderBy(x => x.album.Price);
                    break;

                case "Name":
                    apiModel = apiModel.OrderBy(x => x.album.Name);
                    break;

                default:
                    apiModel = apiModel.OrderBy(x => x.album.ArtistId);
                    break;

            }
            if(price1 > 0 && price2 > 0)
            {
                apiModel = apiModel.Where(x => x.album.Price >= price1 && x.album.Price <= price2);
            }
            else if(price1 > 0 && price2 <= 0)
            {
                apiModel = apiModel.Where(x => x.album.Price >= price1);
            }
            else if (price1 <= 0 && price2 > 0)
            {
                apiModel = apiModel.Where(x => x.album.Price <= price2);
            }

            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists =  apiModel.Skip((page - 1) * PageSize).Take(PageSize),
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

        public async Task<JsonResult> MiniFind(string searchedItem)
        {
            if(searchedItem != null && searchedItem.Length >= 3)
            {
                var albumList = await repository.FindAlbum(searchedItem);

                var artistList = await repository.FindArtist(searchedItem);
                var album = (from N in albumList
                             select new { N.Name }).ToList();

                var artist = (from N in artistList
                              select new { N.Name }).ToList();


                album.AddRange(artist);

                return Json(album, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> Find(string searchedItem)
        {
            var albums = await repository.FindAlbumWithDetails(searchedItem);
            var songs = await repository.FindAlbumWithSong(searchedItem);
            SearchedListViewModel list = new SearchedListViewModel
            {
                albumsList = albums.ToList(),
                artistsList = await repository.FindArtist(searchedItem),
                songsWithAlbum = songs.ToList(),
                searchedItem = searchedItem

            };

            return View(list);
        }


        
        public async Task<ActionResult> ArtistDiscography(int id, int page = 1)
        {
            var list = await repository.GetArtistDiscography(id);
            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists = list.OrderBy(p => p.album.ArtistId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = list.Count()
                },
            };


            return View(model);
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

