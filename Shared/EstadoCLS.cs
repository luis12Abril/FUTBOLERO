using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class EstadoCLS
    {
        public int idestado { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del estado")]
        [MaxLength(30, ErrorMessage = "La longitud máxima del estado es de 30 caracteres")]
        public string nombre { get; set; }

    }
}
