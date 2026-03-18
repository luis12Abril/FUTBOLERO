using System;
using System.Collections.Generic;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ProximosCumpleañerosCLS
    {
        public int idjugador { get; set; }
        public string nombrejugador { get; set; }
        public string nombreequipo { get; set; }
        public DateTime fnacimiento { get; set; }
        public string fnacimientocadena { get; set; }
        public string fcumpleañoscadenacorta { get; set; }
        public int años { get; set; }
        public string quedialocumple()
        {
            return "HOY";
        }

    }
}
