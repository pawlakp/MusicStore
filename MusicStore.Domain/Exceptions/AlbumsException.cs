using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MusicStore.Domain.Abstract;
using MusicStore.Domain.Entities;


namespace MusicStore.Domain.Exceptions
{
    [Serializable]
    public class AlbumsException : Exception
    {
        public AlbumsException() { }

        public AlbumsException(Album album)
            : base(String.Format("Album {0} istnieje w bazie", album.Name))
        {

        }

    }
}
