using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models
{
    public partial class Avisofutboleando
    {
        public int Idavisofutboleando { get; set; }
        public string Titulomensaje { get; set; }
        public string Mensaje { get; set; }
        public DateTime? Fechamensaje { get; set; }
        public int? Habilitado { get; set; }
    }
}
