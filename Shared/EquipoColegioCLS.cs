using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class EquipoColegioCLS
    {
        public int idequipocolegio { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del torneo")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del nombre es de 80 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un colegio")]
        public string idcolegio { get; set; }

        public string colegio { get; set; }
    }
}
