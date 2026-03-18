using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ProgramacionColegioCLS
    {
        public int idprogramacioncolegio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un colegio")]
        public string idcolegio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un arbitro")]
        public string idarbitrocolegio { get; set; }

        //[Required(ErrorMessage = "Debe seleccionar una liga")]
        public string idligacolegio { get; set; }

        //[Required(ErrorMessage = "Debe seleccionar un torneo")]
        public string idtorneocolegio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public string idequipo01 { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public string idequipo02 { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un campo")]
        public string idcampocolegio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha y hora del juego")]
        public DateTime fhorario { get; set; } = DateTime.Now;

        [MaxLength(800, ErrorMessage = "La longitud máxima del comentario es de 800 caracteres")]
        public string comentariocolegio { get; set; } = "";



        //public int golesequipo01 { get; set; } = 0;

        //public int golesequipo02 { get; set; } = 0;



        // ESTO ES PARA VER EN LA LISTA
        public string nombrecolegio { get; set; }
        public string nombrecompleto { get; set; }
        public string campocadena { get; set; }
        public string equipo01cadena { get; set; }
        public string equipo02cadena { get; set; }
        public string fhorariocadena { get; set; }
        public string comentariojuego { get; set; }



        //public int idusuario { get; set; } = 0;

    }
}

