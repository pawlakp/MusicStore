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
        List<Album> GetAlbums();

        //interfejs metody asynchronicznej
        Task<List<Album>> AllAlbumAsync();
        Task<List<Artist>> AllArtistAsync();
        Task<List<Genre>> AllGenreAsync();
        Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync();
        Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre);
        Task<string> GenreNameAsync(int genre);
        Task AddAlbumAsync(AlbumAllDetails album);
        Task<Album> GetAlbumAsync(int id);
        Task<Album> GetAlbumAsync(string Name);
        Task<Artist> GetArtist(int id);
        Task<int> GetArtistId(string Name);
        Task<bool> AddSongsAsync(List<Song> songs);
        Task<List<Song>> AllSongAsync();
        Task<List<Label>> AllLabelAsync();
        Task DeleteAlbumAsync(int id);
        Task EditAlbumAsync(Album album);
        //Task<List<string>> GetLabelsNames();
        //Task<List<int>> GetLabelsId();

        Task<IEnumerable<AlbumAllDetails>> GetAlbumsToLibrary(List<int> albumsId);
        Task<AlbumDetails> GetAlbumDetailsAsync(int id);

    }
}
