using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        Task<bool> AddSongsAsync(List<Song> songs);
        Task<List<Song>> AllSongAsync();
        Task<List<Label>> AllLabelAsync();
        Task<IEnumerable<AlbumAllDetails>> AllAlbumsDetails();


        Task<IEnumerable<AlbumAllDetails>> GetAlbumWithArtistAsync();
        Task<IEnumerable<AlbumAllDetails>> GetFiltredAlbumAsync(int genre);
        Task<string> GenreNameAsync(int genre);
        Task<Album> GetAlbumAsync(int id);
        Task<Album> GetAlbumAsync(string Name);
        Task<Artist> GetArtist(int id);
        Task<int> GetArtistId(string Name);
        Task<Label> GetLabelAsync(string labelName);

        Task AddAlbumAsync(AlbumAllDetails album);
        Task AddLabelAsync(string labelName);
        Task DeleteAlbumAsync(int id);
        Task EditAlbumAsync(Album album);
        //Task<List<string>> GetLabelsNames();
        //Task<List<int>> GetLabelsId();

        Task<IEnumerable<AlbumAllDetails>> GetAlbumsToLibrary(List<int> albumsId);
        Task<AlbumDetails> GetAlbumDetailsAsync(int id);
        Task<List<SelectListItem>> GetAlbumsSelect();
        Task<List<Album>> FindAlbum(string searched);
        Task<List<Artist>> FindArtist(string searched);
        Task<List<Song>> FindSong(string searched);

        Task<IEnumerable<AlbumAllDetails>> FindAlbumWithDetails(string searchedItem);
        Task<IEnumerable<AlbumAllDetails>> FindAlbumWithSong(string searchedItem);
        Task<IEnumerable<AlbumAllDetails>> GetArtistDiscography(int id);
     


    }
}
