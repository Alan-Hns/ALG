using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSecApp.Models
{
    public partial class UserExtendedPermission
    {
        public int Id { get; set; }
        public int ApplicationUserPermissionId { get; set; }
        public string AcessCode { get; set; } = null!;
        public string? BusinessEntity { get; set; }
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;

        public virtual ApplicationUserPermission ApplicationUserPermission { get; set; } = null!;
    }
}
