using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class AlbumsWithArtist
    {
        public Album album { get; set; }
        public Artist artist { get; set; }
    }
}
