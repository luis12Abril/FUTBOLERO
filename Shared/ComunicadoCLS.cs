using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ComunicadoCLS
    {
        public int idcomunicado { get; set; }

        [Required(ErrorMessage = "Ingrese la fecha del comunicado")]
        public DateTime fechacomunicado { get; set; } = DateTime.Now;

        public string fechacomunicadocadena { get; set; }

        [Required(ErrorMessage = "Debe ingresar un encabezado del comunicado")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del encabezado es de 80 caracteres")]
        public string comunicadocorto { get; set; }

        [Required(ErrorMessage = "Debe ingresar un texto en el comunicado")]
        [MaxLength(15000, ErrorMessage = "La longitud máxima del comunicado es de 15000 caracteres")]
        public string comunicadolargo { get; set; }

        public int idtorneo { get; set; }
    }
}
