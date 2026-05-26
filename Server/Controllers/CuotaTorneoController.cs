using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class CuotaTorneoController : Controller
    {
        // GET: Obtener la cuota activa de un torneo
        [HttpGet]
        [Route("api/CuotaTorneo/ObtenerCuota/{idtorneo}")]
        public CuotaTorneoCLS ObtenerCuota(int idtorneo)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                // Primero traer la entidad, luego proyectar en memoria para evitar problemas de traducción EF
                var cuota = db.Cuotatorneo
                    .Where(c => c.Idtorneo == idtorneo && c.Activo == true)
                    .Join(db.Torneo, c => c.Idtorneo, t => t.Idtorneo, (c, t) => new { c, t })
                    .FirstOrDefault();

                if (cuota == null) return null;

                return new CuotaTorneoCLS
                {
                    idcuotatorneo = cuota.c.Idcuotatorneo,
                    idtorneo      = cuota.c.Idtorneo,
                    nombreTorneo  = cuota.t.Nombre,
                    montobase     = cuota.c.Montobase,
                    concepto      = cuota.c.Concepto,
                    fechalimite   = cuota.c.Fechalimite.Year > 1900 ? (DateTime?)cuota.c.Fechalimite : null,
                    activo        = cuota.c.Activo
                };
            }
        }

        // POST: Guardar o actualizar la cuota de un torneo
        [HttpPost]
        [Route("api/CuotaTorneo/GuardarCuota")]
        public int GuardarCuota([FromBody] CuotaTorneoCLS oCuota)
        {
            using (var db = new FUTBOLEANDOContext())
            {
                // Desactivar cuotas anteriores del mismo torneo
                var cuotasAnteriores = db.Cuotatorneo
                    .Where(c => c.Idtorneo == oCuota.idtorneo && c.Activo == true)
                    .ToList();

                foreach (var ca in cuotasAnteriores)
                    ca.Activo = false;

                if (oCuota.idcuotatorneo == 0)
                {
                    // Nueva cuota
                    var nueva = new Cuotatorneo
                    {
                        Idtorneo = oCuota.idtorneo,
                        Montobase = oCuota.montobase,
                        Concepto = oCuota.concepto,
                        Fechalimite = oCuota.fechalimite ?? new DateTime(1900, 1, 1),
                        Activo = true
                    };
                    db.Cuotatorneo.Add(nueva);
                    db.SaveChanges();
                    return nueva.Idcuotatorneo;
                }
                else
                {
                    // Actualizar existente
                    var existente = db.Cuotatorneo.Find(oCuota.idcuotatorneo);
                    if (existente != null)
                    {
                        existente.Montobase = oCuota.montobase;
                        existente.Concepto = oCuota.concepto;
                        existente.Fechalimite = oCuota.fechalimite ?? new DateTime(1900, 1, 1);
                        existente.Activo = true;
                        db.SaveChanges();
                    }
                    return existente.Idcuotatorneo;
                }
            }
        }
    }
}
