using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Abstract;

namespace MusicStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            this.kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mock<IAlbumRepository> mock = new Mock<IAlbumRepository>();
            mock.Setup(m => m.Album).Returns(new List<Album>
            {
                new Album { Name = "Egzotyka", Price = 25},
                new Album {Name = "Ezoteryka", Price = 50}
            });

            kernel.Bind<IAlbumRepository>().ToConstant(mock.Object);
        }
    }
}