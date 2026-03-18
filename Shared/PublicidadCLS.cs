using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class PublicidadCLS
    {
        public int idpublicidad { get; set; }
        public string idtorneo { get; set; }
        public string torneo { get; set; }
        public string foto { get; set; }
        public int? orden { get; set; }
    }
}
