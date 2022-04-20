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
    [TestClass]
    public class AlbumTest
    {
        [TestMethod]
        public async Task Can_AddAlbum()
        {
            
            
            Album album1 = new Album
            {
                Name = "Franek",
                Id = 1500

            };
            Artist artist1 = new Artist
            {
                Name = "Baby",
                ArtistId = 1500
            };
            AlbumAllDetails product = new AlbumAllDetails { album = album1, artist = artist1 };
            AlbumAllDetails pusty = null;
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(t => t.AddAlbumAsync(product)).Returns(Task.FromResult(product));
            await mock.Object.AddAlbumAsync(product);
            mock.Verify(x => x.AddAlbumAsync(It.Is<AlbumAllDetails>(t => t != null)));
       

        }
    }
}
