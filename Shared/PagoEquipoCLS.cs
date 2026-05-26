using System;
using System.ComponentModel.DataAnnotations;

namespace FUTBOLERO.Shared
{
    public class PagoEquipoCLS
    {
        public int idpago { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la cuota del equipo")]
        public int idcuotaequipo { get; set; }
        public int idtorneo { get; set; }

        // Datos de contexto para mostrar en pantalla
        public string nombreEquipo { get; set; }
        public decimal montoasignado { get; set; }
        public decimal totalPagado { get; set; }
        public decimal saldoPendiente { get; set; }

        [Required(ErrorMessage = "Debe ingresar el monto pagado")]
        [Range(0.01, 9999999, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal montopagado { get; set; }

        public DateTime fechapago { get; set; } = DateTime.Now;

        [MaxLength(100, ErrorMessage = "La referencia no puede superar 100 caracteres")]
        public string referencia { get; set; }

        [MaxLength(50, ErrorMessage = "El método de pago no puede superar 50 caracteres")]
        public string metodopago { get; set; } = "Efectivo";

        [MaxLength(300, ErrorMessage = "Las observaciones no pueden superar 300 caracteres")]
        public string observaciones { get; set; }

        public string usuarioregistro { get; set; }
    }
}
