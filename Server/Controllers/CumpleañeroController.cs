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
    public class CumpleañeroController : Controller
    {
        [HttpGet]
        [Route("api/Cumpleañero/ListarCumpleañero/{idtorneoseleccionado}")]
        public List<ProximosCumpleañerosCLS> ListarCumpleañero(string idtorneoseleccionado)
        {
            List<ProximosCumpleañerosCLS> listaCumpleañero = new List<ProximosCumpleañerosCLS>();
            List<ProximosCumpleañerosCLS> listaInicial = new List<ProximosCumpleañerosCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {

                listaInicial = (from jugador in baseDatos.Jugador
                                join equipo in baseDatos.Equipo
                                on jugador.Idequipo equals equipo.Idequipo
                                orderby jugador.Fnacimiento, equipo.Nombre
                                where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                select new ProximosCumpleañerosCLS
                                {
                                    idjugador = jugador.Idjugador,
                                    nombrejugador = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                    nombreequipo = equipo.Nombre,
                                    fnacimiento = (DateTime)jugador.Fnacimiento.Value.AddYears(DateTime.Now.Year - jugador.Fnacimiento.Value.Year),
                                    fnacimientocadena = jugador.Fnacimiento.Value.ToShortDateString(),
                                    años = (DateTime.Now.Year) - jugador.Fnacimiento.Value.Year
                                }).ToList();

                listaCumpleañero = (from jugador in listaInicial
                                    orderby jugador.fnacimiento, jugador.nombreequipo
                                    where jugador.fnacimiento >= DateTime.Now.Date && jugador.fnacimiento < DateTime.Now.Date.AddDays(14)
                                    select new ProximosCumpleañerosCLS
                                    {
                                        idjugador = jugador.idjugador,
                                        nombrejugador = jugador.nombrejugador,
                                        nombreequipo = jugador.nombreequipo,
                                        fnacimientocadena = jugador.fnacimientocadena,
                                        fcumpleañoscadenacorta = regfechacorta(jugador.fnacimiento),
                                        años = jugador.años
                                    }).ToList();
            }
            return listaCumpleañero;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Cumpleañeros/FiltrarCumpleañero/{p_idequipo?}/{idtorneoseleccionado}")]
        public List<ProximosCumpleañerosCLS> FiltrarCumpleañero(string p_idequipo, string idtorneoseleccionado)
        {
            List<ProximosCumpleañerosCLS> listaCumpleañero = new List<ProximosCumpleañerosCLS>();
            List<ProximosCumpleañerosCLS> listaInicial = new List<ProximosCumpleañerosCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idequipo == null || p_idequipo == "--- Seleccione ---")
                {
                    listaInicial = (from jugador in baseDatos.Jugador
                                    join equipo in baseDatos.Equipo
                                    on jugador.Idequipo equals equipo.Idequipo
                                    orderby jugador.Fnacimiento, equipo.Nombre
                                    where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                    select new ProximosCumpleañerosCLS
                                    {
                                        idjugador = jugador.Idjugador,
                                        nombrejugador = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                        nombreequipo = equipo.Nombre,
                                        fnacimiento = (DateTime)jugador.Fnacimiento.Value.AddYears(DateTime.Now.Year - jugador.Fnacimiento.Value.Year),
                                        fnacimientocadena = jugador.Fnacimiento.Value.ToShortDateString(),
                                        años = (DateTime.Now.Year) - jugador.Fnacimiento.Value.Year
                                    }).ToList();

                    listaCumpleañero = (from jugador in listaInicial
                                        orderby jugador.fnacimiento, jugador.nombreequipo
                                        where jugador.fnacimiento >= DateTime.Now.Date && jugador.fnacimiento < DateTime.Now.Date.AddDays(14)
                                        select new ProximosCumpleañerosCLS
                                        {
                                            idjugador = jugador.idjugador,
                                            nombrejugador = jugador.nombrejugador,
                                            nombreequipo = jugador.nombreequipo,
                                            fnacimientocadena = jugador.fnacimientocadena,
                                            fcumpleañoscadenacorta = regfechacorta(jugador.fnacimiento),
                                            años = jugador.años
                                        }).ToList();
                }
                else
                {
                    listaInicial = (from jugador in baseDatos.Jugador
                                    join equipo in baseDatos.Equipo
                                    on jugador.Idequipo equals equipo.Idequipo
                                    orderby jugador.Fnacimiento, equipo.Nombre
                                    where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                   && jugador.Idequipo == int.Parse(p_idequipo)
                                    select new ProximosCumpleañerosCLS
                                    {
                                        idjugador = jugador.Idjugador,
                                        nombrejugador = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                        nombreequipo = equipo.Nombre,
                                        fnacimiento = (DateTime)jugador.Fnacimiento.Value.AddYears(DateTime.Now.Year - jugador.Fnacimiento.Value.Year),
                                        fnacimientocadena = jugador.Fnacimiento.Value.ToShortDateString(),
                                        años = (DateTime.Now.Year) - jugador.Fnacimiento.Value.Year
                                    }).ToList();

                    listaCumpleañero = (from jugador in listaInicial
                                        where jugador.fnacimiento >= DateTime.Now.Date && jugador.fnacimiento < DateTime.Now.Date.AddDays(14)
                                        select new ProximosCumpleañerosCLS
                                        {
                                            idjugador = jugador.idjugador,
                                            nombrejugador = jugador.nombrejugador,
                                            nombreequipo = jugador.nombreequipo,
                                            fnacimientocadena = jugador.fnacimientocadena,
                                            fcumpleañoscadenacorta = regfechacorta(jugador.fnacimiento),
                                            años = jugador.años
                                        }).ToList();
                }
            }
            return listaCumpleañero;
        }


        public static string regfechacorta(DateTime? fnacimiento)
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
            }

            return rfecha;
        }

    }
}


