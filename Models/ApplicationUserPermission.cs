using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSecApp.Models
{
    public partial class ApplicationUserPermission
    {
        public ApplicationUserPermission()
        {
            UserExtendedPermissions = new HashSet<UserExtendedPermission>();
        }

        public int Id { get; set; }
        public int ApplicationUserId { get; set; }
        public int ApplicationPermissionId { get; set; }
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "User Access Transaction";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public int ApplicationId { get; set; }
        public string Site { get; set; } = null!;

        public virtual ApplicationPermission ApplicationPermission { get; set; } = null!;
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual ICollection<UserExtendedPermission> UserExtendedPermissions { get; set; }
    }
}
