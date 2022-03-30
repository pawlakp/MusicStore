using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.WebUI.Controllers;
using MusicStore.WebUI.Models;
using MusicStore.WebUI.HtmlHelpers;

namespace MusicStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Stronicowanie()
        {
            //przygotowanie testu

            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Album).Returns(new Album[]
            {
                new Album{AlbumId = 1, Name="P1"},
                new Album{AlbumId = 2, Name="P2"},
                new Album{AlbumId = 3, Name="P3"},
                new Album{AlbumId = 4, Name="P4"},
                new Album{AlbumId = 5, Name="P5"}
            });

            AlbumController albumController = new AlbumController(mock.Object);
            albumController.PageSize = 3;

            //test

            AlbumListViewModel result = (AlbumListViewModel)albumController.List(2);

            Album[] albumArray = result.Albums.ToArray();
            Assert.IsTrue(albumArray.Length == 2);
            Assert.AreEqual(albumArray[0].Name, "P4");
            Assert.AreEqual(albumArray[1].Name, "P5");
        }

        [TestMethod]
        public void Generowanie_Linku()
        {
            //przygotowanie

            HtmlHelper myHelper = null;

            //tworzenie PagingInfo

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            //konfigurowanie delegatu 
            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            //działanie
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //test

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>" 
            + @"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Strona3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void StronicowanieModelu()
        {
            //przygotowanie
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Album).Returns(new Album[]
            {
            new Album{ AlbumId = 1, Name = "A1"},
            new Album{ AlbumId = 2, Name = "A2"},
            new Album{ AlbumId = 3, Name = "A3"},
            new Album{ AlbumId = 4, Name = "A4"},
            new Album{ AlbumId = 5, Name = "A5"}
            });

            AlbumController controller = new AlbumController(mock.Object)
            {
                PageSize = 3
            };

            //działanie
            AlbumListViewModel result = (AlbumListViewModel)controller.List(2);

            //asercje
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);


        }


    }

   
}
