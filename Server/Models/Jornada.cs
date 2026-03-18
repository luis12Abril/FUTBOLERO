using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Jornada
    {
        public int Idjornada { get; set; }
        public string Nombre { get; set; }
        public DateTime? Finiciojornada { get; set; }
        public int? Habilitado { get; set; }
        public string Torneo { get; set; }
        public int? Idtorneo { get; set; }
    }
}
