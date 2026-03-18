using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ArbitroColegioCLS
    {
        public int idarbitrocolegio { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un colegio de arbitros")]
        public string idcolegioarbitro { get; set; }       

        [Required(ErrorMessage = "Debe ingresar el nombre de la liga")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del nombre es de 80 caracteres")]
        public string nombre { get; set; }
        public string nombrecompleto { get; set; }

        [Required(ErrorMessage = "Debe ingresar el apellido paterno")]
        [MaxLength(80, ErrorMessage = "La longitud máxima del apellido es de 80 caracteres")]
        public string appaterno { get; set; }

        public string apmaterno { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento del arbitro")]
        public DateTime fnacimiento { get; set; } = DateTime.Now;
        public string fnacimientocadena { get; set; }

        public int peso { get; set; }
        public string pesocadena { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de usuario del arbitro")]
        [MaxLength(20, ErrorMessage = "La longitud máxima del nombre del usuario es de 20 caracteres")]
        public string nomusuario { get; set; }
        public string nomusuariocopia { get; set; }

        [Required(ErrorMessage = "Debe ingresar la clave del arbitro")]
        [MaxLength(20, ErrorMessage = "La longitud máxima de la clave es de 20 caracteres")]
        public string codigo { get; set; }

        public string colegio { get; set; }

        public string fotoarbitro { get; set; }
    }
}
