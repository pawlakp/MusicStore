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
using System.Web.Security;

namespace MusicStore.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IAccountRepository repository;

        public AccountController(IAccountRepository repo)
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
        public async Task<ActionResult> Register(RegisterModelDto account)
        {
            var apiModel = await repository.AllAccountsAsync();
            if (ModelState.IsValid)
            {
                var check =  apiModel.FirstOrDefault(s => s.Login == account.Login);
                if (check == null)
                {
                    Accounts tmp = new Accounts
                    {
                        Login = account.Login,
                        Password = account.Password,
                        IsPasswordChangeRequired = account.IsPasswordChangeRequired,
                        IsAdmin = account.IsAdmin,
                    };
                    await repository.AddAccount(tmp);
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

        [HttpPost, ActionName("Login")]
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
                    if (data.IsAdmin == true)
                    {
                        FormsAuthentication.SetAuthCookie(data.Login, false);
                        return RedirectToAction("Index", "Admin", data);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(data.Login, false);
                        return RedirectToAction("AfterLogin", "Client", data);
                    }
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
            var user = await repository.GetAccountAsync(id);
            if (user.IsAdmin) return RedirectToAction("Index", "Admin", user);
            else return RedirectToAction("Index", "Client", user);
        }



        public ActionResult Logout()
        {
            Session.Clear();//remove session
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public PartialViewResult Welcome()
        {
            return PartialView();
        }

    }
}