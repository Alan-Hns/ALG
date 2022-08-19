using System;
using System.Collections.Generic;

namespace CrudSecApp.Models
{
    public class Role
    {
        public Role()
        {
            ApplicationUserRols = new HashSet<ApplicationUserRol>();
            DetalleRols = new HashSet<DetalleRol>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public bool Estado { get; set; }

     
        public virtual ICollection<ApplicationUserRol> ApplicationUserRols { get; set; }
        public virtual ICollection<DetalleRol> DetalleRols { get; set; }
    }
}
