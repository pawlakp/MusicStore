using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.Abstract
{
    public interface IClientRepository
    {
        Task<List<Client>> AllClientsAsync();
        Task<bool> IsClientExist(int id);
        Task CreateAccount(Client client);
        Task<Client> GetClient(Client user);
        Task<Client> GetClient(int id);
        Task CreateAdress(Adress adress);
        Task EditAdress(Adress adress);
        Task<Adress> GetAdressesAsync(int id);
        Task<List<Adress>> AllAdressesAsync();
        Task AddAlbumsToLibrary(List<int> albumsId, int id);
        Task<List<int>> GetClientLibraryAsync(int id);
        Task<Cart> ClientHaveAlbum(int clientId, Cart cart);
        Task<List<Order>> GetAllClientOrders(int clientId);

        Task<List<OrderAlbum>> GetOrderDetails(int orderId);
        Task AddAlbumToClientLibrary(int albumId, int clientId);

        }
}
