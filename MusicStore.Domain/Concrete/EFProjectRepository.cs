using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;


namespace MusicStore.Domain.Concrete
{
    public class EFProjectRepository : IProductsRepository
    {
        private EfDbContext context = new EfDbContext();

       
        public IEnumerable<Album> Album
        {
            get { return context.Albums; }
            
        }
        public IEnumerable<Artist> Artist
        {
            get { return context.Artist; }
            
        }

        public IEnumerable<AlbumsWithArtist> AlbumsWithArtist
        {

            get
            {
                List<Artist> artist = context.Artist.ToList();
                List<Album> albums = context.Albums.ToList();


                var productRecord = from e in albums
                                    join x in artist on e.ArtistId equals x.ArtistId into table1
                                    from x in table1.ToList()
                                    select new AlbumsWithArtist
                                    {
                                        album = e,
                                        artist = x
                                    };

                return productRecord;
            }
        }

        //metoda asychroniczna 
        public async Task<IEnumerable<AlbumsWithArtist>> GetAlbumsWithArtists()
        {

            List<Artist> artist = await Task.FromResult(context.Artist.ToList());
            List<Album> albums = await Task.FromResult(context.Albums.ToList());


            var productRecord = from e in albums
                                join x in artist on e.ArtistId equals x.ArtistId into table1
                                from x in table1.ToList()
                                select new AlbumsWithArtist
                                {
                                    album = e,
                                    artist = x
                                };

            return productRecord;

        }
    }
}

