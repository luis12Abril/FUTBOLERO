using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ProgramacionColegio123CLS
    {

        public int idprogramacioncolegio { get; set; }


        public string idcolegio { get; set; }


        public string idarbitrocolegio { get; set; }


        public string idligacolegio { get; set; }


        public string idtorneocolegio { get; set; }


        public string idequipo01 { get; set; }


        public string idequipo02 { get; set; }


        public string idcampocolegio { get; set; }


        public DateTime fhorario { get; set; } = DateTime.Now;


        public string comentariocolegio { get; set; } = "";



        // ESTO ES PARA VER EN LA LISTA
        public string nombrecolegio { get; set; }
        public string nombrecompleto { get; set; }
        public string campocadena { get; set; }
        public string equipo01cadena { get; set; }
        public string equipo02cadena { get; set; }
        public string fhorariocadena { get; set; }
        public string comentariojuego { get; set; }


    }
}
