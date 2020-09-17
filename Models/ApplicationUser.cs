using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace justAsk.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Karma { get; set; }
        public List<Vote> Votes { get; set; }
    }
}