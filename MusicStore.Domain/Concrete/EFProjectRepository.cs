using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Exceptions;


namespace MusicStore.Domain.Concrete
{
    public class EFProjectRepository : IProductsRepository
    {
        //pobieranie danych z bazy + implementacja interfejsu Products
        private EfDbContext context = new EfDbContext();
        public async Task<List<Album>> AllAlbumAsync() => await context.Album.ToListAsync().ConfigureAwait(false);
        public async Task<List<Artist>> AllArtistAsync() => await context.Artist.ToListAsync().ConfigureAwait(false);
        public async Task<List<Song>> AllSongAsync() => await context.Song.ToListAsync().ConfigureAwait(false);
        public async Task<List<Genre>> AllGenreAsync() => await context.Genre.ToListAsync().ConfigureAwait(false);
        public async Task<List<Label>> AllLabelAsync() => await context.Label.ToListAsync().ConfigureAwait(false); 

        public async Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync()
        {

            var artist = await AllArtistAsync();
            var albums = await AllAlbumAsync();


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

        //pobieranie albumu według określonego gatunku
        public async Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre)
        {

            var artist = await AllArtistAsync();
            var albums = await AllAlbumAsync();


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

        public IEnumerable<Genre> GetGenre() => context.Genre;

        //pobranie nazwy danego gatunku 
        public async Task<string> GenreNameAsync(int id)
        {
            var genre = await AllGenreAsync();

            string name = genre.Where(p=> p.Id==id).First().Name;

            return name;
            
        }

        public async Task AddAlbumAsync(AlbumAllDetails album)
        {
            if (album.album != null)
            {
                bool isAlbum = await isExistAlbum(album.album);
                bool isArtist = await isExistArtist(album.artist);
                var allArtists = await AllArtistAsync();
                if (!isAlbum)
                {
                   
                    if (!isArtist)
                    {
                        context.Artist.Add(album.artist);
                        await context.SaveChangesAsync();
                        allArtists = await AllArtistAsync();
                        var arysta = allArtists.Where(s => s.Name == album.artist.Name).FirstOrDefault();
                        album.album.ArtistId = arysta.ArtistId;
                        await AddAlbum(album.album);
                    }
                    else
                    {
                        allArtists = await AllArtistAsync();
                        var isExistArtist = allArtists.Find(s => s.Name == album.artist.Name);
                        album.album.ArtistId = isExistArtist.ArtistId;
                        await AddAlbum(album.album);
                    }
                }
            }
                  else
                {
                    throw new AlbumsException(album.album);
                }
          
          
            
           
        }

        public async Task<bool> isExistAlbum(Album album)
        {
            var allAlbums = await AllAlbumAsync();
            var test = allAlbums.Find(s => s.Name == album.Name);
            if (test == null) return false;
            else return true;
        }
        public async Task<bool> isExistArtist(Artist artist)
        {
            var allArtists = await AllArtistAsync();
            var test = allArtists.Find(s => s.Name == artist.Name);
            if (test == null) return false;
            else return true;

        }


        public async Task AddAlbum(Album album)
        {
            context.Album.Add(album);
            await context.SaveChangesAsync();
        }

        public async Task<Album> GetAlbumAsync(int id)
        {
            var help = await AllAlbumAsync();
            Album product = help.Where(x => x.AlbumId == id).FirstOrDefault();
            if (product != null)
                return product;
            else
                return null;
        }

        public async Task EditAlbumAsync(Album album)
        {
            var product = context.Album.FirstOrDefault(x => x.AlbumId == album.AlbumId);

            product.AlbumId = album.AlbumId;
            product.Name = album.Name;
            product.Price = album.Price;
            product.GraphicId = album.GraphicId;
            product.ArtistId = album.ArtistId;
            product.CountryId = album.CountryId;
            product.GenreId = album.GenreId;
            product.Year = album.Year;
            product.LabelId = album.LabelId;


            await context.SaveChangesAsync();

        }
        public async Task DeleteAlbumAsync(int id)
        {
            var album = await context.Album.FindAsync(id);
            if (album != null)
            {
                context.Album.Remove(album);
            }

            await context.SaveChangesAsync();
        }


        public async Task<int> GetArtistId(string Name)
        {
            var model = await AllArtistAsync();
            Artist artysta = model.Where(s=> s.Name == Name).FirstOrDefault();
            return artysta.ArtistId;
        }
        public async Task<Artist> GetArtist(int id)
        {
            var model = await AllArtistAsync();
            Artist artysta = model.Where(s => s.ArtistId == id).FirstOrDefault();
            return artysta;
        }

        public async Task<bool> AddSongsAsync(List<Song> songs)
        {
            if (songs != null)
            {
                for (int i = 0; i < songs.Count; i++)
                {
                    Song piosenka = songs[i];
                    context.Song.Add(piosenka);
                    await context.SaveChangesAsync();
                }
                
                return true;
            }
            else return false;

        }


    }
}


