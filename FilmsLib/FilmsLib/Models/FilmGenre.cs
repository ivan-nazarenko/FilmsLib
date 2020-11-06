﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class FilmGenre
    {
        public int FilmId { get; set; }
        public Film Film { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
