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
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly FilmsLibContext _context;
        public ReviewsRepository(FilmsLibContext context)
        {
            _context = context;
        }

        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void Delete(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<IEnumerable<Review>> GetByFilmIdAsync(int id)
        {
            return await _context.Reviews.Include(r => r.Film)
                                         .Include(r => r.Reviewer)
                                         .Where(r => r.FilmId == id)
                                         .ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.Include(r => r.Film)
                                         .Include(r => r.Reviewer)
                                         .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetByReviewerIdAsync(int id)
        {
            return await _context.Reviews.Include(r => r.Film)
                                         .Include(r => r.Reviewer)
                                         .Where(r => r.ReviewerId == id)
                                         .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            return await _context.Reviews.Include(r => r.Film)
                                         .Include(r => r.Reviewer)
                                         .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
