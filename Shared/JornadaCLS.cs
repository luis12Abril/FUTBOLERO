using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class JornadaCLS
    {
        public int idjornada { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de la jornada")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre de la jornada es de 50 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Ingrese la fecha de inicio de la jornada")]
        public DateTime finiciojornada { get; set; } = DateTime.Now;
        public string fjornada { get; set; }
        public string torneo { get; set; }
        public int idtorneo { get; set; }

    }
}
