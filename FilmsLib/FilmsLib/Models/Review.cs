using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class Review
    {
        public int Id { get; set; }
        public float Mark { get; set; }

        [MaxLength(100)]
        public string Heading { get; set; }
        public string Text { get; set; }
        public DateTime PublicatonDate { get; set; }

        public int FilmId { get; set; }
        public virtual Film Film { get; set; }

        public int ReviewerId { get; set; }
        public virtual Reviewer Reviewer { get; set; }
    }
}
