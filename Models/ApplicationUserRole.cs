using System;
using System.Collections.Generic;

namespace CrudSecApp.Models
{
    public partial class ApplicationUserRole
    {
        public ApplicationUserRole()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Roles { get; set; } = null!;

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
