using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class MunicipioCLS
    {
        public int idmunicipio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre es de 50 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el estado del municipio")]
        public string idestado { get; set; }

        public string estado { get; set; }
    }
}
