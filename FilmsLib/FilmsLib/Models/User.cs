using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Models
{
    public class User : IdentityUser
    {
        [MaxLength(20)]
        public string Nickname { get; set; }
        public DateTime BirthDate { get; set; }

        public int? ViewerId { get; set; }
        public virtual Viewer Viewer { get; set; }
    }
}
