using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSecApp.Models
{
    public partial class ApplicationPermission
    {
        public ApplicationPermission()
        {
            ApplicationUserPermissions = new HashSet<ApplicationUserPermission>();
        }

        public int Id { get; set; }
        public string Permission { get; set; } = null!;
        public bool IsActive { get; set; }
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add Permission";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add Permission";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public string Group { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int ApplicationSectionId { get; set; }

        public virtual ApplicationSection ApplicationSection { get; set; } = null!;
        public virtual ICollection<ApplicationUserPermission> ApplicationUserPermissions { get; set; }
        public virtual ICollection<DetalleRol> DetalleRols { get; set; } = null!;
    }
}
