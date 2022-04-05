using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using System.Data.Entity;

namespace MusicStore.Domain.Concrete
{
    public class EFAccountsRepository: IAccountsRepository
    {
        private EfDbContext context = new EfDbContext();

        public IEnumerable<Accounts> Accounts { get => context.Accounts; }
        public async Task<List<Accounts>> GetAllAsync() => await context.Accounts.ToListAsync().ConfigureAwait(false);
       
        
        public async void AddAccount(Accounts account)
        {
                
               // var pass = GetMD5(account.Password);
                account.Password = GetMD5(account.Password);
                context.Accounts.Add(account);
                await context.SaveChangesAsync().ConfigureAwait(false);               
        }

        public async Task<Accounts> LoginAsync(string name, string password)
        {
            var db = await GetAllAsync();
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

        public async Task<Accounts> GetAsync(int id)
        {
            var db = await GetAllAsync();
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


    }
}
