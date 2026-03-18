using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Torneocolegio
    {
        public int Idtorneocolegio { get; set; }
        public string Nombre { get; set; }
        public int? Idligacolegio { get; set; }
        public int? Habilitado { get; set; }
    }
}
