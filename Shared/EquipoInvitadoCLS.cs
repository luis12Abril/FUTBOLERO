using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class EquipoInvitadoCLS
    {
        public int idequipo { get; set; }

        public string nombre { get; set; }

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
        public int años { get; set; } = 0;
        public int meses { get; set; } = 0;
        public DateTime fnacimiento { get; set; } = DateTime.Now;
        public List<JugadorCLS> ListaJugadorEquipoInvitado { get; set; } = new List<JugadorCLS>();

    }
}