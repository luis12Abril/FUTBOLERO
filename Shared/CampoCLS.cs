using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class CampoCLS
    {
        public int idcampo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del campo o estadio en donde se jugaran los partidos")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre es de 50 caracteres")]
        public string nombre { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima de la ubicación del campo o estadio es de 50 caracteres")]
        public string ubicacion { get; set; }

        public int idtorneo { get; set; }

        public string torneo { get; set; }

    }
}
