using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ArbitroCLS
    {
        public int idarbitro { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del arbitro")]
        [MaxLength(30, ErrorMessage = "La longitud máxima del nombre es de 30 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el apellido paterno del arbitro")]
        [MaxLength(30, ErrorMessage = "La longitud máxima del apellido paterno es de 30 caracteres")]
        public string appaterno { get; set; }

        [MaxLength(30, ErrorMessage = "La longitud máxima del apellido materno es de 30 caracteres")]
        public string apmaterno { get; set; }
        public string nombrecompleto { get; set; }
        public int idtorneo { get; set; }
        public string torneo { get; set; }

    }
}
