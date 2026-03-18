using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;
using System.Text;
using System.Transactions;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class TorneoColegioController : Controller
    {
        [HttpGet]
        [Route("api/TorneoColegio/ListaTorneoColegio")]
        public List<TorneoColegioCLS> ListaTorneoColegio()
        {
            List<TorneoColegioCLS> listaTorneoColegio = new List<TorneoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaTorneoColegio = (from torneocolegio in baseDatos.Torneocolegio
                                    join liga in baseDatos.Ligacolegio
                                    on torneocolegio.Idligacolegio equals liga.Idligacolegio
                                    orderby liga.Nombre
                                    where torneocolegio.Habilitado == 1
                                    select new TorneoColegioCLS
                                    {
                                        idtorneocolegio = torneocolegio.Idtorneocolegio,
                                        nombre = torneocolegio.Nombre,
                                        liga = liga.Nombre
                                    }).ToList();
            }
            return listaTorneoColegio;
        }


        [HttpPost]
        [Route("api/TorneoColegio/GuardarDatosTorneoColegio")]
        public int GuardarDatosTorneoColegio([FromBody] TorneoColegioCLS oTorneoColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oTorneoColegioCLS.idtorneocolegio == 0)
                    {
                        // VER SI ESTA EN LA TABLA LIGACOLEGIO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Torneocolegio.Where(p => (p.Nombre.Trim()).Equals(oTorneoColegioCLS.nombre.Trim()) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Torneocolegio oTorneoColegio = new Torneocolegio();
                            oTorneoColegio.Nombre = oTorneoColegioCLS.nombre;
                            oTorneoColegio.Idligacolegio = int.Parse(oTorneoColegioCLS.idligacolegio);
                            oTorneoColegio.Habilitado = 1;
                            baseDatos.Torneocolegio.Add(oTorneoColegio);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Torneocolegio.Where(p => (p.Nombre.Trim()).Equals(oTorneoColegioCLS.nombre.Trim())
                      && p.Idtorneocolegio != oTorneoColegioCLS.idtorneocolegio && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Torneocolegio oTorneoColegio = baseDatos.Torneocolegio.Where(p => p.Idtorneocolegio == oTorneoColegioCLS.idtorneocolegio).First();
                            oTorneoColegio.Nombre = oTorneoColegioCLS.nombre;
                            oTorneoColegio.Idligacolegio = int.Parse(oTorneoColegioCLS.idligacolegio);
                            oTorneoColegio.Habilitado = 1;
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }


        [HttpGet]
        [Route("api/TorneoColegio/RecuperarInformacionTorneoColegio/{idTorneoColegio}")]
        public TorneoColegioCLS RecuperarInformacionTorneoColegio(int idTorneoColegio)
        {
            TorneoColegioCLS oTorneoColegioCLS = new TorneoColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oTorneoColegioCLS = (from torneocolegio in baseDatos.Torneocolegio
                                 where torneocolegio.Idtorneocolegio == idTorneoColegio
                                 select new TorneoColegioCLS
                                 {
                                     idtorneocolegio = torneocolegio.Idtorneocolegio,
                                     nombre = torneocolegio.Nombre,
                                     idligacolegio = torneocolegio.Idligacolegio.ToString()
                                 }).First();

                return oTorneoColegioCLS;
            }
        }

        [HttpGet]
        [Route("api/TorneoColegio/EliminarTorneoColegio/{idTorneoColegio}")]
        public int EliminarTorneoColegio(int idTorneoColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Torneocolegio oTorneoColegio = baseDatos.Torneocolegio.Where(p => p.Idtorneocolegio == idTorneoColegio).First();
                    oTorneoColegio.Habilitado = 0;
                    baseDatos.SaveChanges();
                    rpta = 1;
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }


    }
}
