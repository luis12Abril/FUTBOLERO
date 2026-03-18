using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class UsuarioCLS
    {
        public int idusuario { get; set; }

        [Required(ErrorMessage = "debe ingresar el nombre del usuario")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "debe ingresar una contraseña")]
        [MaxLength(15, ErrorMessage = "La longitud máxima de la contraseña es de 15 caracteres")]
        //[MinLength(4, ErrorMessage = "La contraseña deba tener minimo 4 caracteres")]
        public string contraseña { get; set; }
        public string tipousuariocadena { get; set; }

        // ESTA OPCION ESTA COMENTADA PORQUE CUANDO QUIERO ENTRAR EN EL LOGIN SE QUEDA EN ESPERA Y NO AVANZA
        //[Required(ErrorMessage = "debe asignarle un tipo de usuario al sistema")]
        public string idtipousuario { get; set; }
        public List<int> listaidtorneo { get; set; } = new List<int>();          // ES PARA SELECCIONAR QUE TORNEOS PUEDE ADMINISTRAR EL USUARIO
        public int visitas { get; set; }
        public int visitascel { get; set; }
        public string idarbitrocolegio { get; set; }

        public DateTime fechaalta { get; set; }
        public string origenalta { get; set; }
        
    }
}
