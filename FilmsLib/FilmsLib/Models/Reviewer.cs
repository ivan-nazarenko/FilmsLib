using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class Reviewer
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Nickname { get; set; }
        public int Age { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int WatchListId { get; set; }
        public virtual WatchList WatchList { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }
             
    }
}
