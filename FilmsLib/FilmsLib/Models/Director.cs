using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class Director
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName {get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public virtual IEnumerable<FilmDirector> FilmDirectors { get; set; }
}
}
