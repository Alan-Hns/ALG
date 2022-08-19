using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CrudSecApp.Models
{
    public partial class Application
    {
        public Application()
        {
            ApplicationModules = new HashSet<ApplicationModule>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid TransaccionUid { get; set; }
        public string TipoTransaccion { get; set; } = "Add"; //null!;
        public DateTime FechaTransaccionUtc { get; set; }
        public string DescripcionTransaccion { get; set; } = "Add"; //null!;
        public DateTime? FechaTransaccion { get; set; }
        public string ModificadoPor { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public string Code { get; set; } = null!;

        public virtual ICollection<ApplicationModule> ApplicationModules { get; set; }
    }
}
