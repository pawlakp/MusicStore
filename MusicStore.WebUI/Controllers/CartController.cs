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
        private IOrderProcessor orderProcessor;

        public CartController(IProductsRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
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


        public async Task<ViewResult> Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

    }
}