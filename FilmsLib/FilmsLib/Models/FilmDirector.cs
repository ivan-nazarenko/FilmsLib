using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class FilmDirector
    {
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }

        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
    }
}
