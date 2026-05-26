using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class CuotaEquipoController : Controller
    {
        // GET: Listar cuotas de todos los equipos de una cuota de torneo (resumen de pagos)
        [HttpGet]
        [Route("api/CuotaEquipo/ListarResumen/{idcuotatorneo}")]
        public List<CuotaEquipoCLS> ListarResumen(int idcuotatorneo)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                // Cargar datos base
                var equipos = (from ce in db.Cuotaequipo
                               join e in db.Equipo on ce.Idequipo equals e.Idequipo
                               where ce.Idcuotatorneo == idcuotatorneo
                               orderby e.Nombre
                               select new CuotaEquipoCLS
                               {
                                   idcuotaequipo = ce.Idcuotaequipo,
                                   idcuotatorneo = ce.Idcuotatorneo,
                                   idequipo = ce.Idequipo,
                                   nombreEquipo = e.Nombre,
                                   representante = e.Representante,
                                   montoasignado = ce.Montoasignado,
                                   observaciones = ce.Observaciones
                               }).ToList();

                // Obtener ids para traer pagos en una sola consulta
                var ids = equipos.Select(x => x.idcuotaequipo).ToList();
                var pagos = db.Pagoequipo
                    .Where(p => ids.Contains(p.Idcuotaequipo))
                    .GroupBy(p => p.Idcuotaequipo)
                    .Select(g => new { idcuotaequipo = g.Key, total = g.Sum(p => p.Montopagado) })
                    .ToList();

                // Calcular totales y estatus en memoria
                foreach (var item in equipos)
                {
                    var pago = pagos.FirstOrDefault(p => p.idcuotaequipo == item.idcuotaequipo);
                    item.totalPagado = pago?.total ?? 0;
                    item.saldoPendiente = item.montoasignado - item.totalPagado;

                    if (item.saldoPendiente <= 0)
                        item.estatusPago = "Liquidado";
                    else if (item.totalPagado > 0)
                        item.estatusPago = "Parcial";
                    else
                        item.estatusPago = "Sin pago";
                }

                return equipos;
            }
        }

        // GET: Obtener la cuota de un equipo específico
        [HttpGet]
        [Route("api/CuotaEquipo/ObtenerCuotaEquipo/{idcuotaequipo}")]
        public CuotaEquipoCLS ObtenerCuotaEquipo(int idcuotaequipo)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var ce = (from c in db.Cuotaequipo
                          join e in db.Equipo on c.Idequipo equals e.Idequipo
                          where c.Idcuotaequipo == idcuotaequipo
                          select new CuotaEquipoCLS
                          {
                              idcuotaequipo = c.Idcuotaequipo,
                              idcuotatorneo = c.Idcuotatorneo,
                              idequipo = c.Idequipo,
                              nombreEquipo = e.Nombre,
                              representante = e.Representante,
                              montoasignado = c.Montoasignado,
                              observaciones = c.Observaciones
                          }).FirstOrDefault();

                if (ce != null)
                {
                    ce.totalPagado = db.Pagoequipo
                        .Where(p => p.Idcuotaequipo == idcuotaequipo)
                        .Sum(p => (decimal?)p.Montopagado) ?? 0;
                    ce.saldoPendiente = ce.montoasignado - ce.totalPagado;

                    if (ce.saldoPendiente <= 0)
                        ce.estatusPago = "Liquidado";
                    else if (ce.totalPagado > 0)
                        ce.estatusPago = "Parcial";
                    else
                        ce.estatusPago = "Sin pago";
                }

                return ce;
            }
        }

        // POST: Asignar cuota a un equipo (o actualizar)
        [HttpPost]
        [Route("api/CuotaEquipo/GuardarCuotaEquipo")]
        public int GuardarCuotaEquipo([FromBody] CuotaEquipoCLS oCuota)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                if (oCuota.idcuotaequipo == 0)
                {
                    var nueva = new Cuotaequipo
                    {
                        Idcuotatorneo = oCuota.idcuotatorneo,
                        Idequipo = oCuota.idequipo,
                        Montoasignado = oCuota.montoasignado,
                        Observaciones = oCuota.observaciones
                    };
                    db.Cuotaequipo.Add(nueva);
                    db.SaveChanges();
                    return nueva.Idcuotaequipo;
                }
                else
                {
                    var existente = db.Cuotaequipo.Find(oCuota.idcuotaequipo);
                    if (existente != null)
                    {
                        existente.Montoasignado = oCuota.montoasignado;
                        existente.Observaciones = oCuota.observaciones;
                        db.SaveChanges();
                    }
                    return existente.Idcuotaequipo;
                }
            }
        }

        // POST: Actualizar el monto asignado a TODOS los equipos ya existentes (sin tocar abonos)
        [HttpPost]
        [Route("api/CuotaEquipo/ActualizarMontoATodos/{idcuotatorneo}/{nuevoMonto}")]
        public int ActualizarMontoATodos(int idcuotatorneo, decimal nuevoMonto)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var equipos = db.Cuotaequipo
                    .Where(c => c.Idcuotatorneo == idcuotatorneo)
                    .ToList();

                foreach (var eq in equipos)
                    eq.Montoasignado = nuevoMonto;

                db.SaveChanges();
                return equipos.Count;
            }
        }

        // POST: Asignar la cuota base a TODOS los equipos del torneo de una sola vez
        [HttpPost]
        [Route("api/CuotaEquipo/AsignarATodos/{idcuotatorneo}/{idtorneo}")]
        public int AsignarATodos(int idcuotatorneo, int idtorneo)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                var cuota = db.Cuotatorneo.Find(idcuotatorneo);
                if (cuota == null) return 0;

                var equipos = db.Equipo
                    .Where(e => e.Idtorneo == idtorneo && e.Habilitado == 1 && !e.Nombre.Contains("_SIN EQUIPO"))
                    .ToList();

                int contador = 0;
                foreach (var eq in equipos)
                {
                    bool yaExiste = db.Cuotaequipo.Any(c => c.Idcuotatorneo == idcuotatorneo && c.Idequipo == eq.Idequipo);
                    if (!yaExiste)
                    {
                        db.Cuotaequipo.Add(new Cuotaequipo
                        {
                            Idcuotatorneo = idcuotatorneo,
                            Idequipo = eq.Idequipo,
                            Montoasignado = cuota.Montobase,
                            Observaciones = ""
                        });
                        contador++;
                    }
                }
                db.SaveChanges();
                return contador;
            }
        }
    }
}
