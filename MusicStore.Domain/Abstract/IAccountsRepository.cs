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

        Task<List<Accounts>> AllAccountsAsync();
        Task<Accounts> GetAccountAsync(int id);
        Task<Accounts> LoginAsync(string name, string password);
        Task<List<Client>> AllClientsAsync();
        Task AddAccount(Accounts account);
        Task DeleteUser(int id);
        Task ChangePassword(int id);
        Task<bool> IsClientExist(int id);
        Task CreateAccount(Client client);
        Task<Client> GetClient(Client user);
        Task<Client> GetClient(int id);
        Task CreateAdress(Adress adress);
        Task EditAdress(Adress adress);
        Task<Adress> GetAdressesAsync(int id);
        Task<List<Adress>> AllAdressesAsync();


    }
}
