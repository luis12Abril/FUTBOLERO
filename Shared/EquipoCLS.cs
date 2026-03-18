using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class EquipoCLS
    {
        public int idequipo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre del equipo")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre del equipo es de 50 caracteres")]
        public string nombre { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima de la nombre del representante es de 50 caracteres")]
        public string representante { get; set; } = " ";
        public string fotoequipo { get; set; }
        public int jugados { get; set; } = 0;
        public int ganados { get; set; } = 0;
        public int empatados { get; set; } = 0;
        public int perdidos { get; set; } = 0;
        public int empatadosganados { get; set; } = 0;
        public int empatadosperdidos { get; set; } = 0;
        public int ganadosadmo { get; set; } = 0;
        public int perdidosadmo { get; set; } = 0;
        public int golesafavor { get; set; } = 0;
        public int golesencontra { get; set; } = 0;
        public int difgoles { get; set; } = 0;
        public int puntos { get; set; } = 0;
        public string torneo { get; set; }
        public int años { get; set; } = 0;
        public int meses { get; set; } = 0;
        public List<JugadorCLS> ListaJugadorEquipo { get; set; } = new List<JugadorCLS>();

        public int idtorneo { get; set; }

        public int puntosextras { get; set; }
        public string usuequipo { get; set; } = " ";
        public string claequipo { get; set; } = " ";
        public DateTime vigencia { get; set; } = DateTime.Now;

        public string nomusuario { get; set; }
        public string nomusuariocopia { get; set; }

        public string codigo { get; set; }

    }
}


