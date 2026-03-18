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
    public class JuegoInvitadoController : Controller
    {
        [HttpGet]
        [Route("api/JuegoInvitado/ListarJuegoInvitado/{idtorneoseleccionado}")]
        public List<JuegoCLS> ListarJuegoInvitado(string idtorneoseleccionado)
        {

            List<JuegoCLS> listaJuego = new List<JuegoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaJuego = (from juego in baseDatos.Juego
                              join jornada in baseDatos.Jornada
                              on juego.Idjornada equals jornada.Idjornada
                              join equipo01 in baseDatos.Equipo
                              on juego.Idequipo01 equals equipo01.Idequipo
                              join equipo02 in baseDatos.Equipo
                              on juego.Idequipo02 equals equipo02.Idequipo
                              join estatusjuego in baseDatos.Estatusjuego
                              on juego.Idestatusjuego equals estatusjuego.Idestatusjuego
                              orderby juego.Fhorario descending
                              where juego.Habilitado == 1 && juego.Idtorneo == int.Parse(idtorneoseleccionado)
                              select new JuegoCLS
                              {
                                  idjuego = juego.Idjuego,
                                  jornadacadena = jornada.Nombre,
                                  equipo01cadena = equipo01.Nombre,
                                  equipo02cadena = equipo02.Nombre,
                                  fhorariocadena = juego.Fhorario.Value.ToString(),
                                  fhorario = (DateTime)juego.Fhorario,
                                  estatusjuegocadena = estatusjuego.Nombre,
                                  golesequipo01 = (int)juego.Golesequipo01,
                                  golesequipo02 = (int)juego.Golesequipo02,
                                  peequipo01 = (int)juego.Peequipo01,
                                  peequipo02 = (int)juego.Peequipo02
                              }).ToList();
            }
            return listaJuego;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/JuegoInvitado/FiltrarJuegoInvitado/{p_idjornada?}/{idtorneoseleccionado}")]
        public List<JuegoCLS> FiltrarJuego(string p_idjornada, string idtorneoseleccionado)
        {
            List<JuegoCLS> listaJuego = new List<JuegoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idjornada == null || p_idjornada == "--- Seleccione ---")
                {
                    listaJuego = (from juego in baseDatos.Juego
                                  join jornada in baseDatos.Jornada
                                  on juego.Idjornada equals jornada.Idjornada
                                  join equipo01 in baseDatos.Equipo
                                  on juego.Idequipo01 equals equipo01.Idequipo
                                  join equipo02 in baseDatos.Equipo
                                  on juego.Idequipo02 equals equipo02.Idequipo
                                  join estatusjuego in baseDatos.Estatusjuego
                                  on juego.Idestatusjuego equals estatusjuego.Idestatusjuego
                                  orderby juego.Fhorario descending
                                  where juego.Habilitado == 1 && juego.Idtorneo == int.Parse(idtorneoseleccionado)
                                  select new JuegoCLS
                                  {
                                      idjuego = juego.Idjuego,
                                      jornadacadena = jornada.Nombre,
                                      equipo01cadena = equipo01.Nombre,
                                      equipo02cadena = equipo02.Nombre,
                                      fhorariocadena = juego.Fhorario.Value.ToString(),
                                      fhorario = (DateTime)juego.Fhorario,
                                      estatusjuegocadena = estatusjuego.Nombre,
                                      golesequipo01 = (int)juego.Golesequipo01,
                                      golesequipo02 = (int)juego.Golesequipo02,
                                      peequipo01 = (int)juego.Peequipo01
                                  }).ToList();
                }
                else
                {
                    listaJuego = (from juego in baseDatos.Juego
                                  join jornada in baseDatos.Jornada
                                  on juego.Idjornada equals jornada.Idjornada
                                  join equipo01 in baseDatos.Equipo
                                  on juego.Idequipo01 equals equipo01.Idequipo
                                  join equipo02 in baseDatos.Equipo
                                  on juego.Idequipo02 equals equipo02.Idequipo
                                  join estatusjuego in baseDatos.Estatusjuego
                                  on juego.Idestatusjuego equals estatusjuego.Idestatusjuego
                                  orderby juego.Fhorario descending
                                  where juego.Habilitado == 1 && juego.Idjornada == int.Parse(p_idjornada) && juego.Idtorneo == int.Parse(idtorneoseleccionado)
                                  select new JuegoCLS
                                  {
                                      idjuego = juego.Idjuego,
                                      jornadacadena = jornada.Nombre,
                                      equipo01cadena = equipo01.Nombre,
                                      equipo02cadena = equipo02.Nombre,
                                      fhorariocadena = juego.Fhorario.Value.ToString(),
                                      fhorario = (DateTime)juego.Fhorario,
                                      estatusjuegocadena = estatusjuego.Nombre,
                                      golesequipo01 = (int)juego.Golesequipo01

                                  }).ToList();


                }
            }
            return listaJuego;
        }

        [HttpGet]
        [Route("api/JuegoInvitado/RecuperarInformacionJuegoInvitado/{p_idjuego}")]
        public JuegoInvitadoCLS RecuperarInformacionJuego(int p_idjuego)
        {
            JuegoInvitadoCLS oJuegoInvitadoCLS = new JuegoInvitadoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oJuegoInvitadoCLS = (from juego in baseDatos.Juego
                                     join jornada in baseDatos.Jornada
                                     on juego.Idjornada equals jornada.Idjornada
                                     join equipo01 in baseDatos.Equipo
                                     on juego.Idequipo01 equals equipo01.Idequipo
                                     join equipo02 in baseDatos.Equipo
                                     on juego.Idequipo02 equals equipo02.Idequipo
                                     join campo in baseDatos.Campo
                                     on juego.Idcampo equals campo.Idcampo
                                     join arbitro in baseDatos.Arbitro
                                     on juego.Idarbitro equals arbitro.Idarbitro
                                     join estatusjuego in baseDatos.Estatusjuego
                                     on juego.Idestatusjuego equals estatusjuego.Idestatusjuego
                                     where juego.Idjuego == p_idjuego
                                     select new JuegoInvitadoCLS
                                     {
                                         idjuego = juego.Idjuego,
                                         jornadacadena = jornada.Nombre,
                                         equipo01cadena = equipo01.Nombre,
                                         equipo02cadena = equipo02.Nombre,
                                         resequipo01 = juego.Resequipo01,
                                         resequipo02 = juego.Resequipo02,
                                         golesequipo01 = juego.Golesequipo01.ToString(),
                                         golesequipo02 = juego.Golesequipo02.ToString(),
                                         campocadena = campo.Nombre,
                                         arbitrocadena = arbitro.Nombre + " " + arbitro.Appaterno + " " + arbitro.Apmaterno,
                                         fhorariocadena = regfechajuego(juego.Fhorario.Value) + " -- " + juego.Fhorario.Value.ToShortTimeString(),     // juego.Fhorario.Value.ToLongDateString() + " -- " + juego.Fhorario.Value.ToShortTimeString(),
                                         //fhorariocadena = juego.Fhorario.Value.ToLongDateString() + " -- " + juego.Fhorario.Value.ToShortTimeString(),
                                         estatusjuegocadena = estatusjuego.Nombre,
                                         fhorario = (DateTime)juego.Fhorario,
                                         idequipo01 = juego.Idequipo01.ToString(),
                                         idequipo02 = juego.Idequipo02.ToString(),
                                         peequipo01 = (int)juego.Peequipo01,
                                         peequipo02 = (int)juego.Peequipo02

                                     }).First();

                int idequipo01 = int.Parse(oJuegoInvitadoCLS.idequipo01);           // ESTO POSIBLEMENTE YA LO QUTE
                int idequipo02 = int.Parse(oJuegoInvitadoCLS.idequipo02);

                // AQUI PRIMERO HAY QUE IR A LA TABLA GOL PARA TRAER LOS JUGADORES QUE ANOTARON EN ESE JUEGO, AL PRINCIPIO CLARO QUE ESTARA EN BLANCO
                // PERO SI ES UNA CONSULTA POSTERIOR PUEDE QUE HAYA GOLES REGISTRADOS DE JUGADORES QUE YA NO ESTAN EN EL EQUIPO Y NO LOS TRAERIA LA
                // CONSULTA DE ABAJO.
                // SE TIENE QUE VALIDAR QUE LOS JUGADORES QUE SALGAN DE LA CONSULTA DE ARRIBA YA NO LOS REPITA EN LA CONSULTA DE ABAJO.


                List<JugadorGolesCLS> listagoleadorescongoles01 = (from jugadoresgoles in baseDatos.Gol
                                                                   join jugador in baseDatos.Jugador
                                                                   on jugadoresgoles.Idjugador equals jugador.Idjugador
                                                                   where jugadoresgoles.Idjuego == p_idjuego && jugadoresgoles.Idequipo == idequipo01
                                                                   && jugadoresgoles.Habilitado == 1
                                                                   select new JugadorGolesCLS
                                                                   {
                                                                       idjugador = jugador.Idjugador,
                                                                       idequipo = idequipo01,
                                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                                       goles = (int)jugadoresgoles.Goles
                                                                   }).ToList();

                foreach (JugadorGolesCLS jug in listagoleadorescongoles01)
                {
                    oJuegoInvitadoCLS.ListaJugadorGoles01.Add(jug);
                }


                List<JugadorGolesCLS> listagoleadorescongoles02 = (from jugadoresgoles in baseDatos.Gol
                                                                       //join equipo in baseDatos.Equipo
                                                                       //on jugadoresgoles.Idequipo equals equipo.Idequipo
                                                                   join jugador in baseDatos.Jugador
                                                                   on jugadoresgoles.Idjugador equals jugador.Idjugador
                                                                   where jugadoresgoles.Idjuego == p_idjuego && jugadoresgoles.Idequipo == idequipo02
                                                                   && jugadoresgoles.Habilitado == 1
                                                                   select new JugadorGolesCLS
                                                                   {
                                                                       idjugador = jugador.Idjugador,
                                                                       idequipo = idequipo01,
                                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                                       goles = (int)jugadoresgoles.Goles
                                                                   }).ToList();

                foreach (JugadorGolesCLS jug in listagoleadorescongoles02)
                {
                    oJuegoInvitadoCLS.ListaJugadorGoles02.Add(jug);
                }

            }
            return oJuegoInvitadoCLS;
        }


        public static string regfechajuego(DateTime? fjuego)
        {
            string rfecha = "";

            switch (fjuego.Value.DayOfWeek.ToString())
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

            rfecha = rfecha + " " + fjuego.Value.Day.ToString() + " de ";

            switch (fjuego.Value.Month)
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

            if (fjuego.Value.Year >= 2000)
            {
                rfecha = rfecha + " del " + fjuego.Value.Year.ToString();
            }
            else
            {
                rfecha = rfecha + " de " + fjuego.Value.Year.ToString();
            }

            return rfecha;
        }


    }
}


