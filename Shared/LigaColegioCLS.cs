using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class LigaColegioCLS
    {
        public int idligacolegio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de la liga")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del nombre es de 80 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un colegio de arbitros")]
        public string idcolegioarbitro { get; set; }

        public string colegio { get; set; }
    }
}
