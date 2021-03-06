using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        //interfejs koszyka
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
        Task NewOrder(int clientId, List<int> albumsId, decimal price);
        Task<List<Order>> AllOrdersAsync();
        Task<List<OrderAlbum>> AllOrdersAlbumAsync();
        Task<decimal> GetAllMoneyEarned();
    }
}
