using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class LigaCLS
    {
        public int idliga { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre de la liga es de 50 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el municipio al que pertenece la liga")]
        public string idmunicipio { get; set; }

        public string municipio { get; set; }
    }
}
