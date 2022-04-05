using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MusicStore.Domain.Entities;
using System.Linq;
using Moq;
using MusicStore.Domain.Abstract;
using MusicStore.WebUI.Controllers;
using System.Web.Mvc;
using MusicStore.WebUI.Models;
using System.Threading.Tasks;

namespace MusicStore.UnitTests
{

    //testy jednostkowe koszyka
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //przygotowanie produktów testowych
            Album album1 = new Album { AlbumId = 1, Name = "A1" };
            Album album2 = new Album { AlbumId = 2, Name = "A2" };

            AlbumAllDetails albumAllDetails1 = new AlbumAllDetails { album = album1 };
            AlbumAllDetails albumAllDetails2 = new AlbumAllDetails { album = album2 };



            //utworzenie koszyka
            Cart cart = new Cart();

            //działanie
            cart.AddItem(albumAllDetails1);
            cart.AddItem(albumAllDetails2);
            cart.AddItem(albumAllDetails1);
            CartLine[] results = cart.Lines.ToArray();

            //asert

            Assert.AreEqual(2, 2);
            Assert.AreEqual(results[0].Product.album, album1);
            Assert.AreEqual(results[1].Product.album, album2);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            //przygotowanie produktów testowych
            Album album1 = new Album { AlbumId = 1, Name = "A1" };
            Album album2 = new Album { AlbumId = 2, Name = "A2" };
            Album album3 = new Album { AlbumId = 3, Name = "A3" };

            AlbumAllDetails albumAllDetails1 = new AlbumAllDetails { album = album1 };
            AlbumAllDetails albumAllDetails2 = new AlbumAllDetails { album = album2 };
            AlbumAllDetails albumAllDetails3 = new AlbumAllDetails { album = album3 };



            //utworzenie koszyka
            Cart cart = new Cart();

            //działanie
            cart.AddItem(albumAllDetails1);
            cart.AddItem(albumAllDetails2);
            cart.AddItem(albumAllDetails3);
            
            cart.RemoveLine(albumAllDetails3);
            //asert

            Assert.AreEqual(cart.Lines.Where(c => c.Product == albumAllDetails3).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
         
 
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //przygotowanie produktów testowych
            Album album1 = new Album { AlbumId = 1, Name = "A1", Price =10M };
            Album album2 = new Album { AlbumId = 2, Name = "A2", Price = 20M };
            Album album3 = new Album { AlbumId = 3, Name = "A3", Price = 30M };

            AlbumAllDetails albumAllDetails1 = new AlbumAllDetails { album = album1 };
            AlbumAllDetails albumAllDetails2 = new AlbumAllDetails { album = album2 };
            AlbumAllDetails albumAllDetails3 = new AlbumAllDetails { album = album3 };



            //utworzenie koszyka
            Cart cart = new Cart();

            //działanie
            cart.AddItem(albumAllDetails1);
            cart.AddItem(albumAllDetails2);
            cart.AddItem(albumAllDetails3);
            decimal result = cart.ComputeTotalValue();

            //asert
            Assert.AreEqual(result, 60M);

        }

        [TestMethod]
        public void Can_Clear_Cart()
        {
            Album album1 = new Album { AlbumId = 1, Name = "A1", Price = 10M };
            Album album2 = new Album { AlbumId = 2, Name = "A2", Price = 20M };
            Album album3 = new Album { AlbumId = 3, Name = "A3", Price = 30M };

            AlbumAllDetails albumAllDetails1 = new AlbumAllDetails { album = album1 };
            AlbumAllDetails albumAllDetails2 = new AlbumAllDetails { album = album2 };
            AlbumAllDetails albumAllDetails3 = new AlbumAllDetails { album = album3 };



            //utworzenie koszyka
            Cart cart = new Cart();

            //działanie
            cart.AddItem(albumAllDetails1);
            cart.AddItem(albumAllDetails2);
            cart.AddItem(albumAllDetails3);
            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //przygotowanie
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            //przygotowanie
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController target = new CartController(null, mock.Object);
            ViewResult result = target.Checkout(cart, shippingDetails);

            mock.Verify(m=> m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

    }
}
