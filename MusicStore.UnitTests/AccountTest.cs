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
            var b = await mock.Object.LoginAsync("", "");

            //test
            Assert.IsNull(b);

        }
        [TestMethod]
        public void Can_Get_Account()
        {
            //przygotowanie
            Mock<IAccountsRepository> mock = new Mock<IAccountsRepository>();
            Accounts accounts = new Accounts
           {
               Id = 10,
               Login = "User1"
           };
            mock.Object.AddAccount(accounts);
            var b = mock.Object.GetAsync(10);


            //Test pobrania konta
            Assert.IsNotNull(b);

        }
        [TestMethod]
        public async Task Can_Get_Account_DoesntExist()
        {
            //przygotowanie
            Mock<IAccountsRepository> mock = new Mock<IAccountsRepository>();
            //Accounts accounts = new Accounts
            //{
            //    Id = 10,
            //    Login = "User1"
            //};
            
            var b = await mock.Object.GetAsync(11);


            //Test pobrania konta
            Assert.IsNull(b);

        }


    }
}
