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


namespace MusicStore.WebUI.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private IClientRepository clientRepo;
        private IProductsRepository productsRepo;
        private IAccountRepository accountRepo;
        public int PageSize = 8;
        // GET: Admin

        public ClientController(IClientRepository repo, IProductsRepository repo2, IAccountRepository accountRepository)
        {
            this.clientRepo = repo;
            this.productsRepo = repo2;
            accountRepo = accountRepository;
        }
        public ActionResult Index()
        {
           
            return View();
        }

        public async Task<ActionResult> GetPreferences()
        {
            var list = await clientRepo.GetClientPreferences(1);
            return View(list.OrderByDescending(x=>x.genreAppearances));
        }
        public async Task<ActionResult> AfterLogin(Accounts account)
        {
            if (!await clientRepo.IsClientExist(account.Id)) return RedirectToAction("Create", account);
            else
            {
                Client client = await clientRepo.GetClientByAccountId(account.Id);
                return View("Index",client);
            }
        }

        public ActionResult Create(Accounts account)
        {
            Client client = new Client()
            {
                AccountId = account.Id,
            };
            return View(client);

        }

        [HttpPost]
        public async Task<ActionResult> Create(Client client)
        {
            await clientRepo.CreateAccount(client);
            return RedirectToAction("AddAdress",client);
        }

        public async Task<ActionResult> AddAdress(Client user)
        {
            var client = await clientRepo.GetClient(user);
            Adress adress = new Adress()
            {
                ClientId = client.Id,
            };
            return View(adress);
        }

        [HttpPost]
        public async Task<ActionResult> AddAdress(Adress adress)
        {
            await clientRepo.CreateAdress(adress);
            return View("Index");
        }

        public async Task<ActionResult> DetailsAdress(int id)
        {
            var client = await clientRepo.GetClientByAccountId(id);
            return View(await clientRepo.GetAdressesAsync(client.Id));
        }
        
        public async Task<ActionResult> EditAdress(int id)
        {
            return View(await clientRepo.GetAdressesAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> EditAdress(Adress adress)
        {
            await clientRepo.EditAdress(adress);
            return View("DetailsAdress", adress);
        }

        public async Task<ActionResult> Library(int id, int page = 1)
        {
           
            var clientId = await clientRepo.GetClientByAccountId(id);
            var a = await clientRepo.GetClientLibraryAsync(clientId.Id);
            var b = await productsRepo.GetAlbumsToLibrary(a);
            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists = b.OrderBy(p => p.album.ArtistId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = b.Count()
                },
            };



            return View(model);
        }

        public async Task<ActionResult> Orders(int id)
        {
            var clientId = await clientRepo.GetClientByAccountId(id);
            List<Order> list = await clientRepo.GetAllClientOrders(clientId.Id);
            return View(list);
        }

        public async Task<ActionResult> OrderDetails(int id)
        {
            var x = await clientRepo.GetOrderDetails(id);
            List<OrderAlbumModelDto> orderDetails = new List<OrderAlbumModelDto>();
            decimal totalValue = 0;
            foreach (var item in x)
            {
                var album = await productsRepo.GetAlbumAsync(item.AlbumId);
                orderDetails.Add(new OrderAlbumModelDto
                {
                    Name = album.Name,
                    Price = album.Price
                    
                });
                totalValue += album.Price;
            }
            orderDetails[0].TotalValue= totalValue;
            return View(orderDetails);
        }

        public async Task<JsonResult> ChangePassword(int id)
        {
            await accountRepo.ChangePassword(id);
            var result = await accountRepo.GetAccountAsync(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Wishlist(int id, int page = 1)
        {

            var clientId = await clientRepo.GetClientByAccountId(id);
            var a = await clientRepo.GetClientWishlist(clientId.Id);
            var b = await productsRepo.GetAlbumsToLibrary(a);
            AlbumListViewModel model = new AlbumListViewModel
            {
                AlbumsWithArtists = b.OrderBy(p => p.album.ArtistId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = b.Count()
                },
            };



            return View(model);
        }

        public async Task<JsonResult> AddToWishList(int productId, int clientId)
        {
            var client = await clientRepo.GetClientByAccountId(clientId);
            bool IsExist = await clientRepo.AddToClientWishlist(client.Id, productId);

            var result = new ProductModelDto();
            if (IsExist)
            {
                result = new ProductModelDto
                {
                    AlbumId = productId,
                    Wishlist = true,
                };
            }
            else
            {
                result = new ProductModelDto
                {
                    AlbumId = productId,
                    Wishlist = false,
                };
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}