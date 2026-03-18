using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class GolCLS
    {
        public int idgol { get; set; }
        public int idjuego { get; set; }
        public int idequipo { get; set; }
        public int idjugador { get; set; }
        public int goles { get; set; }
        public int habilitado { get; set; }
    }
}