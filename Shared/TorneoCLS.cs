using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class TorneoCLS
    {
        public int idtorneo { get; set; }
        [MaxLength(100, ErrorMessage = "La longitud máxima del apellido paterno del jugador es de 100 caracteres")]
        [Required(ErrorMessage = "debe ingresar el nombre del torneo")]
        public string nombre { get; set; }
        public string clavetorneo { get; set; } = "";    // YA NO LO ESTOY USANDO
        public bool visible { get; set; }          // ME SERVIRA PARA LISTAR
        public string torneovisible { get; set; }   // ESTA PROPIEDAD SERA PARA EL LISTADO
        public int ordentorneo { get; set; }     // ESTE SI ES PARA ORDENAR LOS TORNEOS A MOSTRAR
        public int visitas { get; set; }     // ESTE PARA MOSTRAR LAS VISITAS AL TORNEO

        //[Required(ErrorMessage = "Debe ingresar la liga a la que pertenece el torneo")]
        public string idligaTorneo { get; set; }

        public string idestado { get; set; }
        public string estado { get; set; }

        public string idmunicipio { get; set; }
        public string municipio { get; set; }

        [Required(ErrorMessage = "Debe ingresar la liga a la que pertenece el torneo")]
        public string idliga { get; set; }
        public string liga { get; set; }
    }
}