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
        private IAccountRepository accounts;
        private IProductsRepository products;
        private IOrderProcessor orders;
        private IClientRepository clients;
        public int PageSize = 10;
        // GET: Admin

        public AdminController(IAccountRepository accountsRepository, IProductsRepository productsRepository, IClientRepository clientRepository, IOrderProcessor orderProcessor)
        {
            this.accounts = accountsRepository;
            this.products = productsRepository;
            this.clients = clientRepository;
            this.orders = orderProcessor;
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
            var apiModel = await this.accounts.AllAccountsAsync();
            if (ModelState.IsValid)
            {
                var check = apiModel.FirstOrDefault(s => s.Login == konto.Login);
                if (check == null)
                {
                    await this.accounts.AddAccount(konto);
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
            var result = await accounts.GetAccountAsync(id);
            await accounts.DeleteUser(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> ChangePassword(int id)
        {
            await accounts.ChangePassword(id);
            var result = await accounts.GetAccountAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ListUsers()
        {
            var list = await accounts.AllAccountsAsync();
            return View(list.AsEnumerable());
        }
        public async Task<ActionResult> ListAlbums(int page = 1)
        {
            //var list = await products.GetAlbumWithArtistAsync();
            //return View(list.AsEnumerable());
            var apiModel = await products.AllAlbumsDetails();
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

        public async Task<ActionResult> DetailsAlbum(int id)
        {
            var album = await products.GetAlbumDetailsAsync(id);
            return View(album);
        }

        public async Task<ActionResult> DetailsClient(int id)
        {
            var client = await clients.GetClientByAccountId(id);
            var adress = await clients.GetAdressesAsync(client.Id);

            ClientModelDto clientDetails = new ClientModelDto
            {
                City = adress.City,
                Town = adress.Town,
                PostCode = adress.PostCode,
                Street = adress.Street,
                HouseNumber = adress.HouseNumber,
                ApartamentNumber = adress.ApartamentNumber,
                State = adress.State,
                Country = adress.Country,
                FirstName = client.FirstName,
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,

            };
            return View(clientDetails);
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
        public async Task<ActionResult> AddAlbum(ProductModelDto product, string labelName, HttpPostedFileBase image =null)
        {
            var apiModel = await this.accounts.AllAccountsAsync();
            if(labelName != null)
            {
                
                await products.AddLabelAsync(labelName);
                
                if (ModelState["LabelId"] != null) ModelState["LabelId"].Errors.Clear();
  
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                var label = await products.GetLabelAsync(labelName);
                Album helpAlbum = new Album
                {
                    Name = product.AlbumName,
                    Id = product.AlbumId,
                    LabelId = label.Id,
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
        public ActionResult AddSong(ProductModelDto product)
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
        public async Task<ActionResult> AddSong(SongModelDto piosenki, int albumId,int artistId)
        {
            for (int i = 0; i < piosenki.SongList.Count; i++)
            {
                piosenki.SongList[i].AlbumId = piosenki.AlbumId;
                piosenki.SongList[i].ArtistId = piosenki.ArtistId;
            }
            
            await products.AddSongsAsync(piosenki.SongList);

            return RedirectToAction("Index");


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
                ImageData = album.ImageData,
                ImageMimeType = album.ImageMimeType,
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
        public async Task<ActionResult> AssignAlbum()
        {
            var client = await accounts.AllAccountsAsync();
            var albumList = await products.AllAlbumAsync();
            var clientList = client.Where(x => x.IsAdmin == false).ToList();
            return View(new AssignAlbumModelDto
            {
              ClientsList = clientList,
              AlbumsList = albumList
            });
        }

        [HttpPost]
        public async Task AssignAlbum(AssignAlbumModelDto newAssign)
        {
            var client = await clients.GetClientByAccountId(newAssign.clientId);
            await clients.AddAlbumToClientLibrary(newAssign.albumId,client.Id);


        }


        public async Task<JsonResult> DeleteAlbum(int id)
        {
            var result = await products.GetAlbumAsync(id);
            await products.DeleteAlbumAsync(id);
            return Json(result,JsonRequestBehavior.AllowGet);

        }

        public async Task<PartialViewResult> News()
        {
            var numberOfOrders = await orders.AllOrdersAsync();
            var numberOfSaleAlbums = await orders.AllOrdersAlbumAsync();
            var numberOfAlbums = await products.AllAlbumAsync();
            var moneyEarned = await orders.GetAllMoneyEarned();
            var numberOfUsers = await clients.AllClientsAsync();
           
            return PartialView(new NewsModelDto
            {
                NumberOfOrders = numberOfOrders.Count(),
                NumberOfSaleAlbums = numberOfSaleAlbums.Count(),
                NumberOfAlbums = numberOfAlbums.Count(),
                MoneyEarned = moneyEarned,
                NumberOfUsers = numberOfUsers.Count() 
            });
        }

        public async Task<PartialViewResult> NumberAllbumsView()
        {
           
            var numberOfAlbums = await products.AllAlbumAsync();
           

            return PartialView(new NewsModelDto
            {               
                NumberOfAlbums = numberOfAlbums.Count(),
            });
        }


      public async Task<ActionResult> ListOrders()
        {
           var a = await clients.AllOrdersAsync();
           
            return View(a);

        }

        public async Task<ActionResult> OrderDetails(int id)
        {
            var clientOrder = await clients.GetOrderDetails(id);
            var getOrder = await clients.AllOrdersAsync();
            decimal totalValue = 0;
            List<OrderAlbumModelDto> orderDetails = new List<OrderAlbumModelDto>();
          
            Client client = await clients.GetClientById(getOrder[1].ClientId);
            Accounts account = await accounts.GetAccountAsync(client.AccountId);

            foreach (var item in clientOrder)
            {
                var album = await products.GetAlbumAsync(item.AlbumId);
                orderDetails.Add(new OrderAlbumModelDto
                {
                    Name = album.Name,
                    Price = album.Price

                });
                totalValue += album.Price;
            }

            orderDetails[0].TotalValue = totalValue;
            orderDetails[0].ClientMail = account.Login;
            orderDetails[0].FirstName = client.FirstName;
            orderDetails[0].Surname = client.Surname;


            return View(orderDetails);
            
        }





    }
}