using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.Concrete
{
    public class EFAlbumRepository : IAlbumRepository
    {
        private EfDbContext context = new EfDbContext();

        public IEnumerable<Album> Album
        {
            get { return context.Albums; }
        }

    }
}

