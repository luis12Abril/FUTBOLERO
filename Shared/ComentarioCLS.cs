using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class ComentarioCLS
    {
        public int idcomentario { get; set; }

        [Required(ErrorMessage = "Debe ingresar texto en el comentario")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del comentario es de 500 caracteres")]
        public string comentario { get; set; }

        public DateTime fechacomentario { get; set; } = DateTime.Now;
        public string fechacomentariocadena { get; set; } = "";
        public string usuario { get; set; }
        public int idtorneo { get; set; }
    }
}
