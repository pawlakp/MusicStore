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
    public class EFClientRepository: IClientRepository
    {
        private EfDbContext context = new EfDbContext();

      
        public async Task<List<Client>> AllClientsAsync() => await context.Client.ToListAsync().ConfigureAwait(false);
        public async Task<List<Adress>> AllAdressesAsync() => await context.Adresses.ToListAsync().ConfigureAwait(false);
        public async Task<List<Order>> AllOrdersAsync() => await context.Order.ToListAsync().ConfigureAwait(false);
        public async Task<List<OrderAlbum>> AllOrderDetailsAsync() => await context.OrdersAlbums.ToListAsync().ConfigureAwait(false);

        public async Task<List<ClientLibrary>> AllClientLibrariesAsync() => await context.ClientLibrary.ToListAsync().ConfigureAwait(false);

       

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
            Adress adress = list.Where(x => x.ClientId == id).FirstOrDefault();
            return adress;
        }

        public async Task AddMusicToLibrary(List<int> albumsId, int clientId)
        {
              foreach(var albumId in albumsId)
            {
                ClientLibrary nowy = new ClientLibrary()
                {
                    AlbumId = albumId,
                    ClientId = clientId,
                };

                context.ClientLibrary.Add(nowy);
            }
              context.SaveChanges();
        }

        public async Task<List<int>> GetClientLibraryAsync(int id)
        {
            var libraries = await AllClientLibrariesAsync();


            var productRecord = from e in libraries
                                where e.ClientId == id
                                select e.AlbumId;

            

            return productRecord.ToList();
        }

        public async Task<Cart> ClientHaveAlbum(int clientId,Cart cart)
        {
            //var libraries = await AllClientLibrariesAsync();
            //var have = libraries.Where(x=> x.ClientId== clientId && x.AlbumId==albumId).FirstOrDefault();
            //if (have != null)
            //    return true;
            //else
            //    return false;
            
            var libraries = await AllClientLibrariesAsync();
            var clientLibrary = libraries.Where(x=> x.ClientId == clientId).ToList();
            Cart nowy = cart;
            foreach(var koszyk in cart.Lines.ToList())
            {
                foreach (var album in clientLibrary)
                {
                    if(album.AlbumId == koszyk.Product.album.Id)
                    {
                        nowy.RemoveLine(koszyk.Product);
                    }
                }
            }
            return nowy;
         
                   
        }

       public async Task<List<Order>> GetAllClientOrders(int clientId)
        {
            var libraries = await AllOrdersAsync();
            var list = libraries.Where(x=> x.ClientId == clientId).ToList();

            return list;
        }

        public async Task<List<OrderAlbum>> GetOrderDetails(int orderId)
        {
            var libraries = await AllOrderDetailsAsync();
            var list = libraries.Where(x => x.OrderId == orderId).ToList();

            return list;
        }

     




    }
}
