using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Publicidad
    {
        public int Idpublicidad { get; set; }
        public int? Idtorneo { get; set; }
        public string Foto { get; set; }
        public int? Habilitado { get; set; }
        public int? Orden { get; set; }
    }
}
