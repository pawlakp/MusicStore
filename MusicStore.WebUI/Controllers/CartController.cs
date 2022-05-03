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
        private IProductsRepository productsRepository;
        private IClientRepository clientRepository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductsRepository repo, IOrderProcessor proc, IClientRepository accounts)
        {
            productsRepository = repo;
            orderProcessor = proc;
            clientRepository = accounts; 
        }
        public ActionResult Index(Cart cart, string returnUrl)
        {
           if (cart.Lines.Count() == 0)
            {
                
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
                CartCount = cart.Lines.Count(),
                CartTotal = cart.ComputeTotalValue(),
                
            });
           
        }

        
    

        public async Task<ActionResult> AddToCart(Cart cart, int productId, string returnUrl)
        {

            var apiModel = await productsRepository.GetAlbumWithArtistAsync();
            AlbumAllDetails album = apiModel.FirstOrDefault(p => p.album.Id == productId);
            var result = "Błąd dodawania do koszyka";
            if (album != null)
            {
                cart.AddItem(album);
                result = "Dodano do koszyka";
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index", new { returnUrl });

        }


        public async Task<ActionResult> RemoveFromCart(Cart cart, int productId)
        {
            var apiModel = await productsRepository.GetAlbumWithArtistAsync();
            AlbumAllDetails album = apiModel.FirstOrDefault(p => p.album.Id == productId);

            if (album != null)
            {
                cart.RemoveLine(album);
            }
            var results = new CartIndexViewModel
            {
                CartTotal = cart.ComputeTotalValue(),
                DeleteId = productId
            };
            return Json(results, JsonRequestBehavior.AllowGet);

        }
        //public async Task<ActionResult> RemoveFromCart(Cart cart)
        //{
        //    cart.Clear();
        //    var results = new CartIndexViewModel
        //    {
        //        Cart = cart,
        //        CartCount = cart.Lines.Count(),
        //    };
        //    return Json(results,JsonRequestBehavior.AllowGet);

        //}
        //public async Task<ActionResult> RemoveFromCart(Cart cart, int productId, string returnUrl)
        //{
        //    var apiModel = await productsRepository.GetAlbumWithArtistAsync();
        //    AlbumAllDetails album = apiModel.FirstOrDefault(p => p.album.Id == productId);

        //    if (album != null)
        //    {
        //        cart.RemoveLine(album);
        //    }
        //    return View("Index", new CartIndexViewModel
        //    {
        //        Cart = cart,
        //        ReturnUrl = returnUrl,
        //        CartCount = cart.Lines.Count(),

        //    });
        //}

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

        public async Task<ActionResult> Checkout(Cart cart)
        {
            if (Session["Id"] != null)
            {

                int id = (int)Session["Id"];
                Client client = await clientRepository.GetClient(id);
                Adress adress = await clientRepository.GetAdressesAsync(client.Id);

                ShippingDetails data = new ShippingDetails()
                {
                    Name = client.Surname,
                    City = adress.City,
                    Line1 = adress.Town,
                    Line2 = adress.Street,
                    Line3 = adress.HouseNumber.ToString(),
                    Zip = adress.PostCode,
                    State = adress.State,
                    Country = adress.Country,


                };
                Cart nowy = await clientRepository.ClientHaveAlbum(client.Id, cart);

                var oldCart = cart.Lines.ToList();
                var newCart = nowy.Lines.ToList();
                var diffrence = newCart.Except(newCart).ToList();
                if (diffrence != null)
                {

                    ModelState.AddModelError("", "Usunięto albumy które masz w swojej bibliotece");

                }
                else
                {
                    var b = "ss";
                }

                return View(new CartIndexViewModel
                {
                    Cart = nowy,
                    CartCount = cart.Lines.Count(),
                    ShippingDetails = data

                });

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        //public async Task<ActionResult> ShippDetail()
        //{

        //        if (Session["Id"] != null)
        //        {
        //            int id = (int)Session["Id"];
        //            Client client = await clientRepository.GetClient(id);
        //            Adress adress = await clientRepository.GetAdressesAsync(client.Id);
        //            if (adress != null)
        //            {
        //                return View(new ShippingDetails()
        //                {
        //                    Name = client.Surname,
        //                    City = adress.City,
        //                    Line1 = adress.Town,
        //                    Line2 = adress.Street,
        //                    Line3 = adress.HouseNumber.ToString(),
        //                    Zip = adress.PostCode,


        //                });
        //            }
        //            else
        //            {
        //                return View(new ShippingDetails());
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Login", "Account");
        //        }
        //}

        [HttpPost]
        public async Task<ActionResult> ShippDetail(Cart cart, ShippingDetails shippingDetails, int Id)
        {


            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                decimal price = cart.ComputeTotalValue();
               

                List<int> albumsId = new List<int>();
                foreach (var line in cart.Lines)
                {
                    albumsId.Add(line.Product.album.Id);
                }
                if (albumsId.Count > 0 && Id >= 1)
                {
                    var clientId = await clientRepository.GetClient(Id);
                    await clientRepository.AddAlbumsToLibrary(albumsId, clientId.Id);
                    await orderProcessor.NewOrder(clientId.Id, albumsId,price);
                }

                cart.Clear();
                return View("Completed");


            }
            else
            {
                 return View("Checkout", new CartIndexViewModel
                {
                    Cart = cart,
                    CartCount = cart.Lines.Count(),
                    ShippingDetails = shippingDetails

                });
            }
        }

    }
}