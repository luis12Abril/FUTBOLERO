using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class JuegoInvitadoCLS
    {
        public int idjuego { get; set; }
        public string jornadacadena { get; set; }
        public string equipo01cadena { get; set; }
        public string equipo02cadena { get; set; }
        public string resequipo01 { get; set; } = "E";
        public string resequipo02 { get; set; } = "E";
        public string golesequipo01 { get; set; }       // LOS PUSE STRING PORQUE SOLO LOS VOY A MOSTRAR
        public string golesequipo02 { get; set; }       // LOS PUSE STRING PORQUE SOLO LOS VOY A MOSTRAR
        public string fhorariocadena { get; set; }
        public string campocadena { get; set; }
        public string arbitrocadena { get; set; }
        public DateTime fhorario { get; set; } = DateTime.Now;
        public string estatusjuegocadena { get; set; }

        public string idequipo01 { get; set; }
        public string idequipo02 { get; set; }
        public List<JugadorGolesCLS> ListaJugadorGoles01 { get; set; } = new List<JugadorGolesCLS>();
        public List<JugadorGolesCLS> ListaJugadorGoles02 { get; set; } = new List<JugadorGolesCLS>();


        [MaxLength(300, ErrorMessage = "La longitud máxima del comentario es de 300 caracteres")]
        public string comentario { get; set; } = "";
        public int idusuario { get; set; } = 0;

        public int peequipo01 { get; set; } = 0;
        public int peequipo02 { get; set; } = 0;

    }
}