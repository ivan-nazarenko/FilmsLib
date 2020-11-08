using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class Film
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public string TrailerLink { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int? CoverId { get; set; }
        public virtual Cover Cover { get; set; }

        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }

        public virtual IEnumerable<FilmGenre> FilmGenres { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public virtual IEnumerable<WatchListFilm> WatchListFilms { get; set; }
    }
}
