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

        //public async Task<List<ClientPreferences>> GetClientPreferences(int clientId)
        //{
        //    var b = await GetClientPreferencesGenre(clientId);

        //    return b;

        //    //var LibraryPrefernce = await GetClientPreferencesGenreLibrary(clientId);
        //    //var WishlistPrefernce = await GetClientPreferencesGenreWishlist(clientId);

        //    //List<ClientPreferences> newPrefe = new List<ClientPreferences>();

        //    //double all = 0;
        //    //foreach (var item in LibraryPrefernce.OrderBy(x=>x.Name))
        //    //{
        //    //    double tmp = 0;
        //    //    foreach (var item2 in WishlistPrefernce)
        //    //    {
        //    //        if (item.Name == item2.Name)
        //    //        {
        //    //            tmp = (double)(item.Id + item2.Id * 0.25);
        //    //        }
        //    //    }
        //    //    all += tmp;
        //    //}

        //    //foreach (var item in LibraryPrefernce)
        //    //{
        //    //    double tmp = 0;
        //    //    foreach (var item2 in WishlistPrefernce)
        //    //    {
        //    //        if (item.Name == item2.Name)
        //    //        {
        //    //            tmp = (double)(item.Id + item2.Id * 0.25);
        //    //        }
        //    //    }
        //    //    var newGenre = await context.Genre.Where(x=> x.Name == item.Name).FirstOrDefaultAsync();  
        //    //    ClientPreferences preferences = new ClientPreferences()
        //    //    {
        //    //        //wyst = tmp,
        //    //        //Name = item.Name,
        //    //        genreAppearances = (tmp*100/all),
        //    //        genre = newGenre,
        //    //    };
        //    //    newPrefe.Add(preferences);

        //    //}





        //    //return newPrefe;


        //}


        public async Task<List<ClientGenrePrefences>> GetClientPreferences(int clientId)
        {


            var clientWishlist = await context.ClientWishlist.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie listy życzeń klienta
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie biblioteki klienta
           
            
            var albumList = await context.Album.ToListAsync(); // lista wszystkich albumów
            var genresList = await context.Genre.ToListAsync(); //lista wszystkich gatunków
            var artistList = await context.Artist.ToListAsync(); //lista wszystkich artystów
            
            
            var listAlbumsWishlist = from x in clientWishlist
                             join y in albumList on x.AlbumId equals y.Id
                             select y; // pobranie detalów albumów z listy życzeń

            var listAlbumsLibrary = from x in clientLibrary
                                    join y in albumList on x.AlbumId equals y.Id
                                     select y;  // pobranie detalów albumów z biblioteki


            //preferowane gatunki
            List<Genre> genreWishlist = new List<Genre>();
            List<Genre> genreLibrary = new List<Genre>();

            //preferowani artyści
            //List<Artist> artistWishlist = new List<Artist>();
            //List<Artist> artistLibrary = new List<Artist>();
          

            //int liczbaWystąpień = 0;
            int liczbaWystąpieńLib = 0;
            int liczbaWystąpieńWis = 0;
            double allGenre = 0;
            //double allArtists = 0;

            //liczenie gatunków
            foreach (var item in genresList)
            {
                liczbaWystąpieńLib = 0;
                liczbaWystąpieńWis = 0;


                foreach (var item2 in listAlbumsWishlist)
                {
                    if (item.Id == item2.GenreId)
                    {
                        liczbaWystąpieńWis++;
                    }
                }


                Genre genreWish = new Genre()
                {
                    Id = liczbaWystąpieńWis,
                    Name = item.Name,
                };


                genreWishlist.Add(genreWish);

                foreach (var item2 in listAlbumsLibrary)
                {

                    if (item.Id == item2.GenreId)
                    {
                        liczbaWystąpieńLib++;
                    }
                }

                Genre genreLib = new Genre()
                {
                    Id = liczbaWystąpieńLib,
                    Name = item.Name,
                };

                genreLibrary.Add(genreLib);

                allGenre += (double)liczbaWystąpieńLib + liczbaWystąpieńWis * 0.25;

            }

            //liczenie artystów
            //foreach (var item in artistList)
            //{
            //    liczbaWystąpieńLib = 0;
            //    liczbaWystąpieńWis = 0;


            //    foreach (var item2 in listAlbumsWishlist)
            //    {
            //        if (item.ArtistId == item2.ArtistId)
            //        {
            //            liczbaWystąpieńWis++;
            //        }
            //    }


            //    Artist artistWish = new Artist()
            //    {
            //        ArtistId = liczbaWystąpieńWis,
            //        Name = item.Name,
            //    };


            //    artistWishlist.Add(artistWish);

            //    foreach (var item2 in listAlbumsLibrary)
            //    {

            //        if (item.ArtistId == item2.ArtistId)
            //        {
            //            liczbaWystąpieńLib++;
            //        }
            //    }

            //    Artist artistLib = new Artist()
            //    {
            //        ArtistId = liczbaWystąpieńLib,
            //        Name = item.Name,
            //    };

            //    artistLibrary.Add(artistLib);

            //    allArtists += (double)liczbaWystąpieńLib + liczbaWystąpieńWis * 0.25;

            //}



            //foreach (var item in genresList)
            //{
            //    liczbaWystąpień = 0;
            //    foreach (var item2 in listAlbums)
            //    {

            //        if (item.Id == item2.GenreId)
            //        {
            //            liczbaWystąpień++;
            //        }
            //    }
            //    Genre genre = new Genre()
            //    {
            //        Id = liczbaWystąpień,
            //        Name = item.Name,
            //    };
            //    preferencesLibrary.Add(genre);
            //}



            List<ClientGenrePrefences> clientPrefe = new List<ClientGenrePrefences>();

            //double all = 0;
            //foreach (var item in preferencesLibrary.OrderBy(x => x.Name))
            //{
            //    double tmp = 0;
            //    foreach (var item2 in preferencesWishlist)
            //    {
            //        if (item.Name == item2.Name)
            //        {
            //            tmp = (double)(item.Id + item2.Id * 0.25);
            //        }
            //    }
            //    all += tmp;
            //}

            foreach (var item in genreLibrary)
            {
                double tmpGenre = 0;
                foreach (var item2 in genreWishlist)
                {
                    if (item.Name == item2.Name)
                    {
                        tmpGenre = (double)(item.Id + item2.Id * 0.25);
                    }
                }
                if(tmpGenre > 0)
                {

                
                var newGenre = await context.Genre.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
                    ClientGenrePrefences preferences = new ClientGenrePrefences()
                {
                    //wyst = tmp,
                    //Name = item.Name,
                    genreAppearances = (tmpGenre * 100 / allGenre),
                    genre = newGenre,
                };
                clientPrefe.Add(preferences);
                }
            }


            //foreach (var item in artistLibrary)
            //{
            //    double tmpGenre = 0;
            //    foreach (var item2 in artistWishlist)
            //    {
            //        if (item.Name == item2.Name)
            //        {
            //            tmpGenre = (double)(item.ArtistId + item2.ArtistId * 0.25);
            //        }
            //    }
            //    var newArtist = await context.Artist.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
            //    ClientGenrePreferences preferences = new ClientGenrePreferences()
            //    {
            //        //wyst = tmp,
            //        //Name = item.Name,
            //        artistAppearances = (tmpGenre * 100 / allArtists),
            //        artist = newArtist,
            //    };
            //    clientPrefe.Add(preferences);

            //}


            return clientPrefe;
        }


        public async Task<List<ClientArtistPreferences>> GetClientArtistPreferences(int clientId)
        {


            var clientWishlist = await context.ClientWishlist.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie listy życzeń klienta
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie biblioteki klienta


            var albumList = await context.Album.ToListAsync(); // lista wszystkich albumów
            var artistList = await context.Artist.ToListAsync(); //lista wszystkich artystów


            var listAlbumsWishlist = from x in clientWishlist
                                     join y in albumList on x.AlbumId equals y.Id
                                     select y; // pobranie detalów albumów z listy życzeń

            var listAlbumsLibrary = from x in clientLibrary
                                    join y in albumList on x.AlbumId equals y.Id
                                    select y;  // pobranie detalów albumów z biblioteki


            

            //preferowani artyści
            List<Artist> artistWishlist = new List<Artist>();
            List<Artist> artistLibrary = new List<Artist>();


            //int liczbaWystąpień = 0;
            int liczbaWystąpieńLib = 0;
            int liczbaWystąpieńWis = 0;
            double allGenre = 0;
            double allArtists = 0;



            //liczenie artystów
            //foreach (var item in artistList)
            //{
            //    liczbaWystąpieńLib = 0;
            //    liczbaWystąpieńWis = 0;


            //    foreach (var item2 in listAlbumsWishlist)
            //    {
            //        if (item.ArtistId == item2.ArtistId)
            //        {
            //            liczbaWystąpieńWis++;
            //        }
            //    }


            //    Artist artistWish = new Artist()
            //    {
            //        ArtistId = liczbaWystąpieńWis,
            //        Name = item.Name,
            //    };


            //    artistWishlist.Add(artistWish);

            //    foreach (var item2 in listAlbumsLibrary)
            //    {

            //        if (item.ArtistId == item2.ArtistId)
            //        {
            //            liczbaWystąpieńLib++;
            //        }
            //    }

            //    Artist artistLib = new Artist()
            //    {
            //        ArtistId = liczbaWystąpieńLib,
            //        Name = item.Name,
            //    };

            //    artistLibrary.Add(artistLib);

            //    allArtists += (double)liczbaWystąpieńLib + liczbaWystąpieńWis * 0.25;

            //}



            //foreach (var item in genresList)
            //{
            //    liczbaWystąpień = 0;
            //    foreach (var item2 in listAlbums)
            //    {

            //        if (item.Id == item2.GenreId)
            //        {
            //            liczbaWystąpień++;
            //        }
            //    }
            //    Genre genre = new Genre()
            //    {
            //        Id = liczbaWystąpień,
            //        Name = item.Name,
            //    };
            //    preferencesLibrary.Add(genre);
            //}



            List<ClientArtistPreferences> clientPrefe = new List<ClientArtistPreferences>();

            //double all = 0;
            //foreach (var item in preferencesLibrary.OrderBy(x => x.Name))
            //{
            //    double tmp = 0;
            //    foreach (var item2 in preferencesWishlist)
            //    {
            //        if (item.Name == item2.Name)
            //        {
            //            tmp = (double)(item.Id + item2.Id * 0.25);
            //        }
            //    }
            //    all += tmp;
            //}



            foreach (var item in artistLibrary)
            {
                double tmpGenre = 0;
                foreach (var item2 in artistWishlist)
                {
                    if (item.Name == item2.Name)
                    {
                        tmpGenre = (double)(item.ArtistId + item2.ArtistId * 0.25);
                    }
                }
                var newArtist = await context.Artist.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
                ClientArtistPreferences preferences = new ClientArtistPreferences()
                {
                    //wyst = tmp,
                    //Name = item.Name,
                    artistAppearances = (tmpGenre * 100 / allArtists),
                    artist = newArtist,
                };
                clientPrefe.Add(preferences);

            }


            return clientPrefe;
        }




    }
}
