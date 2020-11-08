using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class WatchList
    {
        public int Id { get; set; }

        public int ReviewerId { get; set; }
        public virtual Reviewer Reviewer { get; set; }

        public virtual IEnumerable<WatchListFilm> WatchListFilms { get; set; }
    }
}
