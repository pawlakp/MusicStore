using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.ClientPreferences
{
    public class ClientLabelPreferences
    {
        public double lableApperances { get; set; }
        public Label label { get; set; }

        public int sumLibrary { get; set; }

        public int sumWishlist {get; set;}

    }
}
