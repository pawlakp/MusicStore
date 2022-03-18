using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;
using System.Data.Entity;

namespace MusicStore.Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
    }
    
}
