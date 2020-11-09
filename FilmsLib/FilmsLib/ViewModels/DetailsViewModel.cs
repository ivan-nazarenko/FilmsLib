using FilmsLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class DetailsViewModel
    {
        public IEnumerable<Language> Languages { get; set; }
        public IEnumerable<Director> Directors { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
