using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.WebUI.Models;


namespace MusicStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductsRepository repository;
        private IAccountsRepository accountsRepository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductsRepository repo, IOrderProcessor proc, IAccountsRepository accounts)
        {
            repository = repo;
            orderProcessor = proc;
            accountsRepository = accounts; 
        }
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
                
            });
           
        }

        public async Task<RedirectToRouteResult> AddToCart(Cart cart, int productId, string returnUrl)
        {

            var apiModel = await repository.GetAlbumWithArtistAsync();
            AlbumAllDetails album = apiModel.FirstOrDefault(p => p.album.AlbumId == productId);

            if (album != null)
            {
                cart.AddItem(album);
            }
            return RedirectToAction("Index", new { returnUrl });

        }


        public async Task<RedirectToRouteResult> RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            var apiModel = await repository.GetAlbumWithArtistAsync();
            AlbumAllDetails album = apiModel.FirstOrDefault(p => p.album.AlbumId == productId);

            if (album != null)
            {
                cart.RemoveLine(album);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //private Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if(cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


        public async Task<ActionResult> Checkout()
        {
            if (Session["Id"] != null)
            {
                int id = (int)Session["Id"];
                Client client = await accountsRepository.GetClient(id);
                Adress adress = await accountsRepository.GetAdressesAsync(client.Id);
                if (adress != null)
                {
                    return View(new ShippingDetails()
                    {
                        Name = client.Surname,
                        City = adress.City,
                        Line1 = adress.Town,
                        Line2 = adress.Street,
                        Line3 = adress.HouseNumber.ToString(),
                        Zip = adress.PostCode,


                    });
                }
                else
                {
                    return View(new ShippingDetails());
                }
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Cart cart, ShippingDetails shippingDetails, int Id)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);

                
                List<int> albumsId = new List<int>();
                foreach(var line in cart.Lines)
                {
                    albumsId.Add(line.Product.album.AlbumId);
                }
                if (albumsId.Count > 0 && Id >= 1)
                {
                    var clientId = await accountsRepository.GetClient(Id);
                    await accountsRepository.AddMusicToLibrary(albumsId, clientId.Id);
                    await orderProcessor.NewOrder(clientId.Id, albumsId);
                }
                else
                {
                    return RedirectToAction("Nothing");
                }
                cart.Clear();
                return View("Completed");


            }
            else
            {
                return View(shippingDetails);
            }
        }

        public string Nothing()
        {
            return "Hej tu nie jest pusto";
        }

    }
}