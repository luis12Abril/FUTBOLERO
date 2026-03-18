using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Comunicado
    {
        public int Idcomunicado { get; set; }
        public DateTime? Fechacomunicado { get; set; }
        public string Comunicadocorto { get; set; }
        public string Comunicadolargo { get; set; }
        public int? Idtorneo { get; set; }
        public int? Habilitado { get; set; }
    }
}
