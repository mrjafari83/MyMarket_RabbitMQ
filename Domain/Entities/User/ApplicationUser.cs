using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string ProfileImageSrc { get; set; } = "/Images/DefaultUser.jpg";
    }
}
