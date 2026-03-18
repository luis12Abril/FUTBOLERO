using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Persona
    {
        public int Idpersona { get; set; }
        public string Nombre { get; set; }
        public string Appaterno { get; set; }
        public string Apmaterno { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime? Fechanacimiento { get; set; }
        public int? Habilitado { get; set; }
        public int? Tieneusuario { get; set; }
    }
}
