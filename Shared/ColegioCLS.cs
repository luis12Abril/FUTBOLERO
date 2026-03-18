using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ColegioCLS
    {
        public int idcolegio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del colegio")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del nombre es de 80 caracteres")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un usuario presidente")]
        public string idpresidente { get; set; }
        public string nombreusu { get; set; }

    }
}

