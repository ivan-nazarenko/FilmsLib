using FilmsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Interfaces
{
    public interface IDetailsRepository
    {
        void AddLanguage(Language language);
        void AddDirector(Director director);
        void AddGenre(Genre genre);
        void DeleteLanguage(Language language);
        void DeleteDirector(Director director);
        void DeleteGenre(Genre genre);
        Task<IEnumerable<Language>> GetLanguagesAsync();
        Task<IEnumerable<Director>> GetDirectorsAsync();
        Task<IEnumerable<Genre>> GetGenresAsync();
        Task<Language> GetLanguageByIdAsync(int id);
        Task<Director> GetDirectorByIdAsync(int id);
        Task<Genre> GetGenreByIdAsync(int id);
        Task<IEnumerable<Film>> GetFilmsByGenre(int id); 
        Task<bool> SaveChangesAsync();
    }
}
