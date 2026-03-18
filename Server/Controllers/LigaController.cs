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
//using FutbolObregon.Server.Clases;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class LigaController : Controller
    {
        [HttpGet]
        [Route("api/Liga/ListarLiga")]
        public List<LigaCLS> ListarLiga()
        {
            List<LigaCLS> listaLiga = new List<LigaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaLiga = (from liga in baseDatos.Liga
                                  join municipio in baseDatos.Municipio
                                   on liga.Idmunicipio equals municipio.Idmunicipio
                                  orderby municipio.Nombre
                                  where liga.Habilitado == 1
                                  select new LigaCLS
                                  {
                                      idliga = liga.Idliga,
                                      nombre = liga.Nombre,
                                      municipio = municipio.Nombre,
                                  }).ToList();
            }
            return listaLiga;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Liga/FiltrarLiga/{p_idmunicipio?}")]
        public List<LigaCLS> FiltrarLiga(string p_idmunicipio)
        {
            List<LigaCLS> listaLiga = new List<LigaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idmunicipio == null || p_idmunicipio == "--- Seleccione ---")
                {
                    listaLiga = (from liga in baseDatos.Liga
                                      join municipio in baseDatos.Municipio
                                      on liga.Idmunicipio equals municipio.Idmunicipio
                                      orderby municipio.Nombre
                                      where liga.Habilitado == 1
                                      select new LigaCLS
                                      {
                                          idliga = liga.Idliga,
                                          nombre = liga.Nombre,
                                          municipio = municipio.Nombre,
                                      }).ToList();
                }
                else
                {
                    listaLiga = (from liga in baseDatos.Liga
                                     join municipio in baseDatos.Municipio
                                     on liga.Idmunicipio equals municipio.Idmunicipio
                                     orderby municipio.Nombre
                                     where liga.Habilitado == 1 && liga.Idmunicipio == int.Parse(p_idmunicipio)
                                      select new LigaCLS
                                      {
                                          idliga = liga.Idliga,
                                          nombre = liga.Nombre,
                                          municipio = municipio.Nombre,
                                      }).ToList();
                }
            }
            return listaLiga;
        }


        [HttpPost]
        [Route("api/Liga/GuardarDatosLiga")]
        public int GuardarDatosLiga([FromBody] LigaCLS oLigaCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oLigaCLS.idliga == 0)
                    {
                        // VER SI ESTA EN LA TABLA LIGA, ESE NOMBRE COMPLETO DE LA LIGA Y QUE ESTE HABILITADO
                        nveces = baseDatos.Liga.Where(p => (p.Nombre.Trim()).Equals(oLigaCLS.nombre.Trim()) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Liga oLiga = new Liga();
                            oLiga.Nombre = oLigaCLS.nombre;
                            oLiga.Idmunicipio = int.Parse(oLigaCLS.idmunicipio);
                            oLiga.Habilitado = 1;
                            baseDatos.Liga.Add(oLiga);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA LIGA, ESE NOMBRE COMPLETO DE LA LIGA Y QUE ESTE HABILITADO
                        nveces = baseDatos.Liga.Where(p => (p.Nombre.Trim()).Equals(oLigaCLS.nombre.Trim())
                        && p.Idliga != oLigaCLS.idliga && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Liga oLiga = baseDatos.Liga.Where(p => p.Idliga == oLigaCLS.idliga).First();
                            oLiga.Nombre = oLigaCLS.nombre;
                            oLiga.Idmunicipio = int.Parse(oLigaCLS.idmunicipio);
                            oLiga.Habilitado = 1;
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
        [Route("api/Liga/RecuperarInformacionLiga/{idLiga}")]
        public LigaCLS RecuperarInformacionLiga(int idLiga)
        {
            LigaCLS oLigaCLS = new LigaCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oLigaCLS = (from liga in baseDatos.Liga
                                 where liga.Idliga == idLiga
                                 select new LigaCLS
                                 {
                                     idliga = liga.Idliga,
                                     nombre = liga.Nombre,
                                     idmunicipio = liga.Idmunicipio.ToString()

                                 }).First();

                return oLigaCLS;
            }
        }

        [HttpGet]
        [Route("api/Liga/EliminarLiga/{idLiga}/{idtorneoseleccionado}")]
        public int EliminarLiga(int idLiga, string idtorneoseleccionado)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Liga oLiga = baseDatos.Liga.Where(p => p.Idliga == idLiga).First();
                    oLiga.Habilitado = 0;
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
