using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.ClientPreferences
{
    public class ClientArtistPreferences
    {
        public Artist artist { get; set; }
        public double artistAppearances { get; set; }

        public int sumLibrary { get; set; }

        public int sumWishlist { get; set; }

        public List<Album> listAlbums { get; set; }
    }
}
