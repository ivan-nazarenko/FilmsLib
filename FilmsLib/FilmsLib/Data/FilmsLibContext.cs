using FilmsLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Data
{
    public class FilmsLibContext : DbContext
    {
        public FilmsLibContext(DbContextOptions<FilmsLibContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Viewer> Viewers { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<FilmDirector> FilmDirectors { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FilmDirector>().HasKey(fd => new { fd.FilmId, fd.DirectorId });

            builder.Entity<FilmGenre>().HasKey(fg => new { fg.FilmId, fg.GenreId });
        }
    }
}
