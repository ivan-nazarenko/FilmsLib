﻿using FilmsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Services.Interfaces
{
    public interface IReviewerRepository
    {
        void Add(Reviewer reviewer);
        Task<Reviewer> GetByNicknameAsync(string nickname);
        Task<Reviewer> GetByIdAsync(int id);
        Task<Reviewer> GetByUserId(string id);
        Task<Review> GetReviewByFilmId(int filmId, int reviewerId);
        Task<bool> SaveChangesAsync();
    }
}
