using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Domain.Entities;
using MusicStore.Domain.ClientPreferences;

namespace MusicStore.WebUI.Models
{
    public class PreferencesViewModel
    {
        public IEnumerable<ClientGenrePrefences> genreList { get; set; }
        public IEnumerable<ClientArtistPreferences> artistList { get; set; }
        public IEnumerable<ClientLabelPreferences> labelList { get; set; }

        public string favYear { get; set; }
        public string favCountry { get; set; }
    }
}