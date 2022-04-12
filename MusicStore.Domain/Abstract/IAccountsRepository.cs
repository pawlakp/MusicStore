using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.Abstract
{
    public interface IAccountsRepository
    {
        //interfejs kont użytkowników 
        IEnumerable<Accounts> Accounts { get;}

        Task<List<Accounts>> GetAllAsync();
        Task<Accounts> GetAsync(int id);
        Task<Accounts> LoginAsync(string name, string password);
        Task AddAccount(Accounts account);
        Task DeleteUser(int id);
        Task ChangePassword(int id);



    }
}
