using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CrudSecApp.Models
{  
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            ApplicationUserPermissions = new HashSet<ApplicationUserPermission>();
            ApplicationUserRols = new HashSet<ApplicationUserRol>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add User";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add User";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime LastDateStartSession { get; set; }
        public int? ApplicationUserRoleId { get; set; }

        public virtual ApplicationUserRole? ApplicationUserRole { get; set; } = null!;
        public virtual ICollection<ApplicationUserPermission> ApplicationUserPermissions { get; set; }
        public virtual ICollection<ApplicationUserRol> ApplicationUserRols { get; set; } 

        //public virtual ApplicationUserRol ApplicationUserRols { get; set; } = null!;
        //public virtual Role Roles { get; set; } = null!;
        //public virtual DetalleRol DetalleRols { get; set; } = null!;

    }
}
