using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class ClientArtistPreferences
    {
        public Artist artist { get; set; }
        public double artistAppearances { get; set; }
    }
}
