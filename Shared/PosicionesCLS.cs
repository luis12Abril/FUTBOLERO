using System;
using System.Collections.Generic;
using System.Text;

namespace FUTBOLERO.Shared
{
    public class PosicionesCLS
    {
        public string nombre { get; set; }
        public int jugados { get; set; }
        public int ganados { get; set; }
        public int empatados { get; set; }
        public int perdidos { get; set; }
        public int empatadosganados { get; set; }
        public int golesafavor { get; set; }
        public int golesencontra { get; set; }
        public int diferenciagoles { get; set; }
        public int puntos { get; set; }
        public string torneo { get; set; } = "100";
        public int idtorneo { get; set; }
        public int puntosextras { get; set; }

    }
}

