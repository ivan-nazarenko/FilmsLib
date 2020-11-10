using FilmsLib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Interfaces
{
    public interface IFilmRepository
    {
        void Add(Film film);
        void Delete(Film film);
        void Update(Film film);
        Task<Film> GetByIdAsync(int id);
        Task<IEnumerable<Film>> GetByGenreAsync(int genreId);
        Task<IEnumerable<Film>> GetByDirectorAsync(int directorId);
        Task<IEnumerable<Film>> GetFilmsAsync();
        Task<IEnumerable<Film>> GetByName(string name);
        void AddGenres(int filmId, List<int> genreIds);
        void AddCover(Cover cover, IFormFile image);
        Task<bool> SaveChangesAsync();
    }
}
