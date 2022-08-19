using System;
using System.Collections.Generic;

namespace CrudSecApp.Models.UsViewModels
{
    public class ModelUR
    {
        public IEnumerable<ApplicationUserRol> AppUserRols { get; set; } = null!; 
        public IEnumerable<Role> Role { get; set; } = null!;
        public IEnumerable<DetalleRol> DetatlleRol { get; set; } = null!;
        public IEnumerable<ApplicationUser> Users { get; set; } = null!;
        public IEnumerable<ApplicationPermission> Permissions { get; set; } = null!;
        public IEnumerable<ApplicationUserPermission> UserPermissions { get; set; } = null!;
    }
}
