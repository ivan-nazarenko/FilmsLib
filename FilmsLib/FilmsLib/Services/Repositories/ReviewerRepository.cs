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
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly FilmsLibContext _context;

        public ReviewerRepository(FilmsLibContext context)
        {
            _context = context;
        }

        public void Add(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
        }

        public async Task<Reviewer> GetByIdAsync(int id)
        {
            return await _context.Reviewers.FindAsync(id);
        }

        public async Task<Reviewer> GetByNicknameAsync(string nickname)
        {
            return await _context.Reviewers.SingleOrDefaultAsync(r => r.Nickname == nickname);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
