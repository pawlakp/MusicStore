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
    public class ClientController : Controller
    {
        private IAccountsRepository repository;
        // GET: Admin

        public ClientController(IAccountsRepository repo)
        {
            this.repository = repo;
        }
        public string Index(Accounts accounts)
        {
            return "To jest klient: "+accounts.Login;
        }
    }
}