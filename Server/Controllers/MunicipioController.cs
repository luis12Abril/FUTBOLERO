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
    public class MunicipioController : Controller
    {
        [HttpGet]
        [Route("api/Municipio/ListarMunicipio")]
        public List<MunicipioCLS> ListarMunicipio()
        {
            List<MunicipioCLS> listaMunicipio = new List<MunicipioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaMunicipio = (from municipio in baseDatos.Municipio
                                join estado in baseDatos.Estado
                                 on municipio.Idestado equals estado.Idestado
                                orderby estado.Nombre
                                where municipio.Habilitado == 1 
                                select new MunicipioCLS
                                {
                                    idmunicipio = municipio.Idmunicipio,
                                    nombre = municipio.Nombre,
                                    estado = estado.Nombre
                                }).ToList();
            }
            return listaMunicipio;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Municipio/FiltrarMunicipio/{p_idestado}")]
        public List<MunicipioCLS> FiltrarMunicipio(string p_idestado)
        {
            List<MunicipioCLS> listaMunicipio = new List<MunicipioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idestado == null || p_idestado == "--- Seleccione ---")
                {
                    listaMunicipio = (from municipio in baseDatos.Municipio
                                    join estado in baseDatos.Estado
                                    on municipio.Idestado equals estado.Idestado
                                    orderby estado.Nombre
                                    where municipio.Habilitado == 1
                                    select new MunicipioCLS
                                    {
                                        idmunicipio = municipio.Idmunicipio,
                                        nombre = municipio.Nombre,
                                        estado = estado.Nombre
                                    }).ToList();
                }
                else
                {
                    listaMunicipio = (from municipio in baseDatos.Municipio
                                      join estado in baseDatos.Estado
                                      on municipio.Idestado equals estado.Idestado
                                      orderby estado.Nombre
                                      where municipio.Habilitado == 1 && municipio.Idestado == int.Parse(p_idestado)
                                      select new MunicipioCLS
                                      {
                                          idmunicipio = municipio.Idmunicipio,
                                          nombre = municipio.Nombre,
                                          estado = estado.Nombre
                                      }).ToList();
                }
            }
            return listaMunicipio;
        }



        [HttpPost]
        [Route("api/Municipio/GuardarDatosMunicipio")]
        public int GuardarDatosMunicipio([FromBody] MunicipioCLS oMunicipioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oMunicipioCLS.idmunicipio == 0)
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Municipio.Where(p => (p.Nombre.Trim() ).Equals(oMunicipioCLS.nombre.Trim()) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Municipio oMunicipio = new Municipio();
                            oMunicipio.Nombre = oMunicipioCLS.nombre;
                            oMunicipio.Idestado = int.Parse(oMunicipioCLS.idestado);
                            oMunicipio.Habilitado = 1;
                            baseDatos.Municipio.Add(oMunicipio);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Municipio.Where(p => (p.Nombre.Trim()).Equals(oMunicipioCLS.nombre.Trim())
                        && p.Idmunicipio != oMunicipioCLS.idmunicipio && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Municipio oMunicipio = baseDatos.Municipio.Where(p => p.Idmunicipio == oMunicipioCLS.idmunicipio).First();
                            oMunicipio.Nombre = oMunicipioCLS.nombre;
                            oMunicipio.Idestado = int.Parse(oMunicipioCLS.idestado);
                            oMunicipio.Habilitado = 1;
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
        [Route("api/Municipio/RecuperarInformacionMunicipio/{idMunicipio}")]
        public MunicipioCLS RecuperarInformacionMunicipio(int idMunicipio)
        {
            MunicipioCLS oMunicipioCLS = new MunicipioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oMunicipioCLS = (from municipio in baseDatos.Municipio
                               where municipio.Idmunicipio == idMunicipio
                               select new MunicipioCLS
                               {
                                   idmunicipio = municipio.Idmunicipio,
                                   nombre = municipio.Nombre,
                                   idestado = municipio.Idestado.ToString()
                               }).First();

                return oMunicipioCLS;
            }
        }




        [HttpGet]
        [Route("api/Municipio/EliminarMunicipio/{idMunicipio}/{idtorneoseleccionado}")]
        public int EliminarMunicipio(int idMunicipio, string idtorneoseleccionado)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Municipio oMunicipio = baseDatos.Municipio.Where(p => p.Idmunicipio == idMunicipio).First();
                    oMunicipio.Habilitado = 0;
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
