using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSecApp.Models
{
    public partial class ApplicationSection
    {
        public ApplicationSection()
        {
            ApplicationPermissions = new HashSet<ApplicationPermission>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Section { get; set; } = null!;
        public int ApplicationModuleId { get; set; }
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add Section";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add Section";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public string IconName { get; set; } = null!;

        public virtual ApplicationModule ApplicationModule { get; set; } = null!;
        public virtual ICollection<ApplicationPermission> ApplicationPermissions { get; set; }
    }
}
