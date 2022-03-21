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

        public string GetArtist(int artistId)
        {
            Artist dbEntry = (Artist)context.Albums.Where(x => x.ArtistId == artistId);
            return dbEntry.Name;
            
        }
    }
}

