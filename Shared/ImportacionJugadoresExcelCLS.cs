using System.Collections.Generic;

namespace FUTBOLERO.Shared
{
    public class ImportacionJugadoresExcelCLS
    {
        public int totalFilas { get; set; }
        public int insertados { get; set; }
        public int rechazados { get; set; }
        public List<string> errores { get; set; } = new List<string>();
    }
}
