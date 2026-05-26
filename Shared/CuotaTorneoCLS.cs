using System;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class CuotaTorneoCLS
    {
        public int idcuotatorneo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un torneo")]
        public int idtorneo { get; set; }
        public string nombreTorneo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto base")]
        [Range(0.01, 9999999, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal montobase { get; set; }

        [MaxLength(200, ErrorMessage = "El concepto no puede superar 200 caracteres")]
        public string concepto { get; set; }

        public DateTime? fechalimite { get; set; }

        public bool activo { get; set; } = true;
    }
}
