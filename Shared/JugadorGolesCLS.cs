using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class JugadorGolesCLS
    {
        public int idjugador { get; set; }

        public int idequipo { get; set; }

        public int goles { get; set; }

        public string nombrecompleto { get; set; }

    }
}