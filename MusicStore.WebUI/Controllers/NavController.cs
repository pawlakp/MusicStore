using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MusicStore.Domain.Abstract;

namespace MusicStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository repository; 

        public NavController(IProductsRepository repo)
        {
            this.repository = repo; 
        }

        public PartialViewResult Menu(string genre)
        {
            ViewBag.SelectedGenre = genre;
            return PartialView(repository.GetGenre());
        }
        public PartialViewResult MenuAdmin()
        {
            return PartialView();
        }

    }
}