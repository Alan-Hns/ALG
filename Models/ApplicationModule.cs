using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudSecApp.Models
{
    public partial class ApplicationModule
    {
        public ApplicationModule()
        {
            ApplicationSections = new HashSet<ApplicationSection>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Module { get; set; } = null!;
        public int ApplicationId { get; set; }
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add Module";
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add Module";
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;

        public virtual Application Application { get; set; } = null!;
        public virtual ICollection<ApplicationSection> ApplicationSections { get; set; }
    }
}
