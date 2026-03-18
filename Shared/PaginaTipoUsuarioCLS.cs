using System;
using System.Collections.Generic;
using System.Text;


namespace FUTBOLERO.Shared
{
    public class PaginaTipoUsuarioCLS
    {
        public int idpaginatipousuario { get; set; }

        public string nombrepagina { get; set; }

        public string nombretipousuario { get; set; }

        public List<int> idboton { get; set; } = new List<int>();
    }
}
