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
    public class EstatusJuegoController : Controller
    {
        [HttpGet]
        [Route("api/EstatusJuego/ListarEstatusJuego/{idtorneoseleccionado}")]
        public List<EstatusJuegoCLS> ListarEstatusJuego(string idtorneoseleccionado)
        {
            List<EstatusJuegoCLS> listaEstatusJuego = new List<EstatusJuegoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEstatusJuego = (from estatusjuego in baseDatos.Estatusjuego
                                     orderby estatusjuego.Nombre
                                     where estatusjuego.Habilitado == 1 && estatusjuego.Idtorneo == int.Parse(idtorneoseleccionado)
                                     select new EstatusJuegoCLS
                                     {
                                         idestatusjuego = estatusjuego.Idestatusjuego,
                                         nombre = estatusjuego.Nombre
                                     }).ToList();
            }
            return listaEstatusJuego;
        }


        [HttpGet]
        [Route("api/EstatusJuego/FiltrarEstatusJuego/{mensaje?}/{idtorneoseleccionado}")]
        public List<EstatusJuegoCLS> FiltrarEstatusJuego(string mensaje, string idtorneoseleccionado)
        {
            List<EstatusJuegoCLS> listaEstatusJuego = new List<EstatusJuegoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaEstatusJuego = (from estatusjuego in baseDatos.Estatusjuego
                                         orderby estatusjuego.Nombre
                                         where estatusjuego.Habilitado == 1 && estatusjuego.Idtorneo == int.Parse(idtorneoseleccionado)
                                         select new EstatusJuegoCLS
                                         {
                                             idestatusjuego = estatusjuego.Idestatusjuego,
                                             nombre = estatusjuego.Nombre
                                         }).ToList();
                }
                else
                {
                    listaEstatusJuego = (from estatusjuego in baseDatos.Estatusjuego
                                         orderby estatusjuego.Nombre
                                         where estatusjuego.Habilitado == 1 && estatusjuego.Idtorneo == int.Parse(idtorneoseleccionado)
                                         && estatusjuego.Nombre.Contains(mensaje)
                                         select new EstatusJuegoCLS
                                         {
                                             idestatusjuego = estatusjuego.Idestatusjuego,
                                             nombre = estatusjuego.Nombre
                                         }).ToList();
                }
            }
            return listaEstatusJuego;
        }


        [HttpPost]
        [Route("api/EstatusJuego/GuardarDatosEstatusJuego")]
        public int GuardarDatosEstatusJuego([FromBody] EstatusJuegoCLS oEstatusJuegoCLS)

        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oEstatusJuegoCLS.idestatusjuego == 0)
                    {
                        // VER SI ESTA EN LA TABLA CAMPO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Estatusjuego.Where(p => p.Nombre.Trim().Equals(oEstatusJuegoCLS.nombre) && p.Idtorneo == oEstatusJuegoCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Estatusjuego oEstatusJuego = new Estatusjuego();
                            oEstatusJuego.Nombre = oEstatusJuegoCLS.nombre;
                            oEstatusJuego.Torneo = "";       // NO LO VOY A USAR
                            oEstatusJuego.Idtorneo = oEstatusJuegoCLS.idtorneo;
                            oEstatusJuego.Habilitado = 1;

                            // FALTA PONERLE VALOR A DOS CAMPOS DE LA TABLA, VOY A INVESTIGAR PORQUE NO SE ESTAN AGREGANDO
                            baseDatos.Estatusjuego.Add(oEstatusJuego);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        //nveces = baseDatos.Estatusjuego.Where(p => p.Nombre.Trim().Equals(oEstatusJuegoCLS.nombre) && p.Idtorneo == oEstatusJuegoCLS.idtorneo && p.Habilitado == 1).Count();

                        Estatusjuego oEstatusJuego = baseDatos.Estatusjuego.Where(p => p.Idestatusjuego == oEstatusJuegoCLS.idestatusjuego).First();
                        oEstatusJuego.Nombre = oEstatusJuegoCLS.nombre;
                        oEstatusJuego.Habilitado = 1;
                        baseDatos.SaveChanges();
                        rpta = 1;
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
        [Route("api/EstatusJuego/RecuperarInformacionEstatusJuego/{idestatusjuego}")]
        public EstatusJuegoCLS RecuperarInformacionEstatusJuego(int idestatusjuego)
        {
            EstatusJuegoCLS oEstatusJuegoCLS = new EstatusJuegoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oEstatusJuegoCLS = (from estatusjuego in baseDatos.Estatusjuego
                                    where estatusjuego.Idestatusjuego == idestatusjuego
                                    select new EstatusJuegoCLS
                                    {
                                        idestatusjuego = estatusjuego.Idestatusjuego,
                                        nombre = estatusjuego.Nombre
                                    }).First();

                return oEstatusJuegoCLS;
            }
        }


        [HttpGet]
        [Route("api/EstatusJuego/EliminarEstatusJuego/{idestatusjuego}")]
        public int EliminarEstatusJuego(int idestatusjuego)

        {
            int rpta = 0;
            // EL IDCAMPO = 1 ES EL PENDIENTE, ESE NO LO VAMOS A ELIMINAR
            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Estatusjuego oEstatusjuego = baseDatos.Estatusjuego.Where(p => p.Idestatusjuego == idestatusjuego).First();
                    if (oEstatusjuego.Nombre.Trim().Equals("PENDIENTE"))
                    {
                        rpta = 3;
                    }
                    else if (oEstatusjuego.Nombre.Trim().Equals("JUGADO"))
                    {
                        rpta = 4;
                    }
                    else
                    {
                        nveces = baseDatos.Juego.Where(p => p.Idestatusjuego == idestatusjuego && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            oEstatusjuego.Habilitado = 0;
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


    }
}


