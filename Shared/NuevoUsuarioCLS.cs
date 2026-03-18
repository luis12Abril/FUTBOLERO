using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class NuevoUsuarioCLS
    {

        [Required(ErrorMessage = "Debe ingresar el nombre del usuario")]
        [MaxLength(15, ErrorMessage = "La longitud máxima del nombre del usuario es de 15 caracteres")]
        public string nombreusuario { get; set; } = "";

        [Required(ErrorMessage = "debe ingresar una contraseña")]
        //[MinLength(4, ErrorMessage = "La contraseña debe tener minimo 4 caracteres")]
        [MaxLength(15, ErrorMessage = "La longitud máxima de la contraseña es de 15 caracteres")]
        public string contra { get; set; } = "";
    }
}

