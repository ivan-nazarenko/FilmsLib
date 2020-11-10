using FilmsLib.Data;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly FilmsLibContext _context;

        public FilmRepository(FilmsLibContext context)
        {
            _context = context;
        }

        public void Add(Film film)
        {
            _context.Films.Add(film);
        }

        public void AddCover(Cover cover, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                cover.ImageMimeType = image.ContentType;
                cover.ImageName = Path.GetFileName(image.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    cover.PhotoFile = memoryStream.ToArray();
                }

                _context.Covers.Add(cover);
            }
        }

        public void AddGenres(int filmId, List<int> genreIds)
        {
            foreach (int id in genreIds)
            {
                _context.FilmGenres.Add(new FilmGenre
                {
                    FilmId = filmId,
                    GenreId = id
                });
            }
        }

        public void Delete(Film film)
        {
            _context.Films.Remove(film);
        }

        public async Task<IEnumerable<Film>> GetByDirectorAsync(int directorId)
        {
            return await _context.Films.Include(f => f.FilmGenres)
                                       .ThenInclude(g => g.Genre)
                                       .Include(f => f.Cover)
                                       .Include(f => f.Director)
                                       .Include(f => f.Language)
                                       .Where(f => f.DirectorId == directorId)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetByGenreAsync(int genreId)
        {
            return await _context.Films.Include(f => f.FilmGenres)
                                       .ThenInclude(g => g.Genre)
                                       .ThenInclude(g => g.Id == genreId)
                                       .ToListAsync();
        }

        public async Task<Film> GetByIdAsync(int id)
        {
            return await _context.Films.Include(f => f.FilmGenres)
                                       .ThenInclude(g => g.Genre)
                                       .Include(f => f.Cover)
                                       .Include(f => f.Director)
                                       .Include(f => f.Language)
                                       .SingleOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Film>> GetFilmsAsync()
        {
            return await _context.Films.Include(f => f.FilmGenres)
                                       .ThenInclude(g => g.Genre)
                                       .Include(f => f.Cover)
                                       .Include(f => f.Director)
                                       .Include(f => f.Language)
                                       .ToListAsync();
        }

        public void Update(Film film)
        {
            _context.Films.Update(film);
        }

        public async Task<IEnumerable<Film>> GetByName(string name)
        {
            return await _context.Films.Include(f => f.FilmGenres)
                                       .ThenInclude(g => g.Genre)
                                       .Include(f => f.Cover)
                                       .Include(f => f.Director)
                                       .Include(f => f.Language)
                                       .Where(f => f.Name.Contains(name))
                                       .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
