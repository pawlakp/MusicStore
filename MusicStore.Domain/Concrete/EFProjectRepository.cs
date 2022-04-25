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
        public async Task<List<Country>> AllCountriesAsync() => await context.Country.ToListAsync().ConfigureAwait(false);

        public List<Album> GetAlbums() => context.Album.ToList();

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
            Album product = help.Where(x => x.Id == id).FirstOrDefault();
            if (product != null)
                return product;
            else
                return null;
        }

        public async Task<Album> GetAlbumAsync(string Name)
        {
            var help = await AllAlbumAsync();
            Album product = help.Where(x => x.Name == Name).FirstOrDefault();
            if (product != null)
                return product;
            else
                return null;
        }

        public async Task EditAlbumAsync(Album album)
        {
            var product = context.Album.FirstOrDefault(x => x.Id == album.Id);

            product.Id = album.Id;
            product.Name = album.Name;
            product.Price = album.Price;
            product.CountryId = album.CountryId;
            product.GenreId = album.GenreId;
            product.Year = album.Year;
            product.LabelId = album.LabelId;
            product.ImageData = album.ImageData;
            product.ImageMimeType = album.ImageMimeType;


            await context.SaveChangesAsync();

        }
        public async Task DeleteAlbumAsync(int id)
        {
            var album = await context.Album.FindAsync(id);
            var songs = await AllSongAsync();
           
            foreach (var item in songs)
            {
                if (item.AlbumId == album.Id && item.ArtistId ==album.ArtistId)
                {
                    context.Song.Remove(item);
                }
            }
            await context.SaveChangesAsync();

            if (album != null)
            {
                context.Album.Remove(album);
            }

            await context.SaveChangesAsync();
        }


        public async Task<int> GetArtistId(string Name)
        {
            var model = await AllArtistAsync();
            Artist artysta = model.Find(s => s.Name.Contains(Name));
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
                    context.SaveChanges();
                }

                return true;
            }
            else return false;

        }

        public async Task<IEnumerable<AlbumAllDetails>> GetAlbumsToLibrary(List<int> albumsId)
        {
        
            var productRecord = await GetAlbumWithArtistAsync();
            List<AlbumAllDetails> albumLibrary = new List<AlbumAllDetails>();

            foreach (var item in albumsId)
            {
                foreach(var album in productRecord)
                {
                    if(item == album.album.Id)
                    {
                        albumLibrary.Add(album);
                    }
                }
            }
            
            return albumLibrary.AsEnumerable();


            
        }

        public async Task<AlbumDetails> GetAlbumDetailsAsync(int id)
        {
            var artist = await AllArtistAsync();
            var albums = await AllAlbumAsync();
            var genres = await AllGenreAsync();
            var songs = await AllSongAsync();
            var labels = await AllLabelAsync();
            var countires = await AllCountriesAsync();
            List<Song> songList = new List<Song>();
            foreach(var item in songs)
            {
                if (item.AlbumId == id)
                {
                    songList.Add(item);
                }
            }

            var productRecord = from e in albums
                                where e.Id == id
                                join x in artist on e.ArtistId equals x.ArtistId 
                                join y in genres on e.GenreId equals y.Id 
                                join z in labels on e.LabelId equals z.Id 
                                join c in countires on e.CountryId equals c.Id
                                select new AlbumDetails
                                {
                                  Name = e.Name,
                                  Artist = x.Name,
                                  Genre = y.Name,
                                  Label = z.Name,
                                  Country = c.Name,
                                  Price = e.Price,
                                  Year = e.Year,
                                  Id = e.Id,
                                  ImageData = e.ImageData,
                                  ImageMimeType = e.ImageMimeType,
                                };
            AlbumDetails albumDetails = productRecord.Where(x => x.Id == id).FirstOrDefault();
            albumDetails.Songs = songList;

            return albumDetails;
        }




    }
}


