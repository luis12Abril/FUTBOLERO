using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class EstatusJuegoCLS
    {
        public int idestatusjuego { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre para el estatus del juego, Los estatus principales son PENDIENTE Y JUGADO")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre del estatus es de 50 caracteres")]
        public string nombre { get; set; }
        public int idtorneo { get; set; }
        public string torneo { get; set; }

    }
}
