using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Domain.Entities
{
    public class Album
    {
        public int id { get; set; }
        public string album_name { get; set; }
        public int artist_id { get; set; }
        public int music_label_id { get; set; }
        public string year_production { get; set; }
        public int genre_id { get; set; }
        public int country_id { get; set; }
        public decimal price { get; set; }

    }
}
