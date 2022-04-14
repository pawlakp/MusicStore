using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using System.Data.Entity;
using System.Threading;
using System.Data.Entity.Infrastructure;


namespace MusicStore.Domain.Concrete
{
    public class EFAccountsRepository: IAccountsRepository
    {
        private EfDbContext context = new EfDbContext();

        public IEnumerable<Accounts> Accounts { get => context.Account; }
        public async Task<List<Accounts>> AllAccountsAsync() => await context.Account.ToListAsync().ConfigureAwait(false);
        public async Task<List<Client>> AllClientsAsync() => await context.Client.ToListAsync().ConfigureAwait(false);
        public async Task<List<Adress>> AllAdressesAsync() => await context.Adresses.ToListAsync().ConfigureAwait(false);


        public async Task AddAccount(Accounts account)
        {
                
               // var pass = GetMD5(account.Password);
                account.Password = GetMD5(account.Password);
                context.Account.Add(account);
                await context.SaveChangesAsync();              
        }

        public async Task<Accounts> LoginAsync(string name, string password)
        {
            var db = await AllAccountsAsync();
            //var pass = GetMD5(password);
            var user =  db.Where(s => s.Login == name && s.Password == GetMD5(password)).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<Accounts> GetAccountAsync(int id)
        {
            var db = await AllAccountsAsync();
            //var pass = GetMD5(password);
            var user = db.Where(s => s.Id == id).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task DeleteUser(int id)
        {
            var user = await context.Account.FindAsync(id);
            if (user != null)
            {
                context.Account.Remove(user);
            }
            
            await context.SaveChangesAsync();
        }
        public async Task ChangePassword(int id)
        {
            var user = await context.Account.FindAsync(id);
            if (user != null)
            {
                user.IsPasswordChangeRequired= true;    
            }

            await context.SaveChangesAsync();
        }


        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }


        public async Task<bool> IsClientExist(int id)
        {
            var list = await AllClientsAsync();
            var client = list.Where(x => x.AccountId == id).FirstOrDefault();
            if (client == null) return false;
            else return true;
            
        }

        public async Task<Client> GetClient(Client user)
        {
            var list = await AllClientsAsync();
            var client = list.Where(x => x.FirstName == user.FirstName && x.Surname == user.Surname).FirstOrDefault();
            return client;
        }

        public async Task<Client> GetClient(int id)
        {
            var list = await AllClientsAsync();
            var client = list.Where(x => x.AccountId == id).FirstOrDefault();
            return client;
        }

        public async Task CreateAccount(Client client)
        {
            context.Client.Add(client);
            await context.SaveChangesAsync();
        }

        public async Task CreateAdress(Adress adress)
        {
            context.Adresses.Add(adress);
            await context.SaveChangesAsync();
        }
        public async Task EditAdress(Adress adress)
        {
            var newAdress = context.Adresses.FirstOrDefault(x => x.Id == adress.Id);

            newAdress.Id = adress.Id;
            newAdress.ClientId = adress.ClientId;
            newAdress.City = adress.City;
            newAdress.Town = adress.Town;
            newAdress.PostCode = adress.PostCode;
            newAdress.Street = adress.Street;
            newAdress.HouseNumber = adress.HouseNumber;
            newAdress.ApartamentNumber = adress.ApartamentNumber;


            await context.SaveChangesAsync();
        }

        public async Task<Adress> GetAdressesAsync(int id)
        {
            var list = await AllAdressesAsync();
            var adress = list.Where(x => x.ClientId == id).FirstOrDefault();
            return adress;
        }


    }
}
