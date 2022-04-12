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
    public class AccountTest
    {
      
        [TestMethod]
        public async Task Can_LoginWithoutDataAsync()
        {
            //przygotowanie
            Mock<IAccountsRepository> mock = new Mock<IAccountsRepository>();
            Accounts user = new Accounts()
            {
                Login = "admin",
                Password = "admin"
            };
            var b = await mock.Object.LoginAsync("", "");
            //test
            Assert.IsNull(b);

        }
        [TestMethod]
        public async Task Can_Get_Account()
        {
            //przygotowanie
            Mock<IAccountsRepository> mock = new Mock<IAccountsRepository>();
            Accounts user = new Accounts
           {
                Id=1,
               Login = "admin",
               Password= "admin"
           };
            mock.Setup(x => x.GetAsync(user.Id)).Returns(Task.FromResult(user));  
            
            var c = await mock.Object.GetAsync(1);

            Assert.AreEqual(user,c);

        }
        [TestMethod]
        public async Task Can_Get_Account_DoesntExist()
        {
            //przygotowanie
            Mock<IAccountsRepository> mock = new Mock<IAccountsRepository>();
            Accounts accounts = new Accounts
            {
                Id = 10,
                Login = "User1",
                Password = "User1"
            };
            mock.Setup(x=> x.LoginAsync(accounts.Login,accounts.Password)).Returns(Task.FromResult(accounts));
            mock.Setup(x => x.GetAsync(10)).Returns(Task.FromResult(accounts));
            var b = await mock.Object.GetAsync(11);

            //Test pobrania konta
            Assert.IsNull(b);

        }


    }
}
