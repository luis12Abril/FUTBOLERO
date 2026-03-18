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
    public class EstadoController : Controller
    {
        [HttpGet]
        [Route("api/Estado/ListarEstado")]
        public List<EstadoCLS> ListarEstado()
        {
            List<EstadoCLS> listaEstado = new List<EstadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEstado = (from estado in baseDatos.Estado
                              orderby estado.Nombre
                              where estado.Habilitado == 1 
                              select new EstadoCLS
                              {
                                  idestado = estado.Idestado,
                                  nombre = estado.Nombre,
                              }).ToList();
            }
            return listaEstado;
        }


        [HttpGet]
        [Route("api/Estado/FiltrarEstado/{mensaje?}/{idtorneoseleccionado}")]
        public List<EstadoCLS> FiltrarEstado(string mensaje, string idtorneoseleccionado)
        {
            List<EstadoCLS> listaEstado = new List<EstadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaEstado = (from estado in baseDatos.Estado
                                  orderby estado.Nombre
                                  where estado.Habilitado == 1
                                  select new EstadoCLS
                                  {
                                      idestado = estado.Idestado,
                                      nombre = estado.Nombre,
                                  }).ToList();
                }
                else
                {
                    listaEstado = (from estado in baseDatos.Estado
                                  orderby estado.Nombre
                                  where estado.Habilitado == 1 && estado.Nombre.Contains(mensaje) 
                                  select new EstadoCLS
                                  {
                                      idestado = estado.Idestado,
                                      nombre = estado.Nombre,
                                  }).ToList();
                }
            }
            return listaEstado;
        }


        [HttpPost]
        [Route("api/Estado/GuardarDatosEstado")]
        public int GuardarDatosEstado([FromBody] EstadoCLS oEstadoCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oEstadoCLS.idestado == 0)
                    {
                        // VER SI ESTA EN LA TABLA ESTADO, ESE NOMBRE DE CAMPO, Y QUE ESTE HABILITADO
                        nveces = baseDatos.Estado.Where(p => p.Nombre.Trim().Equals(oEstadoCLS.nombre) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Estado oEstado = new Estado();
                            oEstado.Nombre = oEstadoCLS.nombre;
                            oEstado.Habilitado = 1;
                            baseDatos.Estado.Add(oEstado);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA ESTADO, ESE NOMBRE DE CAMPO, QUE ESTE HABILITADO
                        nveces = baseDatos.Estado.Where(p => p.Nombre.Trim().Equals(oEstadoCLS.nombre) && p.Idestado != oEstadoCLS.idestado
                        && p.Habilitado == 1).Count();

                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Estado oEstado = baseDatos.Estado.Where(p => p.Idestado == oEstadoCLS.idestado).First();
                            oEstado.Nombre = oEstadoCLS.nombre;
                            oEstado.Habilitado = 1;
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
        [Route("api/Estado/RecuperarInformacionEstado/{idestado}")]
        public EstadoCLS RecuperarInformacionEstado(int idestado)
        {
            EstadoCLS oEstadoCLS = new EstadoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oEstadoCLS = (from estado in baseDatos.Estado
                             where estado.Idestado == idestado
                             select new EstadoCLS
                             {
                                 idestado = estado.Idestado,
                                 nombre = estado.Nombre,
                             }).First();

                return oEstadoCLS;
            }
        }


        [HttpGet]
        [Route("api/Estado/EliminarEstado/{idestado}")]
        public int EliminarEstado(int idestado)
        {
            int rpta = 0;

            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Estado oEstado = baseDatos.Estado.Where(p => p.Idestado == idestado).First();
                    oEstado.Habilitado = 0;
                    baseDatos.SaveChanges();
                    rpta = 1;

                    // PARA EL BORRADO LO VOY A VALIDAR CON LA LIGA, PERO TODAVIA NO
                    //nveces = baseDatos.Juego.Where(p => p.Idcampo == idcampo && p.Habilitado == 1).Count();
                    //    if (nveces > 0)
                    //    {
                    //        rpta = 2;
                    //    }
                    //    else
                    //    {
                    //        oCampo.Habilitado = 0;
                    //        baseDatos.SaveChanges();
                    //        rpta = 1;
                    //    }
                    
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
