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
//using FUTBOLEANDO.Server.Clases;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class CampoController : Controller
    {
        [HttpGet]
        [Route("api/Campo/ListarCampo/{idtorneoseleccionado}")]
        public List<CampoCLS> ListarCampo(string idtorneoseleccionado)
        {
            List<CampoCLS> listaCampo = new List<CampoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaCampo = (from campo in baseDatos.Campo
                              orderby campo.Nombre
                              where campo.Habilitado == 1 && campo.Idtorneo == int.Parse(idtorneoseleccionado)
                              select new CampoCLS
                              {
                                  idcampo = campo.Idcampo,
                                  nombre = campo.Nombre,
                                  ubicacion = campo.Ubicacion
                              }).ToList();
            }
            return listaCampo;
        }


        [HttpGet]
        [Route("api/Campo/FiltrarCampo/{mensaje?}/{idtorneoseleccionado}")]
        public List<CampoCLS> FiltrarCampo(string mensaje, string idtorneoseleccionado)
        {
            List<CampoCLS> listaCampo = new List<CampoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaCampo = (from campo in baseDatos.Campo
                                  orderby campo.Nombre
                                  where campo.Habilitado == 1 && campo.Idtorneo == int.Parse(idtorneoseleccionado)
                                  select new CampoCLS
                                  {
                                      idcampo = campo.Idcampo,
                                      nombre = campo.Nombre,
                                      ubicacion = campo.Ubicacion
                                  }).ToList();
                }
                else
                {
                    listaCampo = (from campo in baseDatos.Campo
                                  orderby campo.Nombre
                                  where campo.Habilitado == 1 && campo.Nombre.Contains(mensaje) && campo.Idtorneo == int.Parse(idtorneoseleccionado)
                                  select new CampoCLS
                                  {
                                      idcampo = campo.Idcampo,
                                      nombre = campo.Nombre,
                                      ubicacion = campo.Ubicacion
                                  }).ToList();
                }
            }
            return listaCampo;
        }


        [HttpPost]
        [Route("api/Campo/GuardarDatosCampo")]
        public int GuardarDatosCampo([FromBody] CampoCLS oCampoCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oCampoCLS.idcampo == 0)
                    {
                        // VER SI ESTA EN LA TABLA CAMPO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Campo.Where(p => p.Nombre.Trim().Equals(oCampoCLS.nombre) && p.Idtorneo == oCampoCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Campo oCampo = new Campo();
                            oCampo.Nombre = oCampoCLS.nombre;
                            oCampo.Ubicacion = (oCampoCLS.ubicacion == null ? " " : oCampoCLS.ubicacion);
                            oCampo.Torneo = "";       // NO LO VOY A USAR
                            oCampo.Idtorneo = oCampoCLS.idtorneo;
                            oCampo.Habilitado = 1;
                            baseDatos.Campo.Add(oCampo);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA CAMPO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Campo.Where(p => p.Nombre.Trim().Equals(oCampoCLS.nombre) && p.Idcampo != oCampoCLS.idcampo
                        && p.Idtorneo == oCampoCLS.idtorneo && p.Habilitado == 1).Count();

                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Campo oCampo = baseDatos.Campo.Where(p => p.Idcampo == oCampoCLS.idcampo).First();
                            oCampo.Nombre = oCampoCLS.nombre;
                            oCampo.Ubicacion = (oCampoCLS.ubicacion == null ? " " : oCampoCLS.ubicacion);     //ASI PORQUE NO ES REQUERIDO
                            oCampo.Habilitado = 1;
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
        [Route("api/Campo/RecuperarInformacionCampo/{idcampo}")]
        public CampoCLS RecuperarInformacionCampo(int idcampo)
        {
            CampoCLS oCampoCLS = new CampoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oCampoCLS = (from campo in baseDatos.Campo
                             where campo.Idcampo == idcampo
                             select new CampoCLS
                             {
                                 idcampo = campo.Idcampo,
                                 nombre = campo.Nombre,
                                 ubicacion = campo.Ubicacion,
                                 torneo = campo.Torneo
                             }).First();

                return oCampoCLS;
            }
        }


        [HttpGet]
        [Route("api/Campo/EliminarCampo/{idcampo}")]
        public int EliminarCampo(int idcampo)
        {
            int rpta = 0;
            // EL IDCAMPO = 1 ES EL PENDIENTE, ESE NO LO VAMOS A ELIMINAR
            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Campo oCampo = baseDatos.Campo.Where(p => p.Idcampo == idcampo).First();
                    if (oCampo.Nombre.Trim().Equals("PENDIENTE"))
                    {
                        rpta = 3;
                    }
                    else
                    {
                        nveces = baseDatos.Juego.Where(p => p.Idcampo == idcampo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            oCampo.Habilitado = 0;
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
