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
    public class PublicidadController : Controller
    {
        [HttpGet]
        [Route("api/Publicidad/ListarPublicidad")]
        public List<PublicidadCLS> ListarPublicidad()
        {
            List<PublicidadCLS> listaPublicidad = new List<PublicidadCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaPublicidad = (from publicidad in baseDatos.Publicidad
                                   join torneo in baseDatos.Torneo
                                   on publicidad.Idtorneo equals torneo.Idtorneo
                                   orderby torneo.Nombre, publicidad.Orden
                                   where torneo.Visible == 1 && publicidad.Habilitado == 1
                                   select new PublicidadCLS
                                   {
                                       idpublicidad = publicidad.Idpublicidad,
                                       torneo = torneo.Nombre,
                                       orden = publicidad.Orden
                                   }).ToList();
            }
            return listaPublicidad;
        }


        [HttpGet]
        [Route("api/Publicidad/FiltrarPublicidad/{mensaje?}")]
        public List<PublicidadCLS> FiltrarPublicidad(string mensaje)
        {
            List<PublicidadCLS> listaPublicidad = new List<PublicidadCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaPublicidad = (from publicidad in baseDatos.Publicidad
                                       join torneo in baseDatos.Torneo
                                       on publicidad.Idtorneo equals torneo.Idtorneo
                                       orderby torneo.Nombre, publicidad.Orden
                                       where torneo.Visible == 1 && publicidad.Habilitado == 1
                                       select new PublicidadCLS
                                       {
                                           idpublicidad = publicidad.Idpublicidad,
                                           torneo = torneo.Nombre,
                                           orden = publicidad.Orden
                                       }).ToList();
                }
                else
                {
                    listaPublicidad = (from publicidad in baseDatos.Publicidad
                                       join torneo in baseDatos.Torneo
                                       on publicidad.Idtorneo equals torneo.Idtorneo
                                       orderby torneo.Nombre, publicidad.Orden
                                       where torneo.Visible == 1 && publicidad.Habilitado == 1 && torneo.Nombre.Contains(mensaje)
                                       select new PublicidadCLS
                                       {
                                           idpublicidad = publicidad.Idpublicidad,
                                           torneo = torneo.Nombre,
                                           orden = publicidad.Orden
                                       }).ToList();
                }
            }
            return listaPublicidad;
        }


        [HttpPost]
        [Route("api/Publicidad/GuardarDatosPublicidad")]
        public int GuardarDatosPublicidad([FromBody] PublicidadCLS oPublicidadCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oPublicidadCLS.idpublicidad == 0)
                    {
                        Publicidad oPublicidad = new Publicidad();
                        oPublicidad.Idtorneo = Convert.ToInt32(oPublicidadCLS.idtorneo);
                        oPublicidad.Foto = oPublicidadCLS.foto;
                        oPublicidad.Orden = oPublicidadCLS.orden;
                        oPublicidad.Habilitado = 1;
                        baseDatos.Publicidad.Add(oPublicidad);
                        baseDatos.SaveChanges();
                        rpta = 1;
                    }
                    else
                    {
                        Publicidad oPublicidad = baseDatos.Publicidad.Where(p => p.Idpublicidad == oPublicidadCLS.idpublicidad).First();
                        oPublicidad.Idtorneo = Convert.ToInt32(oPublicidadCLS.idtorneo);
                        oPublicidad.Foto = oPublicidadCLS.foto;
                        oPublicidad.Orden = oPublicidadCLS.orden;
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
        [Route("api/Publicidad/RecuperarInformacionPublicidad/{idPublicidad}")]
        public PublicidadCLS RecuperarInformacionPublicidad(int idPublicidad)
        {
            PublicidadCLS oPublicidadCLS = new PublicidadCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oPublicidadCLS = (from publicidad in baseDatos.Publicidad
                                  join torneo in baseDatos.Torneo
                                   on publicidad.Idtorneo equals torneo.Idtorneo
                                  where publicidad.Idpublicidad == idPublicidad
                                  select new PublicidadCLS
                                  {
                                      idpublicidad = publicidad.Idpublicidad,
                                      idtorneo = publicidad.Idtorneo.ToString(),
                                      foto = publicidad.Foto,
                                      torneo = torneo.Nombre,
                                      orden = publicidad.Orden

                                  }).First();

                return oPublicidadCLS;
            }
        }



        [HttpGet]
        [Route("api/Publicidad/EliminarPublicidad/{idPublicidad}")]
        public int EliminarPublicidad(int idPublicidad)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Publicidad oPublicidad = baseDatos.Publicidad.Where(p => p.Idpublicidad == idPublicidad).First();
                    oPublicidad.Habilitado = 0;
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
