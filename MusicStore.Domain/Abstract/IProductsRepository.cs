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
        IEnumerable<Genre> GetGenre();


        //interfejs metody asynchronicznej
        Task<List<Album>> AllAlbumAsync();
        Task<List<Artist>> AllArtistAsync();
        Task<List<Genre>> GetGenreAsync();
        Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync();
        Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre);
        Task<string> GenreNameAsync(int genre);
        Task AddAlbumAsync(AlbumAllDetails album);
        Task<Album> GetAlbumAsync(int id);

         Task<int> GetArtistId(string Name);
        Task<bool> AddSongsAsync(List<Song> songs);
        Task<List<Song>> AllSongAsync();
        Task<List<string>> GetGenresNames();
        Task<List<int>> GetGenresId();



    }
}
