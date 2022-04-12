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
        private IAccountsRepository account;
        private IProductsRepository products;
        // GET: Admin

        public AdminController(IAccountsRepository accountsRepository, IProductsRepository productsRepository)
        {
            this.account = accountsRepository;
            this.products = productsRepository;
        }
        public async Task<ActionResult> Index()
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
            var apiModel = await this.account.GetAllAsync();
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

        public async Task<ActionResult> List()
        {
            var list = await account.GetAllAsync();
            return View(list.AsEnumerable());
        }

        public async Task<ActionResult> AddAlbum()
        { 
            
            ProductModelDto model = new ProductModelDto {
                Genres = await products.GetGenreAsync(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> AddAlbum(ProductModelDto product, string LabelName)
        {
            //var apiModel = await this.account.GetAllAsync();

            //    if (ModelState.IsValid)
            //    {
            //    Album helpAlbum = new Album
            //    {
            //        Name = product.AlbumName,
            //        AlbumId = product.AlbumId,
            //        LabelId = product.LabelId,
            //        Year = product.Year,
            //        GenreId = product.GenreId,
            //        CountryId = product.CountryId,
            //        Price = product.Price,
            //        GraphicId = product.GraphicId
            //    };
            //    Artist helpArtist = new Artist { 
            //        Name = product.ArtistName
            //    };
            //    await this.products.AddAlbumAsync(new AlbumAllDetails
            //    {
            //        album = helpAlbum,
            //        artist = helpArtist

            //    });
            //}

            //return RedirectToAction("AddSong", product);
            return LabelName;

        }
        public async Task<ActionResult> AddSong(ProductModelDto product)
        {
            string arytysta = product.ArtistName.ToString();
            int artistId = await products.GetArtistId(arytysta);
            SongModelDto piosenki = new SongModelDto()
            {
                SongList = new List<Song>(new Song[product.NumberOfSongs]),
                AlbumId = product.AlbumId,
                ArtistId = artistId
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
 

    }
}