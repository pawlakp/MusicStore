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
        Task<List<Album>> AllAlbumAsync();
        Task<List<Artist>> AllArtistAsync();
        Task<List<Genre>> GetGenreAsync();
        Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync();
        Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre);
        IEnumerable<Genre> GetGenre();
        Task<string> GenreNameAsync(int genre);
       


    }
}
