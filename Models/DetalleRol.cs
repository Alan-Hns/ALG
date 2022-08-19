using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CrudSecApp.Models
{
    public class DetalleRol
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int ApplicationPermissionId { get; set; }

        public virtual ApplicationPermission ApplicationPermission { get; set; } = null!;
        public virtual Role Rol { get; set; } = null!;
    }
}
