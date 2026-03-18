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
    public class JornadaController : Controller
    {
        [HttpGet]
        [Route("api/Jornada/ListarJornada/{idtorneoseleccionado}")]
        public List<JornadaCLS> ListarJornada(string idtorneoseleccionado)
        {
            List<JornadaCLS> listaJornada = new List<JornadaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaJornada = (from Jornada in baseDatos.Jornada
                                orderby Jornada.Finiciojornada descending
                                where Jornada.Habilitado == 1 && Jornada.Idtorneo == int.Parse(idtorneoseleccionado)
                                select new JornadaCLS
                                {
                                    idjornada = Jornada.Idjornada,
                                    nombre = Jornada.Nombre,
                                    fjornada = regfechajornada(Jornada.Finiciojornada)      //Jornada.Finiciojornada.Value.ToLongDateString()
                                }).ToList();
            }
            return listaJornada;
        }


        [HttpGet]
        [Route("api/Jornada/FiltrarJornada/{mensaje?}/{idtorneoseleccionado}")]
        public List<JornadaCLS> FiltrarJornada(string mensaje, string idtorneoseleccionado)
        {
            List<JornadaCLS> listaJornada = new List<JornadaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaJornada = (from Jornada in baseDatos.Jornada
                                    orderby Jornada.Finiciojornada descending
                                    where Jornada.Habilitado == 1 && Jornada.Idtorneo == int.Parse(idtorneoseleccionado)
                                    select new JornadaCLS
                                    {
                                        idjornada = Jornada.Idjornada,
                                        nombre = Jornada.Nombre,
                                        fjornada = regfechajornada(Jornada.Finiciojornada)      //Jornada.Finiciojornada.Value.ToLongDateString()
                                    }).ToList();
                }
                else
                {
                    listaJornada = (from Jornada in baseDatos.Jornada
                                    orderby Jornada.Finiciojornada descending
                                    where Jornada.Habilitado == 1
                                    && Jornada.Nombre.Contains(mensaje) && Jornada.Idtorneo == int.Parse(idtorneoseleccionado)
                                    select new JornadaCLS
                                    {
                                        idjornada = Jornada.Idjornada,
                                        nombre = Jornada.Nombre,
                                        fjornada = regfechajornada(Jornada.Finiciojornada)      //Jornada.Finiciojornada.Value.ToLongDateString()
                                    }).ToList();
                }
            }
            return listaJornada;
        }


        [HttpPost]
        [Route("api/Jornada/GuardarDatosJornada")]
        public int GuardarDatosJornada([FromBody] JornadaCLS oJornadaCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oJornadaCLS.idjornada == 0)
                    {
                        // VER SI ESTA EN LA TABLA JORNADA, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Jornada.Where(p => p.Nombre.Trim().Equals(oJornadaCLS.nombre) && p.Idtorneo == oJornadaCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            Jornada oJornada = new Jornada();
                            oJornada.Nombre = oJornadaCLS.nombre;
                            oJornada.Finiciojornada = oJornadaCLS.finiciojornada;
                            oJornada.Torneo = "";       // NO LO VOY A USAR
                            oJornada.Idtorneo = oJornadaCLS.idtorneo;
                            oJornada.Habilitado = 1;
                            baseDatos.Jornada.Add(oJornada);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JORNADA, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Jornada.Where(p => p.Nombre.Trim().Equals(oJornadaCLS.nombre) && p.Idjornada != oJornadaCLS.idjornada
                        && p.Idtorneo == oJornadaCLS.idtorneo && p.Habilitado == 1).Count();

                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            Jornada oJornada = baseDatos.Jornada.Where(p => p.Idjornada == oJornadaCLS.idjornada).First();
                            oJornada.Nombre = oJornadaCLS.nombre;
                            oJornada.Finiciojornada = oJornadaCLS.finiciojornada;      //ASI PORQUE NO ES REQUERIDO
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
        [Route("api/Jornada/RecuperarInformacionJornada/{idJornada}")]
        public JornadaCLS RecuperarInformacionJornada(int idJornada)
        {
            JornadaCLS oJornadaCLS = new JornadaCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oJornadaCLS = (from jornada in baseDatos.Jornada
                               where jornada.Idjornada == idJornada
                               select new JornadaCLS
                               {
                                   idjornada = jornada.Idjornada,
                                   nombre = jornada.Nombre,
                                   finiciojornada = (DateTime)jornada.Finiciojornada
                               }).First();

                return oJornadaCLS;
            }
        }


        [HttpGet]
        [Route("api/Jornada/EliminarJornada/{idJornada}")]
        public int EliminarJornada(int idJornada)
        {
            int rpta = 0;
            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Jornada oJornada = baseDatos.Jornada.Where(p => p.Idjornada == idJornada).First();

                    nveces = baseDatos.Juego.Where(p => p.Idjornada == idJornada && p.Habilitado == 1).Count();
                    if (nveces > 0)
                    {
                        rpta = 2;
                    }
                    else
                    {
                        oJornada.Habilitado = 0;
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


        public static string regfechajornada(DateTime? fnacimiento)
        {
            string rfecha = "";

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
          

            return rfecha;
        }


    }
}


