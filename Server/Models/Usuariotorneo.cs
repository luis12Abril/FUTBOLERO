using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Usuariotorneo
    {
        public int Idusuariotorneo { get; set; }
        public int? Idusuario { get; set; }
        public int? Idtorneo { get; set; }
        public int? Habilitado { get; set; }
    }
}
