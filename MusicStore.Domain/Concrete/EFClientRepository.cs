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
using System.Web.Mvc;

namespace MusicStore.Domain.Concrete
{
    public class EFClientRepository : IClientRepository
    {
        private EfDbContext context = new EfDbContext();


        public async Task<List<Client>> AllClientsAsync() => await context.Client.ToListAsync().ConfigureAwait(false);
        public async Task<List<Adress>> AllAdressesAsync() => await context.Adresses.ToListAsync().ConfigureAwait(false);
        public async Task<List<Order>> AllOrdersAsync() => await context.Order.ToListAsync().ConfigureAwait(false);
        public async Task<List<OrderAlbum>> AllOrderDetailsAsync() => await context.OrdersAlbums.ToListAsync().ConfigureAwait(false);

        public async Task<List<ClientLibrary>> AllClientLibrariesAsync() => await context.ClientLibrary.ToListAsync().ConfigureAwait(false);
        public async Task<List<ClientWishlist>> AllClientWishlistAsync() => await context.ClientWishlist.ToListAsync().ConfigureAwait(false);


        public async Task<bool> IsClientExist(int id)
        {
            //var list = await AllClientsAsync();
            var client = await context.Client.Where(x => x.AccountId == id).FirstOrDefaultAsync();
            if (client == null) return false;
            else return true;

        }

        public async Task<Client> GetClient(Client user)
        {
            //var list = await AllClientsAsync();
            var client = await context.Client.Where(x => x.FirstName == user.FirstName && x.Surname == user.Surname).FirstOrDefaultAsync();
            return client;
        }
        public async Task<Client> GetClientById(int id)
        {
            //var list = await AllClientsAsync();
            var client = await context.Client.Where(x => x.Id == id).FirstOrDefaultAsync();
            return client;
        }
        public async Task<Client> GetClientByAccountId(int id)
        {
            //var list = await AllClientsAsync();
            var client = await context.Client.Where(x => x.AccountId == id).FirstOrDefaultAsync();
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
            var newAdress = await context.Adresses.FirstOrDefaultAsync(x => x.Id == adress.Id);

            newAdress.Id = adress.Id;
            newAdress.ClientId = adress.ClientId;
            newAdress.City = adress.City;
            newAdress.Town = adress.Town;
            newAdress.PostCode = adress.PostCode;
            newAdress.Street = adress.Street;
            newAdress.HouseNumber = adress.HouseNumber;
            newAdress.ApartamentNumber = adress.ApartamentNumber;
            newAdress.State = adress.State;
            newAdress.Country = adress.Country;


            await context.SaveChangesAsync();
        }

        public async Task<Adress> GetAdressesAsync(int id)
        {
            //var list = await AllAdressesAsync();
            Adress adress = await context.Adresses.Where(x => x.ClientId == id).FirstOrDefaultAsync();
            return adress;
        }

        public async Task AddAlbumsToLibrary(List<int> albumsId, int clientId)
        {
            foreach (var albumId in albumsId)
            {
                ClientLibrary nowy = new ClientLibrary()
                {
                    AlbumId = albumId,
                    ClientId = clientId,
                };

                context.ClientLibrary.Add(nowy);
            }
            await context.SaveChangesAsync();
        }

        public async Task AddAlbumToLibrary(Album album, int clientId)
        {

            ClientLibrary nowy = new ClientLibrary()
            {
                AlbumId = album.Id,
                ClientId = clientId,
            };

            context.ClientLibrary.Add(nowy);

            await context.SaveChangesAsync();
        }

        public async Task<List<int>> GetClientLibraryAsync(int id)
        {
            //var libraries = await AllClientLibrariesAsync();


            var productRecord = from e in context.ClientLibrary
                                where e.ClientId == id
                                select e.AlbumId;



            return await productRecord.ToListAsync();
        }

        public async Task<List<int>> GetClientWishlist(int id)
        {
            //var libraries = await AllClientWishlistAsync();


            var productRecord = from e in context.ClientWishlist
                                where e.ClientId == id
                                select e.AlbumId;



            return await productRecord.ToListAsync();
        }

        public async Task<Cart> ClientHaveAlbum(int clientId, Cart cart)
        {
            //var libraries = await AllClientLibrariesAsync();
            //var have = libraries.Where(x=> x.ClientId== clientId && x.AlbumId==albumId).FirstOrDefault();
            //if (have != null)
            //    return true;
            //else
            //    return false;

            //var libraries = await AllClientLibrariesAsync();
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync();
            Cart nowy = cart;
            foreach (var koszyk in cart.Lines.ToList())
            {
                foreach (var album in clientLibrary)
                {
                    if (album.AlbumId == koszyk.Product.album.Id)
                    {
                        nowy.RemoveLine(koszyk.Product);
                    }
                }
            }
            return nowy;


        }

        public async Task<List<Order>> GetAllClientOrders(int clientId)
        {
            //var libraries = await AllOrdersAsync();
            var list = await context.Order.Where(x => x.ClientId == clientId).ToListAsync();

            return list;
        }

        public async Task<List<OrderAlbum>> GetOrderDetails(int orderId)
        {
            //var libraries = await AllOrderDetailsAsync();
            var list = await context.OrdersAlbums.Where(x => x.OrderId == orderId).ToListAsync();

            return list;
        }

        public async Task AddAlbumToClientLibrary(int albumId, int clientId)
        {
            ClientLibrary newAdd = new ClientLibrary
            {
                AlbumId = albumId,
                ClientId = clientId
            };

            context.ClientLibrary.Add(newAdd);
            await context.SaveChangesAsync();
        }

        public async Task<bool> AddToClientWishlist(int clientId, int albumId)
        {
           
            //var clientWishlist = await AllClientWishlistAsync();
            var isExist = await context.ClientWishlist.Where(x => x.ClientId == clientId && x.AlbumId ==albumId).FirstOrDefaultAsync();
            if (isExist!=null)
            {
               
                //context.Entry(newAdd).State = EntityState.Modified;
                context.ClientWishlist.Remove(isExist);
                await context.SaveChangesAsync();
                return false;
            }
            else
            {
                ClientWishlist newAdd = new ClientWishlist
                {
                    AlbumId = albumId,
                    ClientId = clientId
                };
                context.ClientWishlist.Add(newAdd);
                await context.SaveChangesAsync();
                return true;
            }
           

           


        }
    }
}
