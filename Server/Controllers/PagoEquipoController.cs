using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class PagoEquipoController : Controller
    {
        // GET: Listar pagos de un equipo (historial de abonos)
        [HttpGet]
        [Route("api/PagoEquipo/ListarPagos/{idcuotaequipo}")]
        public List<PagoEquipoCLS> ListarPagos(int idcuotaequipo)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var lista = (from p in db.Pagoequipo
                             where p.Idcuotaequipo == idcuotaequipo
                             orderby p.Fechapago descending
                             select new PagoEquipoCLS
                             {
                                 idpago = p.Idpago,
                                 idcuotaequipo = p.Idcuotaequipo,
                                 idtorneo = p.Idtorneo,
                                 montopagado = p.Montopagado,
                                 fechapago = p.Fechapago,
                                 referencia = p.Referencia,
                                 metodopago = p.Metodopago,
                                 observaciones = p.Observaciones,
                                 usuarioregistro = p.Usuarioregistro
                             }).ToList();

                return lista;
            }
        }

        // POST: Registrar un abono
        [HttpPost]
        [Route("api/PagoEquipo/RegistrarPago")]
        public int RegistrarPago([FromBody] PagoEquipoCLS oPago)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var nuevo = new Pagoequipo
                {
                    Idcuotaequipo = oPago.idcuotaequipo,
                    Idtorneo = oPago.idtorneo,
                    Montopagado = oPago.montopagado,
                    Fechapago = oPago.fechapago,
                    Referencia = oPago.referencia,
                    Metodopago = oPago.metodopago,
                    Observaciones = oPago.observaciones,
                    Usuarioregistro = oPago.usuarioregistro
                };
                db.Pagoequipo.Add(nuevo);
                db.SaveChanges();
                return nuevo.Idpago;
            }
        }

        // PUT: Actualizar un pago registrado
        [HttpPut]
        [Route("api/PagoEquipo/ActualizarPago")]
        public bool ActualizarPago([FromBody] PagoEquipoCLS oPago)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var pago = db.Pagoequipo.Find(oPago.idpago);
                if (pago == null) return false;
                pago.Montopagado = oPago.montopagado;
                pago.Fechapago = oPago.fechapago;
                pago.Referencia = oPago.referencia;
                pago.Metodopago = oPago.metodopago;
                pago.Observaciones = oPago.observaciones;
                db.SaveChanges();
                return true;
            }
        }

        // DELETE: Eliminar un pago registrado
        [HttpDelete]
        [Route("api/PagoEquipo/EliminarPago/{idpago}")]
        public bool EliminarPago(int idpago)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var pago = db.Pagoequipo.Find(idpago);
                if (pago == null) return false;
                db.Pagoequipo.Remove(pago);
                db.SaveChanges();
                return true;
            }
        }
    }
}
