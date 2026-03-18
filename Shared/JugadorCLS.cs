using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class JugadorCLS
    {
        public int idjugador { get; set; }
        public int numero { get; set; } = 0;

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre es de 50 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el apellido paterno")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del apellido paterno del jugador es de 50 caracteres")]
        public string appaterno { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima del apellido materno del jugador es de 50 caracteres")]
        public string apmaterno { get; set; } = " ";

        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento del jugador")]
        public DateTime fnacimiento { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe ingresar el equipo del jugador")]
        public string idequipo { get; set; }

        public int goles { get; set; }

        public string nombrecompleto { get; set; }

        public string equipo { get; set; }

        public string fnacimientocadena { get; set; }
        public string torneo { get; set; }
        public int idtorneo { get; set; }
        public int años { get; set; }       
    }
}


