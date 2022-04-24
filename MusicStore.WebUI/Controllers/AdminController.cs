using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Concrete;
using MusicStore.WebUI.Models;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;

namespace MusicStore.WebUI.Controllers
{
    [Authorize(Users = "admin")]
    public class AdminController : Controller
    {
        private IAccountRepository account;
        private IProductsRepository products;
        public int PageSize = 10;
        // GET: Admin

        public AdminController(IAccountRepository accountsRepository, IProductsRepository productsRepository)
        {
            this.account = accountsRepository;
            this.products = productsRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(Accounts konto)
        {
            var apiModel = await this.account.AllAccountsAsync();
            if (ModelState.IsValid)
            {
                var check = apiModel.FirstOrDefault(s => s.Login == konto.Login);
                if (check == null)
                {
                    await this.account.AddAccount(konto);
                    return RedirectToAction("Index");
                }
                else
                { 
                    return View();
                }


            }
            return View();

        }

        public async Task<ActionResult> DeleteUser(int id)
        {
            await account.DeleteUser(id);
            return RedirectToAction("List");

        }

        public async Task<ActionResult> ChangePassword(int id)
        {
            await account.ChangePassword(id);
            return RedirectToAction("List");

        }

        public async Task<ActionResult> ListUsers()
        {
            var list = await account.AllAccountsAsync();
            return View(list.AsEnumerable());
        }
        public async Task<ActionResult> ListAlbums(int page = 1)
        {
            //var list = await products.GetAlbumWithArtistAsync();
            //return View(list.AsEnumerable());
            var apiModel = await products.GetAlbumWithArtistAsync();
            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists = apiModel.OrderBy(p => p.album.Id).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = apiModel.Count()
                },
            };


            return View(model);
        }

        public async Task<ActionResult> AddAlbum()
        { 
            
            ProductModelDto model = new ProductModelDto {
                Genres = await products.AllGenreAsync(),
                Labels = await products.AllLabelAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAlbum(ProductModelDto product, string LabelName, HttpPostedFileBase image =null)
        {
            var apiModel = await this.account.AllAccountsAsync();

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                else
                {
                    string a = "Pusto";
                }

                Album helpAlbum = new Album
                {
                    Name = product.AlbumName,
                    Id = product.AlbumId,
                    LabelId = product.LabelId,
                    Year = product.Year,
                    GenreId = product.Genre,
                    CountryId = product.Country,
                    Price = product.Price,
                    ImageData = product.ImageData,
                    ImageMimeType = product.ImageMimeType,
                   
                    
                };
                Artist helpArtist = new Artist
                {
                    Name = product.ArtistName
                };
                await this.products.AddAlbumAsync(new AlbumAllDetails
                {
                    album = helpAlbum,
                    artist = helpArtist

                });


                Album nowy = await products.GetAlbumAsync(product.AlbumName);
                product.AlbumId = nowy.Id;
                product.ArtistId = nowy.ArtistId;
                return RedirectToAction("AddSong", product);
            }
            else
            {

                product = new ProductModelDto
                {
                    Genres = await products.AllGenreAsync(),
                    Labels = await products.AllLabelAsync(),
                };
                return View(product);
            }

        }
        public async Task<ActionResult> AddSong(ProductModelDto product)
        {
            SongModelDto piosenki = new SongModelDto()
            {
                SongList = new List<Song>(new Song[product.NumberOfSongs]),
                AlbumId = product.AlbumId,
                ArtistId = product.ArtistId,
            };
            return View(piosenki);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> AddSong(SongModelDto piosenki, int albumId,int artistId)
        {
            for (int i = 0; i < piosenki.SongList.Count; i++)
            {
                piosenki.SongList[i].AlbumId = piosenki.AlbumId;
                piosenki.SongList[i].ArtistId = piosenki.ArtistId;
            }
            return await this.products.AddSongsAsync(piosenki.SongList);


        }

        public async Task<ActionResult> EditAlbum(int id)
        {
            var album = await products.GetAlbumAsync(id);
            Artist artist = await products.GetArtist(album.ArtistId);
            ProductModelDto product = new ProductModelDto()
            {
                AlbumId = album.Id,
                AlbumName = album.Name,
                Genre = album.GenreId,
                Country = album.CountryId,
                LabelId = album.LabelId,
                ArtistId = album.ArtistId,
                Price = album.Price,
                Year = album.Year,
                NumberOfSongs = 10,
                Genres = await products.AllGenreAsync(),
                Labels = await products.AllLabelAsync(),
                ArtistName = artist.Name,
            };
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> EditAlbum(ProductModelDto product, HttpPostedFileBase image = null)
        {
            if (image != null)
            {
                product.ImageMimeType = image.ContentType;
                product.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(product.ImageData, 0, image.ContentLength);
            }
            Album album = new Album
            {
               Id = product.AlbumId,
               Name = product.AlbumName,
               Price = product.Price,
               ArtistId = await products.GetArtistId(product.ArtistName),
               CountryId = product.Country,
               GenreId = product.Genre,
               Year = product.Year,
               LabelId = product.LabelId,
                ImageData = product.ImageData,
                ImageMimeType = product.ImageMimeType,
            };
            await products.EditAlbumAsync(album);
            return RedirectToAction("ListAlbums");
        }

        
        public async Task<ActionResult> DeleteAlbum(int id)
        {
            await products.DeleteAlbumAsync(id);
            return RedirectToAction("ListAlbums");
        }



 

    }
}