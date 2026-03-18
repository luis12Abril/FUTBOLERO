using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class TorneoColegioCLS
    {
        public int idtorneocolegio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del torneo")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del nombre es de 80 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una liga")]
        public string idligacolegio { get; set; }

        public string liga { get; set; }
    }
}
