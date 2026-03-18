using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class JuegoCLS
    {
        public int idjuego { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una jornada")]
        public string idjornada { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public string idequipo01 { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public string idequipo02 { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un campo")]
        public string idcampo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un arbitro")]
        public string idarbitro { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estatus del juego")]
        public string idestatusjuego { get; set; } = "1";

        [Required(ErrorMessage = "Debe seleccionar una fecha y hora del juego")]
        public DateTime fhorario { get; set; } = DateTime.Now;

        public int golesequipo01 { get; set; } = 0;

        public int golesequipo02 { get; set; } = 0;

        public bool cuentaparapuntos { get; set; } = true;

        public bool cuentaparagoles { get; set; } = true;

        // ESTO ES PARA VER EN LA LISTA
        public string jornadacadena { get; set; }
        public string equipo01cadena { get; set; }
        public string equipo02cadena { get; set; }
        public string fhorariocadena { get; set; }
        public string estatusjuegocadena { get; set; } = "PENDIENTE";


        public List<JugadorGolesCLS> ListaJugadorGoles01 { get; set; } = new List<JugadorGolesCLS>();
        public List<JugadorGolesCLS> ListaJugadorGoles02 { get; set; } = new List<JugadorGolesCLS>();
        public string resequipo01 { get; set; } = "E";
        public string resequipo02 { get; set; } = "E";
        public int puntosequipo01 { get; set; } = 1;
        public int puntosequipo02 { get; set; } = 1;
        public string torneo { get; set; }
        public int idtorneo { get; set; }

        [MaxLength(300, ErrorMessage = "La longitud máxima del comentario es de 300 caracteres")]
        public string comentario { get; set; } = "";
        public int idusuario { get; set; } = 0;

        public int peequipo01 { get; set; } = 0;
        public int peequipo02 { get; set; } = 0;

    }
}
