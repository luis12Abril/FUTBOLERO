using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class TipoUsuarioCLS
    {
        public int iidTipoUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del tipo de usuario")]
        [MaxLength(100, ErrorMessage = "La longitus máxima es de 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripción del tipo de usuario")]
        [MaxLength(100, ErrorMessage = "La longitus máxima es de 100 caracteres")]
        public string descripcion { get; set; }

        public List<int> listaid { get; set; } = new List<int>();
    }
}
