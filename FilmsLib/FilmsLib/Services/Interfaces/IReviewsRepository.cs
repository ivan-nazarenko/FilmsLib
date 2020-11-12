using FilmsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Interfaces
{
    public interface IReviewsRepository
    {
        void Add(Review review);
        void Delete(Review review);
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task<Review> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetByReviewerIdAsync(int id);
        Task<IEnumerable<Review>> GetByFilmIdAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
