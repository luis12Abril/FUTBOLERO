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
    public class ComunicadoController : Controller
    {
        [HttpGet]
        [Route("api/Comunicado/ListarComunicado/{idtorneoseleccionado}")]
        public List<ComunicadoCLS> ListarComunicado(string idtorneoseleccionado)
        {
            List<ComunicadoCLS> listaComunicado = new List<ComunicadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaComunicado = (from comunicado in baseDatos.Comunicado
                                   orderby comunicado.Fechacomunicado descending
                                   where comunicado.Habilitado == 1 && comunicado.Idtorneo == int.Parse(idtorneoseleccionado)
                                   select new ComunicadoCLS
                                   {
                                       idcomunicado = comunicado.Idcomunicado,
                                       fechacomunicadocadena = regfechacomunicado(comunicado.Fechacomunicado),      //comunicado.Fechacomunicado.Value.ToLongDateString(),
                                       comunicadocorto = comunicado.Comunicadocorto.Substring(0, 50)

                                   }).ToList();
            }
            return listaComunicado;
        }


        [HttpGet]
        [Route("api/Comunicado/FiltrarComunicado/{mensaje?}/{idtorneoseleccionado}")]
        public List<ComunicadoCLS> FiltrarComunicado(string mensaje, string idtorneoseleccionado)
        {
            List<ComunicadoCLS> listaComunicado = new List<ComunicadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaComunicado = (from comunicado in baseDatos.Comunicado
                                       orderby comunicado.Fechacomunicado
                                       where comunicado.Habilitado == 1 && comunicado.Idtorneo == int.Parse(idtorneoseleccionado)
                                       select new ComunicadoCLS
                                       {
                                           idcomunicado = comunicado.Idcomunicado,
                                           fechacomunicadocadena = regfechacomunicado(comunicado.Fechacomunicado),      //comunicado.Fechacomunicado.Value.ToLongDateString(),
                                           comunicadocorto = comunicado.Comunicadocorto.Substring(0, 50)

                                       }).ToList();
                }
                else
                {
                    listaComunicado = (from comunicado in baseDatos.Comunicado
                                       orderby comunicado.Fechacomunicado
                                       where comunicado.Habilitado == 1 &&
                                       comunicado.Comunicadocorto.Contains(mensaje) && comunicado.Idtorneo == int.Parse(idtorneoseleccionado)
                                       select new ComunicadoCLS
                                       {
                                           idcomunicado = comunicado.Idcomunicado,
                                           fechacomunicadocadena = regfechacomunicado(comunicado.Fechacomunicado),      //comunicado.Fechacomunicado.Value.ToLongDateString(),
                                           comunicadocorto = comunicado.Comunicadocorto.Substring(0, 50)

                                       }).ToList();
                }
            }
            return listaComunicado;
        }




        [HttpPost]
        [Route("api/Comunicado/GuardarDatosComunicado")]
        public int GuardarDatosComunicado([FromBody] ComunicadoCLS oComunicadoCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oComunicadoCLS.idcomunicado == 0)
                    {
                        // VER SI ESTA EN LA TABLA COMUNICADO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Comunicado.Where(p => p.Comunicadocorto.Trim().Equals(oComunicadoCLS.comunicadocorto)
                        && p.Idtorneo == oComunicadoCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            Comunicado oComunicado = new Comunicado();
                            oComunicado.Fechacomunicado = oComunicadoCLS.fechacomunicado;
                            oComunicado.Comunicadocorto = oComunicadoCLS.comunicadocorto;
                            oComunicado.Comunicadolargo = oComunicadoCLS.comunicadolargo;
                            oComunicado.Idtorneo = oComunicadoCLS.idtorneo;

                            //oComunicado.Fechacomunicado = DateTime.Now;
                            //oComunicado.Comunicadocorto = "CORTO";
                            //oComunicado.Comunicadolargo = "LARGO";
                            //oComunicado.Idtorneo = 6;
                            oComunicado.Habilitado = 1;

                            baseDatos.Comunicado.Add(oComunicado);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JORNADA, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Comunicado.Where(p => p.Comunicadocorto.Trim().Equals(oComunicadoCLS.comunicadocorto)
                        && p.Idcomunicado != oComunicadoCLS.idcomunicado && p.Idtorneo == oComunicadoCLS.idtorneo && p.Habilitado == 1).Count();

                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            Comunicado oComunicado = baseDatos.Comunicado.Where(p => p.Idcomunicado == oComunicadoCLS.idcomunicado).First();
                            oComunicado.Fechacomunicado = oComunicadoCLS.fechacomunicado;
                            oComunicado.Comunicadocorto = oComunicadoCLS.comunicadocorto;
                            oComunicado.Comunicadolargo = oComunicadoCLS.comunicadolargo;
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
        [Route("api/Comunicado/RecuperarInformacionComunicado/{idcomunicado}")]
        public ComunicadoCLS RecuperarInformacionComunicado(int idcomunicado)
        {
            ComunicadoCLS oComunicadoCLS = new ComunicadoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oComunicadoCLS = (from comunicado in baseDatos.Comunicado
                                  where comunicado.Idcomunicado == idcomunicado
                                  select new ComunicadoCLS
                                  {
                                      idcomunicado = comunicado.Idcomunicado,
                                      fechacomunicado = (DateTime)comunicado.Fechacomunicado,
                                      comunicadocorto = comunicado.Comunicadocorto,
                                      comunicadolargo = comunicado.Comunicadolargo,
                                      idtorneo = (int)comunicado.Idtorneo
                                  }).First();

                return oComunicadoCLS;
            }
        }


        [HttpGet]
        [Route("api/Comunicado/EliminarComunicado/{idComunicado}")]
        public int EliminarComunicado(int idComunicado)
        {
            int rpta = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Comunicado oComunicado = baseDatos.Comunicado.Where(p => p.Idcomunicado == idComunicado).First();
                    oComunicado.Habilitado = 0;
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

        public static string regfechacomunicado(DateTime? fnacimiento)
        {
            string rfecha = "";

            if (fnacimiento == DateTime.Now.Date)
            {
                rfecha = "HOY";
            }
            else
            {
                switch (fnacimiento.Value.DayOfWeek.ToString())
                {
                    case "Sunday":
                        rfecha = rfecha + "Domingo";
                        break;
                    case "Monday":
                        rfecha = rfecha + "Lunes";
                        break;
                    case "Tuesday":
                        rfecha = rfecha + "Martes";
                        break;
                    case "Wednesday":
                        rfecha = rfecha + "Miercoles";
                        break;
                    case "Thursday":
                        rfecha = rfecha + "Jueves";
                        break;
                    case "Friday":
                        rfecha = rfecha + "Viernes";
                        break;
                    case "Saturday":
                        rfecha = rfecha + "Sabado";
                        break;

                    default:
                        break;
                }

                rfecha = rfecha + " " + fnacimiento.Value.Day.ToString() + " de ";

                switch (fnacimiento.Value.Month)
                {
                    case 1:
                        rfecha = rfecha + "enero";
                        break;
                    case 2:
                        rfecha = rfecha + "febrero";
                        break;
                    case 3:
                        rfecha = rfecha + "marzo";
                        break;
                    case 4:
                        rfecha = rfecha + "abril";
                        break;
                    case 5:
                        rfecha = rfecha + "mayo";
                        break;
                    case 6:
                        rfecha = rfecha + "junio";
                        break;
                    case 7:
                        rfecha = rfecha + "julio";
                        break;
                    case 8:
                        rfecha = rfecha + "agosto";
                        break;
                    case 9:
                        rfecha = rfecha + "septiembre";
                        break;
                    case 10:
                        rfecha = rfecha + "octubre";
                        break;
                    case 11:
                        rfecha = rfecha + "noviembre";
                        break;
                    case 12:
                        rfecha = rfecha + "diciembre";
                        break;
                    default:
                        break;
                }

                if (fnacimiento.Value.Year >= 2000)
                {
                    rfecha = rfecha + " del " + fnacimiento.Value.Year.ToString();
                }
                else
                {
                    rfecha = rfecha + " de " + fnacimiento.Value.Year.ToString();
                }

            }

            return rfecha;
        }

    }
}


