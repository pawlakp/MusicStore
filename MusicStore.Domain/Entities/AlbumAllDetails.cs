using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class AlbumAllDetails
    {
        public Album album { get; set; }
        public Artist artist { get; set; }
        public Genre genre { get; set; }
        public Label label { get; set; }
        
    }
}
