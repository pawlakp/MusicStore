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
        public int PageSize = 4;
        // GET: Admin

        public ClientController(IClientRepository repo, IProductsRepository repo2)
        {
            this.clientRepo = repo;
            this.productsRepo = repo2;
        }
        public async Task<ActionResult> Index(int id)
        {
            var list = await clientRepo.AllClientsAsync();
            Client client = list.Where(c => c.Id == id).FirstOrDefault();
            return View(client);
        }
        public async Task<ActionResult> AfterLogin(Accounts account)
        {
            if (!await clientRepo.IsClientExist(account.Id)) return RedirectToAction("Create", account);
            else
            {
                Client client = await clientRepo.GetClient(account.Id);
                return View("Index",client);
            }
        }

        public async Task<ActionResult> Create(Accounts account)
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
            var client = await clientRepo.GetClient(id);
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
           
            var clientId = await clientRepo.GetClient(id);
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
            var clientId = await clientRepo.GetClient(id);
            List<Order> list = await clientRepo.GetAllClientOrders(clientId.Id);
            return View(list);
        }

        public async Task<ActionResult> OrderDetails(int id)
        {
            var x = await clientRepo.GetOrderDetails(id);
            List<OrderAlbumModelDto> orderDetails = new List<OrderAlbumModelDto>();
           
            foreach (var item in x)
            {
                var album = await productsRepo.GetAlbumAsync(item.AlbumId);
                orderDetails.Add(new OrderAlbumModelDto
                {
                    Name = album.Name,
                    Price = album.Price
                }); 
            }
            return View(orderDetails);
        }



    }
}