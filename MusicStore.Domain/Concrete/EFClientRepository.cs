using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.Domain.ClientPreferences;
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
            var clientLibrary = from e in context.ClientLibrary
                                where e.ClientId == id
                                select e.AlbumId;

            return await clientLibrary.ToListAsync();
        }

        public async Task<List<int>> GetClientWishlist(int id)
        {
            var clientWishlist = from e in context.ClientWishlist
                                where e.ClientId == id
                                select e.AlbumId;

            return await clientWishlist.ToListAsync();
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

        public async Task<List<ClientLabelPreferences>> GetClientLabelPreferences(int clientId)
        {


            var clientWishlist = await context.ClientWishlist.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie listy życzeń klienta
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie biblioteki klienta


            var albumList = await context.Album.ToListAsync(); // lista wszystkich albumów
            var labelList = await context.Label.ToListAsync(); //lista wszystkich gatunków
            var artistList = await context.Artist.ToListAsync(); //lista wszystkich artystów


            var listAlbumsWishlist = from x in clientWishlist
                                     join y in albumList on x.AlbumId equals y.Id
                                     select y; // pobranie detalów albumów z listy życzeń

            var listAlbumsLibrary = from x in clientLibrary
                                    join y in albumList on x.AlbumId equals y.Id
                                    select y;  // pobranie detalów albumów z biblioteki


            //preferowane gatunki
            List<Label> labelWishlist = new List<Label>();
            List<Label> labelLibrary = new List<Label>();

            //preferowani artyści
            //List<Artist> artistWishlist = new List<Artist>();
            //List<Artist> artistLibrary = new List<Artist>();


            //int liczbaWystąpień = 0;
            int liczbaWystąpieńLib = 0;
            int liczbaWystąpieńWis = 0;
            double allLabel = 0;
            //double allArtists = 0;

            //liczenie gatunków
            foreach (var item in labelList)
            {
                liczbaWystąpieńLib = 0;
                liczbaWystąpieńWis = 0;


                foreach (var item2 in listAlbumsWishlist)
                {
                    if (item.Id == item2.LabelId)
                    {
                        liczbaWystąpieńWis++;
                    }
                }


                Label labelWish = new Label()
                {
                    Id = liczbaWystąpieńWis,
                    Name = item.Name,
                };


                labelWishlist.Add(labelWish);

                foreach (var item2 in listAlbumsLibrary)
                {

                    if (item.Id == item2.LabelId)
                    {
                        liczbaWystąpieńLib++;
                    }
                }

                Label labelLib = new Label()
                {
                    Id = liczbaWystąpieńLib,
                    Name = item.Name,
                };

                labelLibrary.Add(labelLib);

                allLabel += (double)liczbaWystąpieńLib + liczbaWystąpieńWis * 0.25;

            }
            
            int sumWishlist = 0;
            List<ClientLabelPreferences> clientPrefe = new List<ClientLabelPreferences>();

            foreach (var item in labelLibrary)
            {
                double tmpLabel = 0;
               
                foreach (var item2 in labelWishlist)
                {
                    
                    if (item.Name == item2.Name)
                    {
                        sumWishlist += item2.Id;
                        tmpLabel = (double)(item.Id + item2.Id * 0.25);
                    }
                }
                if (tmpLabel > 0)
                {


                    var newGenre = await context.Label.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
                    ClientLabelPreferences preferences = new ClientLabelPreferences()
                    {
                        //wyst = tmp,
                        //Name = item.Name,
                        lableApperances = Math.Round((tmpLabel * 100 / allLabel),2),
                        label = newGenre,
                        sumLibrary = item.Id,
                        sumWishlist = sumWishlist
                    };
                    clientPrefe.Add(preferences);
                    sumWishlist = 0;
                }
            }
            return clientPrefe;
        }





        public async Task<List<ClientGenrePrefences>> GetClientGenrePreferences(int clientId)
        {


            var clientWishlist = await context.ClientWishlist.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie listy życzeń klienta
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie biblioteki klienta
           
            
            var albumList = await context.Album.ToListAsync(); // lista wszystkich albumów
            var genresList = await context.Genre.ToListAsync(); //lista wszystkich gatunków
            var artistList = await context.Artist.ToListAsync(); //lista wszystkich artystów


            var listAlbumsLibrary = from x in clientLibrary
                                    join y in albumList on x.AlbumId equals y.Id
                                    select y;  // pobranie  albumów z biblioteki


            var listAlbumsWishlist = from x in clientWishlist
                                     join y in albumList on x.AlbumId equals y.Id
                                     select y; // pobranie albumów z listy życzeń

          

            
            List<Genre> genreWishlist = new List<Genre>(); //lista gatunków na liście życzeń
            List<Genre> genreLibrary = new List<Genre>(); //lista gatunków w bibliotece

            
            int numLibrary = 0;
            int numWishlist = 0;
            double allGenre = 0;
            

            //liczenie gatunków
            foreach (var item in genresList)
            {
                numLibrary = 0;
                numWishlist = 0;


                foreach (var item2 in listAlbumsWishlist)
                {
                    if (item.Id == item2.GenreId)
                    {
                        numWishlist++;
                    }
                }


                Genre genreWish = new Genre()
                {
                    Id = numWishlist,
                    Name = item.Name,
                };


                genreWishlist.Add(genreWish);

                foreach (var item2 in listAlbumsLibrary)
                {

                    if (item.Id == item2.GenreId)
                    {
                        numLibrary++;
                    }
                }

                Genre genreLib = new Genre()
                {
                    Id = numLibrary,
                    Name = item.Name,
                };

                genreLibrary.Add(genreLib);

                allGenre += (double)numLibrary + numWishlist * 0.25;

            }


            int sumWishlist = 0;

            List<ClientGenrePrefences> clientPrefe = new List<ClientGenrePrefences>(); //Lista preferowanych gatunków

            foreach (var item in genreLibrary)
            {
                double tmpGenre = 0; 
                foreach (var item2 in genreWishlist)
                {
                   
                    if (item.Name == item2.Name)
                    {
                        sumWishlist += item2.Id; //zliczanie liczby albumów na liście życzeń
                        tmpGenre = (double)(item.Id + item2.Id * 0.25); // liczba wystąpień razem
                    }
                }
                if(tmpGenre > 0)
                {
                    var newGenre = await context.Genre.Where(x => x.Name == item.Name).FirstOrDefaultAsync(); //pobranie gatunku z bazy
                        ClientGenrePrefences preferences = new ClientGenrePrefences()
                                {
                    
                                genreAppearances = Math.Round((tmpGenre * 100 / allGenre), 2), // % liczby wystąpień
                                genre = newGenre, //gatunek
                                sumLibrary = item.Id, //liczba albumów w bibliotece 
                                sumWishlist = sumWishlist, //liczba albumów na liście życzeń
                                };

                    clientPrefe.Add(preferences);
                    sumWishlist = 0;
                }  
            }
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
            double allArtists = 0;



            //liczenie artystów
            foreach (var item in artistList)
            {
                liczbaWystąpieńLib = 0;
                liczbaWystąpieńWis = 0;


                foreach (var item2 in listAlbumsWishlist)
                {
                    if (item.ArtistId == item2.ArtistId)
                    {
                        liczbaWystąpieńWis++;
                    }
                }


                Artist artistWish = new Artist()
                {
                    ArtistId = liczbaWystąpieńWis,
                    Name = item.Name,
                };


                artistWishlist.Add(artistWish);

                foreach (var item2 in listAlbumsLibrary)
                {

                    if (item.ArtistId == item2.ArtistId)
                    {
                        liczbaWystąpieńLib++;
                    }
                }

                Artist artistLib = new Artist()
                {
                    ArtistId = liczbaWystąpieńLib,
                    Name = item.Name,
                };

                artistLibrary.Add(artistLib);

                allArtists += (double)liczbaWystąpieńLib + liczbaWystąpieńWis * 0.25;

            }


            List<ClientArtistPreferences> clientPrefe = new List<ClientArtistPreferences>();



            int sumWishlist = 0;

            foreach (var item in artistLibrary)
            {
                double tmpArtist = 0;
                foreach (var item2 in artistWishlist)
                {
                   
                    if (item.Name == item2.Name)
                    {
                        sumWishlist += item2.ArtistId;
                        tmpArtist = (double)(item.ArtistId + item2.ArtistId * 0.25);
                    }
                }
                if(tmpArtist > 0)
                {
                    var newArtist = await context.Artist.Where(x => x.Name == item.Name).FirstOrDefaultAsync();
                    ClientArtistPreferences preferences = new ClientArtistPreferences()
                    {

                        artistAppearances = Math.Round((tmpArtist * 100 / allArtists), 2),
                        artist = newArtist,
                        sumLibrary = item.ArtistId,
                        sumWishlist = sumWishlist,
                        listAlbums = listAlbumsLibrary.Where(x=> x.ArtistId == newArtist.ArtistId).ToList(),
                    };
                    clientPrefe.Add(preferences);
                }
                sumWishlist = 0;
            }


            return clientPrefe;
        }

        public async Task<ClientRestPreferences> GetClientRestPreferences(int clientId)
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



            var listAlbumsunder2000 = listAlbumsLibrary.Where(x => x.Year < 2000).ToList();
            var listAlbumsover2000 = listAlbumsLibrary.Where(x => x.Year >= 2000).ToList();

            string favYear = "Obydwa";

            if (listAlbumsunder2000.Count() >= listAlbumsover2000.Count()) favYear = "Klasyczna";
            else favYear = "Nowoczesna";

            var listAlbumsPoland = listAlbumsLibrary.Where(x => x.CountryId == 1);
            var listAlbumsOther = listAlbumsLibrary.Where(x => x.CountryId == 2).ToList();

            string favCountry = "Obydwa";

            if (listAlbumsPoland.Count() > listAlbumsOther.Count()) favCountry = "Polska";
            else favCountry = "Zagraniczna";

            ClientRestPreferences newPrefe = new ClientRestPreferences
            {
                favYear = favYear,
                favCountry = favCountry,
                numberLibrary = clientLibrary.Count(),
                numberWishlist = clientWishlist.Count(),
            };

            return newPrefe;


        }

        public async Task<IEnumerable<Album>> GetLastPucharses(int clientId)
        {
            var orderList = await context.Order.Where(x=> x.ClientId == clientId).ToListAsync();
            var lastOrder = orderList.OrderByDescending(x => x.Data).First();

            var albumsOrder = await context.OrdersAlbums.Where(x => x.OrderId == lastOrder.Id).ToListAsync();
            var albumsAll = await context.Album.ToListAsync();
            
            var albumsList = from x in albumsOrder
                             join y in albumsAll on x.AlbumId equals y.Id
                             select y;

            return albumsList;


        }

        public async Task<IEnumerable<AlbumAllDetails>> GetClientSuggestions(int clientId)
        {
            var clientWishlist = await context.ClientWishlist.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie listy życzeń klienta
            var clientLibrary = await context.ClientLibrary.Where(x => x.ClientId == clientId).ToListAsync(); // pobranie biblioteki klienta
            var allAlbums = await context.Album.ToListAsync();
            var allArtist = await context.Artist.ToListAsync();


            var clientGenrePreferences = await GetClientGenrePreferences(clientId);
            var clientArtistPreferences = await GetClientArtistPreferences(clientId);
            var clientLabelPreferences = await GetClientLabelPreferences(clientId);

            var artistSuggestion = from x in allAlbums
                                   join y in clientArtistPreferences on x.ArtistId equals y.artist.ArtistId
                                   select x;
            var labelSuggestion = from x in allAlbums
                                   join y in clientLabelPreferences on x.LabelId equals y.label.Id
                                   select x;
            var genreSuggestion = from x in allAlbums
                                  join y in clientGenrePreferences on x.GenreId equals y.genre.Id
                                  select x;

            List<AlbumAllDetails> albumList = new List<AlbumAllDetails>();

            foreach(var item in artistSuggestion)
            {
                AlbumAllDetails album = new AlbumAllDetails
                {
                    album = item,
                    artist = allArtist.Where(x => x.ArtistId == item.ArtistId).FirstOrDefault(),
                };
                albumList.Add(album);
            }

            foreach (var item in labelSuggestion)
            {
                AlbumAllDetails album = new AlbumAllDetails
                {
                    album = item,
                    artist = allArtist.Where(x => x.ArtistId == item.ArtistId).FirstOrDefault(),
                };
                albumList.Add(album);
            }
            foreach (var item in genreSuggestion)
            {
                AlbumAllDetails album = new AlbumAllDetails
                {
                    album = item,
                    artist = allArtist.Where(x => x.ArtistId == item.ArtistId).FirstOrDefault(),
                };
                albumList.Add(album);
            }

            return albumList;


        }

      



    }
}
