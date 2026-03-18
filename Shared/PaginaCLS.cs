using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class PaginaCLS
    {
        public int idpagina { get; set; }
        [Required(ErrorMessage = "Debe ingresar el mensaje que se vera en el menu de la página")]
        [MaxLength(100, ErrorMessage = "La longitud máxima del mensaje es de 100")]
        public string mensaje { get; set; }
        [Required(ErrorMessage = "Debe ingresar a que página o componente se dirigira esta página, ej. /ListaUsuario")]
        [MaxLength(100, ErrorMessage = "La longitud máxima de la acción es de 100")]
        public string accion { get; set; }          // ME SERVIRA PARA AGREGAR
        public bool visible { get; set; }          // ME SERVIRA PARA LISTAR
        public string nombrevisible { get; set; }       // ESTA PROPIEDAD SERA PARA EL LISTADO
        public int ordenmenu { get; set; } = 0;      // ESTA PROPIEDAD SERA ORDENAR EN EL MENU
    }
}
