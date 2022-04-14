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
    [Authorize]
    public class ClientController : Controller
    {
        private IAccountsRepository repository;
        // GET: Admin

        public ClientController(IAccountsRepository repo)
        {
            this.repository = repo;
        }
        public async Task<ActionResult> Index(int id)
        {
            var list = await repository.AllClientsAsync();
            Client client = list.Where(c => c.Id == id).FirstOrDefault();
            return View(client);
        }
        public async Task<ActionResult> AfterLogin(Accounts account)
        {
            if (!await repository.IsClientExist(account.Id)) return RedirectToAction("Create", account);
            else
            {
                Client client = await repository.GetClient(account.Id);
                return View("Index",client);
            }
        }

        public async Task<ActionResult> Create(Accounts account)
        {
            Client client = new Client()
            {
                AccountId = account.Id,
            };
            return View(client);

        }

        [HttpPost]
        public async Task<ActionResult> Create(Client client)
        {
            await repository.CreateAccount(client);
            return RedirectToAction("AddAdress",client);
        }

        public async Task<ActionResult> AddAdress(Client user)
        {
            var client = await repository.GetClient(user);
            Adress adress = new Adress()
            {
                ClientId = client.Id,
            };
            return View(adress);
        }

        [HttpPost]
        public async Task<ActionResult> AddAdress(Adress adress)
        {
            await repository.CreateAdress(adress);
            return View("Index");
        }

        public async Task<ActionResult> DetailsAdress(int id)
        {
            return View(await repository.GetAdressesAsync(id));
        }
        
        public async Task<ActionResult> EditAdress(int id)
        {
            return View(await repository.GetAdressesAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> EditAdress(Adress adress)
        {
            await repository.EditAdress(adress);
            return View("DetailsAdress", adress);
        }


    }
}