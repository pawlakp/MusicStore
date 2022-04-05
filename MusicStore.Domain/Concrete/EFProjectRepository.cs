using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;


namespace MusicStore.Domain.Concrete
{
    public class EFProjectRepository : IProductsRepository
    {
        //pobieranie danych z bazy + implementacja interfejsu Products
        private EfDbContext context = new EfDbContext();


        public async Task<List<Album>> AllAlbumAsync() => await context.Albums.ToListAsync().ConfigureAwait(false);
        public async Task<List<Artist>> AllArtistAsync() => await context.Artist.ToListAsync().ConfigureAwait(false);

        public IEnumerable<Album> Album
        {
            get { return context.Albums; }

        }
        public IEnumerable<Artist> Artist
        {
            get { return context.Artist; }
            
        }
        public IEnumerable<Genre> Genre
        {
            get { return context.Genre; }

        }

        public IEnumerable<Accounts> Accounts
        {
            get { return context.Accounts;  }
        }


        //pobieranie danych albumu wraz z artystą
        public IEnumerable<AlbumAllDetails> AlbumsWithArtist
        {

            get
            {
                List<Artist> artist = context.Artist.ToList();
                List<Album> albums = context.Albums.ToList();


                var productRecord = from e in albums
                                    join x in artist on e.ArtistId equals x.ArtistId into table1
                                    from x in table1.ToList()
                                    select new AlbumAllDetails
                                    {
                                        album = e,
                                        artist = x
                                    };

                return productRecord;
            }
        }

        //metoda asychroniczna 
        public async Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync()
        {

            List<Artist> artist = await AllArtistAsync();
            List<Album> albums = await AllAlbumAsync();


            var productRecord = from e in albums
                                join x in artist on e.ArtistId equals x.ArtistId into table1
                                from x in table1.ToList()
                                select new AlbumAllDetails
                                {
                                    album = e,
                                    artist = x
                                };

            return productRecord;

        }

        //pobieranie danych gatunków muzycznych
        public async Task<List<Genre>> GetGenreAsync() => await context.Genre.ToListAsync().ConfigureAwait(false);
        public IEnumerable<Genre> GetGenre() => context.Genre;

        //pobieranie albumu według określonego gatunku
        public async Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre)
        {

            List<Artist> artist = await AllArtistAsync();
            List<Album> albums = await AllAlbumAsync();


            var productRecord = from e in albums where e.GenreId == genre
                                join x in artist on e.ArtistId equals x.ArtistId into table1
                                from x in table1.ToList()
                                select new AlbumAllDetails
                                {
                                    album = e,
                                    artist = x
                                };

            return productRecord;

        }

        //pobranie nazwy danego gatunku 
        public async Task<string> GenreNameAsync(int id)
        {
            var genre = await GetGenreAsync();

            string name = genre.Where(p=> p.Id==id).First().Name;

            return name;
            
        }
    }
}


