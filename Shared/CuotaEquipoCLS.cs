using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class CuotaEquipoCLS
    {
        public int idcuotaequipo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una cuota de torneo")]
        public int idcuotatorneo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public int idequipo { get; set; }
        public string nombreEquipo { get; set; }
        public string representante { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto asignado")]
        [Range(0.01, 9999999, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal montoasignado { get; set; }

        [MaxLength(300, ErrorMessage = "Las observaciones no pueden superar 300 caracteres")]
        public string observaciones { get; set; }

        // Campos calculados para el resumen
        public decimal totalPagado { get; set; }
        public decimal saldoPendiente { get; set; }
        public string estatusPago { get; set; }  // "Liquidado", "Parcial", "Sin pago"
    }
}
