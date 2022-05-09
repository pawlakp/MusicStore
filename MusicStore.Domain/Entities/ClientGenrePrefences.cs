using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class ClientGenrePrefences
    {
        public double genreAppearances { get; set; }
        
        public Genre genre { get; set; }

    }
}
