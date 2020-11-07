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
        public virtual Reviewer Reviewer { get; set; }
    }
}
