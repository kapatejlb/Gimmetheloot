using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimmeTheLoot.Models
{
    public class AspNetUser : IdentityUser
    {
        public ICollection<Project> Projects { get; set; }

        public ICollection<Commentary> Comments { get; set; }
    }
}
