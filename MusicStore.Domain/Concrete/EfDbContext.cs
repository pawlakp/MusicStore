using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;
using System.Data.Entity;
using System.Threading;

namespace MusicStore.Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        public DbSet<Album> Album { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Genre> Genre {get; set; }
        public DbSet<Accounts> Account { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Adress> Adresses { get; set; }
    }
    
}
