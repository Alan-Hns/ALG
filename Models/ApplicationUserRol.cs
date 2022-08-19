using System;
using System.Collections.Generic;

namespace CrudSecApp.Models
{
    public class ApplicationUserRol
    {
        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public int RolesId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual Role Roles { get; set; } = null!;
    }
}
