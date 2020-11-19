using FilmsLib.Data;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Repositories
{
    public class DetailsRepository : IDetailsRepository
    {
        private readonly FilmsLibContext _context;

        public DetailsRepository(FilmsLibContext context)
        {
            _context = context;
        }

        public void AddDirector(Director director)
        {
            _context.Directors.Add(director);
        }

        public void AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
        }

        public void AddLanguage(Language language)
        {
            _context.Languages.Add(language);
        }

        public void DeleteDirector(Director director)
        {
            _context.Directors.Remove(director);
        }

        public void DeleteGenre(Genre genre)
        {
            _context.Genres.Remove(genre);
        }

        public void DeleteLanguage(Language language)
        {
            _context.Languages.Remove(language);
        }

        public async Task<Director> GetDirectorByIdAsync(int id)
        {
            return await _context.Directors.FindAsync(id);
        }

        public async Task<IEnumerable<Director>> GetDirectorsAsync()
        {
            return await _context.Directors.ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetFilmsByGenre(int id)
        {
            return await _context.FilmGenres.Include(g => g.Genre)
                                            .Include(f => f.Film)
                                            .ThenInclude(f => f.Cover)
                                            .Include(f => f.Film)
                                            .ThenInclude(f => f.FilmGenres)
                                            .ThenInclude(f => f.Genre)
                                            .Include(f => f.Film)
                                            .ThenInclude(d => d.Director)
                                            .Where(g => g.GenreId == id)
                                            .Select(f => f.Film).ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Language> GetLanguageByIdAsync(int id)
        {
            return await _context.Languages.FindAsync(id);
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
