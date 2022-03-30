using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Moq;
using Ninject;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Concrete;
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
          
            kernel.Bind<IProductsRepository>().To<EFProjectRepository>();
            
            //wstrzyknięcie  EmailSettings do konstruktora EmailOrderProcessor gdy jest tworzony nowy egzemplarz 
            //w odpowiedzi na żądanie interfejsu IOrderProcessor
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
        }
    }
}