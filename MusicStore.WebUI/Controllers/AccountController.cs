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
    public class AccountController : Controller
    {
        private IAccountsRepository repository;

        public AccountController(IAccountsRepository repo)
        {
            this.repository = repo;
        }
        // GET: Account
        public ActionResult Index()
        {
            if(Session["Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Accounts account)
        {
            var apiModel = await repository.GetAllAsync();
            if (ModelState.IsValid)
            {
                var check =  apiModel.FirstOrDefault(s => s.Login == account.Login);
                if (check == null)
                {
                    repository.AddAccount(account);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();

        }

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string login, string password)
        {
            if (ModelState.IsValid)
            {
               
                var data =  await repository.LoginAsync(login, password);
                if (data!=null)
                {
                    //add session
                    Session["Login"] = data.Login;
                    Session["Id"] = data.Id;
                    return View("UserPanel",data);
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        public async Task<ActionResult> UserPanel(int id)
        {
            return View(await repository.GetAsync(id));
        }


        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public PartialViewResult Welcome()
        {
            return PartialView();
        }

    }
}