using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class UsuarioEACLS
    {
        public int idusuario { get; set; }
        public string usuequipo { get; set; }
        public int? idtipousuario { get; set; }
    }
}
