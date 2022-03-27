using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Domain.Entities;

namespace MusicStore.Domain.Abstract
{
    public interface IProductsRepository
    {
        IEnumerable<Album> Album { get; }
        IEnumerable<Artist> Artist { get; }
        IEnumerable<Genre> Genre { get; }
        IEnumerable<AlbumAllDetails> AlbumsWithArtist { get; }


        //interfejs metody asynchronicznej
        Task<IEnumerable<AlbumAllDetails>> GetAlbumsWithArtists();
        Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbums(int genre);
        Task<IEnumerable<Genre>> GetGenreAsync();
        Task<string> GetGenreName(int genre);
       


    }
}
