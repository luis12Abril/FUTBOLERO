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
    public class JuegoController : Controller
    {
        [HttpGet]
        [Route("api/Juego/ListarJuego/{idtorneoseleccionado}")]
        public List<JuegoCLS> ListarJuego(string idtorneoseleccionado)
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
                                  golesequipo02 = (int)juego.Golesequipo02
                              }).ToList();
            }
            return listaJuego;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Juego/FiltrarJuego/{p_idjornada?}/{idtorneoseleccionado}")]
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
                                      golesequipo02 = (int)juego.Golesequipo02
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
                                      golesequipo01 = (int)juego.Golesequipo01,
                                      golesequipo02 = (int)juego.Golesequipo02
                                  }).ToList();
                }
            }
            return listaJuego;
        }


        [HttpGet]
        [Route("api/Juego/EliminarJuego/{p_idjuego}")]
        public int EliminarJuego(int p_idjuego)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        Juego oJuego = baseDatos.Juego.Where(p => p.Idjuego == p_idjuego).First();
                        int equipo01 = (int)oJuego.Idequipo01;
                        int equipo02 = (int)oJuego.Idequipo02;
                        int idtorneo = (int)oJuego.Idtorneo;
                        oJuego.Habilitado = 0;
                        baseDatos.SaveChanges();
                        //rpta = 1;

                        // SACO LA LISTA DE GOLES EN ESE JUEGO PARA DARLOS DE BAJA, AQUI VAN LOS DOS EQUIPOS
                        List<Gol> listaGoles01 = (from gol in baseDatos.Gol
                                                  where gol.Idjuego == p_idjuego
                                                  && gol.Habilitado == 1
                                                  select gol).ToList();

                        if (listaGoles01 != null && listaGoles01.Count > 0)
                        {
                            foreach (Gol oGol in listaGoles01)
                            {
                                // AQUI BORRO LOS GOLES DE LOS JUGADORES ANOTARON EN EL JUEGO
                                oGol.Habilitado = 0;
                                baseDatos.SaveChanges();

                                // AQUI LE RESTO LOS GOLES QUE TENIAN LOS JUGADORES EN ESE JUEGO
                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oGol.Idjugador).First();
                                oJugador.Goles = oJugador.Goles - oGol.Goles;
                                baseDatos.SaveChanges();
                            }
                        }



                        // RECALCULAR LOS DATOS DEL EQUIPO01
                        Equipo oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == equipo01).First();

                        int idestatusjugado = baseDatos.Estatusjuego.Where(p => p.Nombre.Trim().Equals("JUGADO") && p.Idtorneo == idtorneo).First().Idestatusjuego;

                        List<Juego> oJuego01 = baseDatos.Juego.Where(p => p.Idequipo01 == equipo01 && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();
                        List<Juego> oJuego02 = baseDatos.Juego.Where(p => p.Idequipo02 == equipo01 && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();

                        //List<Juego> oJuego01 = baseDatos.Juego.Where(p => p.Idequipo01 == equipo01 && p.Idestatusjuego == 2 && p.Habilitado == 1).ToList();
                        //List<Juego> oJuego02 = baseDatos.Juego.Where(p => p.Idequipo02 == equipo01 && p.Idestatusjuego == 2 && p.Habilitado == 1).ToList();

                        int sumagolesfavor = (int)oJuego01.Sum(x => x.Golesequipo01) + (int)oJuego02.Sum(x => x.Golesequipo02);
                        int sumagolescontra = (int)oJuego01.Sum(x => x.Golesequipo02) + (int)oJuego02.Sum(x => x.Golesequipo01);
                        int diferenciagoles = sumagolesfavor - sumagolescontra;
                        int jugados = oJuego01.Count() + oJuego02.Count();
                        int ganados = oJuego01.Where(p => p.Resequipo01 == "G").Count() + oJuego02.Where(p => p.Resequipo02 == "G").Count();
                        int empatados = oJuego01.Where(p => p.Resequipo01 == "E").Count() + oJuego02.Where(p => p.Resequipo02 == "E").Count();
                        int perdidos = oJuego01.Where(p => p.Resequipo01 == "P").Count() + oJuego02.Where(p => p.Resequipo02 == "P").Count();
                        int ganadospenales = oJuego01.Where(p => p.Resequipo01 == "GP").Count() + oJuego02.Where(p => p.Resequipo02 == "GP").Count();
                        int ganadosadmo = oJuego01.Where(p => p.Resequipo01 == "GA").Count() + oJuego02.Where(p => p.Resequipo02 == "GA").Count();
                        int totalganados = ganados + ganadosadmo;
                        int puntos = (int)oJuego01.Sum(x => x.Puntosequipo01) + (int)oJuego02.Sum(x => x.Puntosequipo02);

                        oEquipo.Jugados = jugados;
                        oEquipo.Ganados = totalganados;
                        oEquipo.Empatados = empatados;
                        oEquipo.Perdidos = perdidos;
                        oEquipo.Empatadosganados = ganadospenales;
                        oEquipo.Golesafavor = sumagolesfavor;
                        oEquipo.Golesencontra = sumagolescontra;
                        oEquipo.Difgoles = diferenciagoles;
                        oEquipo.Puntos = puntos;
                        baseDatos.SaveChanges();


                        // RECALCULAR LOS DATOS DEL EQUIPO02
                        oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == equipo02).First();

                        idestatusjugado = baseDatos.Estatusjuego.Where(p => p.Nombre.Trim().Equals("JUGADO") && p.Idtorneo == idtorneo).First().Idestatusjuego;

                        oJuego01 = baseDatos.Juego.Where(p => p.Idequipo01 == equipo02 && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();
                        oJuego02 = baseDatos.Juego.Where(p => p.Idequipo02 == equipo02 && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();

                        //oJuego01 = baseDatos.Juego.Where(p => p.Idequipo01 == equipo02 && p.Idestatusjuego == 2 && p.Habilitado == 1).ToList();
                        //oJuego02 = baseDatos.Juego.Where(p => p.Idequipo02 == equipo02 && p.Idestatusjuego == 2 && p.Habilitado == 1).ToList();

                        sumagolesfavor = (int)oJuego01.Sum(x => x.Golesequipo01) + (int)oJuego02.Sum(x => x.Golesequipo02);
                        sumagolescontra = (int)oJuego01.Sum(x => x.Golesequipo02) + (int)oJuego02.Sum(x => x.Golesequipo01);
                        diferenciagoles = sumagolesfavor - sumagolescontra;
                        jugados = oJuego01.Count() + oJuego02.Count();
                        ganados = oJuego01.Where(p => p.Resequipo01 == "G").Count() + oJuego02.Where(p => p.Resequipo02 == "G").Count();
                        empatados = oJuego01.Where(p => p.Resequipo01 == "E").Count() + oJuego02.Where(p => p.Resequipo02 == "E").Count();
                        perdidos = oJuego01.Where(p => p.Resequipo01 == "P").Count() + oJuego02.Where(p => p.Resequipo02 == "P").Count();
                        ganadospenales = oJuego01.Where(p => p.Resequipo01 == "GP").Count() + oJuego02.Where(p => p.Resequipo02 == "GP").Count();
                        ganadosadmo = oJuego01.Where(p => p.Resequipo01 == "GA").Count() + oJuego02.Where(p => p.Resequipo02 == "GA").Count();
                        totalganados = ganados + ganadosadmo;
                        puntos = (int)oJuego01.Sum(x => x.Puntosequipo01) + (int)oJuego02.Sum(x => x.Puntosequipo02);

                        oEquipo.Jugados = jugados;
                        oEquipo.Ganados = totalganados;
                        oEquipo.Empatados = empatados;
                        oEquipo.Perdidos = perdidos;
                        oEquipo.Empatadosganados = ganadospenales;
                        oEquipo.Golesafavor = sumagolesfavor;
                        oEquipo.Golesencontra = sumagolescontra;
                        oEquipo.Difgoles = diferenciagoles;
                        oEquipo.Puntos = puntos;

                        baseDatos.SaveChanges();

                        // BORRO LOS COMENTARIOS DE ESE JUEGO
                        List<Comentario> oComentario = baseDatos.Comentario.Where(p => p.Idjuego == p_idjuego).ToList();
                        foreach (Comentario item in oComentario)
                        {
                            item.Habilitado = 0;
                            baseDatos.SaveChanges();
                        }

                        transaccion.Complete();
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

        [HttpPost]
        [Route("api/Juego/GuardarDatosJuego/")]
        public int GuardarDatosJuego([FromBody] JuegoCLS oJuegoCLS)
        {

            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oJuegoCLS.idjuego == 0)     // CUANDO ES UN JUEGO NUEVO
                        {
                            int idestatuspendiente = baseDatos.Estatusjuego.Where(p => p.Nombre.Contains("PENDIENTE") && p.Idtorneo == oJuegoCLS.idtorneo).First().Idestatusjuego;
                            Juego oJuego = new Juego();
                            oJuego.Idjornada = int.Parse(oJuegoCLS.idjornada);
                            oJuego.Idequipo01 = int.Parse(oJuegoCLS.idequipo01);
                            oJuego.Idequipo02 = int.Parse(oJuegoCLS.idequipo02);
                            oJuego.Idcampo = int.Parse(oJuegoCLS.idcampo);
                            oJuego.Idarbitro = int.Parse(oJuegoCLS.idarbitro);
                            oJuego.Idestatusjuego = idestatuspendiente;  // int.Parse(oJuegoCLS.idestatusjuego);  
                            oJuego.Fhorario = oJuegoCLS.fhorario;
                            oJuego.Golesequipo01 = 0;
                            oJuego.Golesequipo02 = 0;
                            oJuego.Resequipo01 = "E";
                            oJuego.Resequipo02 = "E";
                            oJuego.Puntosequipo01 = 1;
                            oJuego.Puntosequipo02 = 1;
                            oJuego.Torneo = "";       // NO LO VOY A USAR
                            oJuego.Idtorneo = oJuegoCLS.idtorneo;
                            oJuego.Habilitado = 1;
                            oJuego.Cuentaparapuntos = Convert.ToInt32(oJuegoCLS.cuentaparapuntos);
                            oJuego.Cuentaparagoles = Convert.ToInt32(oJuegoCLS.cuentaparagoles);

                            oJuego.Peequipo01 = 0;
                            oJuego.Peequipo02 = 0;

                            baseDatos.Juego.Add(oJuego);
                            baseDatos.SaveChanges();

                            transaccion.Complete();
                            rpta = 1;
                        }
                        else
                        {
                            int ngoles01 = oJuegoCLS.ListaJugadorGoles01.Sum(x => x.goles);
                            int ngoles02 = oJuegoCLS.ListaJugadorGoles02.Sum(x => x.goles);

                            if (rpta == 0)
                            {
                                rpta = ValidaFinal(oJuegoCLS.resequipo01, oJuegoCLS.resequipo02, oJuegoCLS.golesequipo01, oJuegoCLS.golesequipo02);
                            }

                            string estatus = baseDatos.Estatusjuego.Where(p => p.Idestatusjuego == int.Parse(oJuegoCLS.idestatusjuego)).First().Nombre;
                            bool juegojugado = false;
                            if (estatus.Contains("JUGADO"))
                            {
                                juegojugado = true;
                            }

                            // SOLO CON EL ESTATUS DEL JUEGO JUGADO Y QUE SE CUMPLA CON ALGUNA DE LAS PRIMERAS TRES CONSICIONES ENTRARA AL IF
                            if (((ngoles01 != oJuegoCLS.golesequipo01) || (ngoles02 != oJuegoCLS.golesequipo02) || (rpta >= 10)) && juegojugado)
                            {
                                if (ngoles01 != oJuegoCLS.golesequipo01)
                                {
                                    rpta = 4;           // LOS GOLES DEL EQUIPO 01 NO SON IGUALES
                                }
                                if (ngoles02 != oJuegoCLS.golesequipo02)
                                {
                                    rpta = 5;            // LOS GOLES DEL EQUIPO 02 NO SON IGUALES
                                }
                                if (ngoles01 != oJuegoCLS.golesequipo01 && ngoles02 != oJuegoCLS.golesequipo02)
                                {
                                    rpta = 3;           //  // LOS GOLES DEL EQUIPO 01 Y 02 NO SON IGUALES
                                }

                            }
                            else
                            {
                                // OBTENGO TODOS LOS DATOS DEL JUEGO
                                Juego oJuego = baseDatos.Juego.Where(p => p.Idjuego == oJuegoCLS.idjuego).First();

                                // LE ASIGNO LOS PUNTOS OBTENIDOS DEL EQUIPO EN BASE A LO SELECCIONADO
                                switch (oJuegoCLS.resequipo01)
                                {
                                    case "G":
                                        oJuegoCLS.puntosequipo01 = 3;
                                        break;
                                    case "E":
                                        oJuegoCLS.puntosequipo01 = 1;
                                        break;
                                    case "P":
                                        oJuegoCLS.puntosequipo01 = 0;
                                        break;
                                    case "GP":
                                        oJuegoCLS.puntosequipo01 = 2;
                                        break;
                                    case "GA":
                                        oJuegoCLS.puntosequipo01 = 3;
                                        break;
                                    default:
                                        break;
                                }

                                switch (oJuegoCLS.resequipo02)
                                {
                                    case "G":
                                        oJuegoCLS.puntosequipo02 = 3;
                                        break;
                                    case "E":
                                        oJuegoCLS.puntosequipo02 = 1;
                                        break;
                                    case "P":
                                        oJuegoCLS.puntosequipo02 = 0;
                                        break;
                                    case "GP":
                                        oJuegoCLS.puntosequipo02 = 2;
                                        break;
                                    case "GA":
                                        oJuegoCLS.puntosequipo02 = 3;
                                        break;
                                    default:
                                        break;
                                }

                                // ESTO ES PARA LAS VALIDACIONES DE CUANDO CAMBIO EL CHECK DE CUENTA PARA GOLES
                                int opciongoles;
                                if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == (int)oJuego.Cuentaparagoles)
                                {
                                    opciongoles = 0;        // NADA CAMBIA
                                }
                                else
                                {
                                    if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == 0)
                                    {
                                        opciongoles = 1;        // CAMBIA DE, SI CUENTAN LOS GOLES A NO CUENTAN LOS GOLES
                                    }
                                    else
                                    {
                                        opciongoles = 2;        // CAMBIA DE, NO CUENTAN LOS GOLES A SI CUENTAN LOS GOLES
                                    }
                                }

                                // ESTA VARIABLE PARECE QUE NO LA USO, PERO NO LA VOY A BORRAR TODAVIA
                                int cuentaparagolesoriginal = (int)oJuego.Cuentaparagoles;

                                // SE LLENA EL OBJETO oJuego CON LOS DATOS DE PANTALLA JUEGO
                                oJuego.Idjornada = int.Parse(oJuegoCLS.idjornada);
                                oJuego.Idequipo01 = int.Parse(oJuegoCLS.idequipo01);
                                oJuego.Idequipo02 = int.Parse(oJuegoCLS.idequipo02);
                                oJuego.Idcampo = int.Parse(oJuegoCLS.idcampo);
                                oJuego.Idarbitro = int.Parse(oJuegoCLS.idarbitro);
                                oJuego.Idestatusjuego = int.Parse(oJuegoCLS.idestatusjuego);
                                oJuego.Fhorario = oJuegoCLS.fhorario;
                                oJuego.Golesequipo01 = oJuegoCLS.golesequipo01;
                                oJuego.Golesequipo02 = oJuegoCLS.golesequipo02;
                                oJuego.Resequipo01 = oJuegoCLS.resequipo01;
                                oJuego.Resequipo02 = oJuegoCLS.resequipo02;
                                oJuego.Puntosequipo01 = oJuegoCLS.puntosequipo01;
                                oJuego.Puntosequipo02 = oJuegoCLS.puntosequipo02;

                                oJuego.Cuentaparapuntos = Convert.ToInt32(oJuegoCLS.cuentaparapuntos);
                                oJuego.Cuentaparagoles = Convert.ToInt32(oJuegoCLS.cuentaparagoles);

                                oJuego.Peequipo01 = oJuegoCLS.peequipo01;
                                oJuego.Peequipo02 = oJuegoCLS.peequipo02;

                                baseDatos.SaveChanges();


                                // AQUI ES UNA LISTA DE LOS GOLES QUE ANOTO EL EQUIPO01 EN ESE JUEGO, LOS QUE TENIA ORIGINALMENTE ANTES DE EDITAR
                                List<Gol> listaGoles01 = (from gol in baseDatos.Gol
                                                          where gol.Idjuego == oJuego.Idjuego && gol.Idequipo == int.Parse(oJuegoCLS.idequipo01)
                                                          && gol.Habilitado == 1
                                                          select gol).ToList();

                                // CHECO SI LA LISTA NO ESTA VACIA O EL EQUIPO01 SI TIENE GOLES REGISTRADOS EN EL JUEGO
                                if (listaGoles01 != null && listaGoles01.Count > 0)
                                {
                                    // RECORRO LA LISTA
                                    foreach (Gol oGol in listaGoles01)
                                    {
                                        // AQUI BORRO LOS GOLES DE LOS JUGADORES QUE YA ESTABAN EN EL JUEGO, SE ESTAN DESHABILITANDO DE LA TABLA GOL
                                        // CHECA ESTE PASO PORQUE SE ME ESTAN QUEDANDO MUCHOS REGISTROS CON HABILITADO = 0, AL FINAL CUADO TODO FUNCIONES
                                        // VOY A BUSCAR UNA FORMA DE UTILIZAR ESTOS REGISTROS
                                        oGol.Habilitado = 0;
                                        baseDatos.SaveChanges();

                                        // AQUI LE QUITO LOS GOLES QUE TENIAN LOS JUGADORES EN ESE JUEGO, SE LOS RESTO DE LA TABLA JUGADOR, AHI LLEVO EL NUMERO DE GOLA ANOTADOS
                                        if (opciongoles == 0)       // 0 ES QUE NO CAMBIO LA OPCION
                                        {
                                            if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == 1)        // ES QUE EL JUEGO CUENTA PARA GOLES PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oGol.Idjugador).First();
                                                oJugador.Goles = oJugador.Goles - oGol.Goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            if (opciongoles == 1)        // CAMBIO DE, SI CUENTAN A NO CUENTAN LOS GOLES DEL JUEGO PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oGol.Idjugador).First();
                                                oJugador.Goles = oJugador.Goles - oGol.Goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }

                                    }
                                }

                                // SUMO LOS GOLES EN CASO DE QUE EL PARTIDO TENGA ESTATUS DE JUGADO
                                // SI ESTA JUGADO, PENDIENTE O CUENTA O NO PARA GOLES, SE LOS AGREGO IGUA A LA TABLA GOL, PORQUE LOS QUE TENIA ESE JUEGO YA LOS BORRE

                                int nrepetido = 0;
                                foreach (JugadorGolesCLS oJugadorGolesCLS in oJuegoCLS.ListaJugadorGoles01)
                                {
                                    Gol oRegistroGolJugadorEquipo01 = new Gol();
                                    if (oJugadorGolesCLS.goles > 0)
                                    {
                                        Gol oGoL01 = new Gol();

                                        // AQUI EN VEZ DE ADD DEBO BUSCAR SI ESTE JUGADOR ESTA EN LA TABLA IDJUEGO Y IDJUGADOR Y USAR ESE REGISTRO, ACTALIZARLO
                                        // Y PONERLO HABILITADO = 1, ESTO ES PARA VOLVER A UTILIZAR LOS REGISTROS QUE ARRIBA PUSE HABILITADO = 0

                                        nrepetido = baseDatos.Gol.Where(p => p.Idjuego == oJuegoCLS.idjuego
                                        && p.Idequipo == int.Parse(oJuegoCLS.idequipo01) && p.Idjugador == oJugadorGolesCLS.idjugador).Count();

                                        // SI ES MAYOR A 0 ES QUE LO ENCONTRO EN LA TABLA GOL
                                        if (nrepetido > 0)
                                        {
                                            oRegistroGolJugadorEquipo01 = baseDatos.Gol.Where(p => p.Idjuego == oJuegoCLS.idjuego
                                            && p.Idequipo == int.Parse(oJuegoCLS.idequipo01) && p.Idjugador == oJugadorGolesCLS.idjugador).First();

                                            oRegistroGolJugadorEquipo01.Goles = oJugadorGolesCLS.goles;
                                            oRegistroGolJugadorEquipo01.Habilitado = 1;
                                        }
                                        else
                                        {
                                            // SI NO LO ENCONTRO ENTONCES LO AGREGO A LA TABLA GOL
                                            oGoL01.Idjuego = oJuego.Idjuego;
                                            oGoL01.Idequipo = oJugadorGolesCLS.idequipo;
                                            oGoL01.Idjugador = oJugadorGolesCLS.idjugador;
                                            oGoL01.Goles = oJugadorGolesCLS.goles;
                                            oGoL01.Habilitado = 1;
                                            baseDatos.Gol.Add(oGoL01);
                                        }

                                        baseDatos.SaveChanges();

                                        // AQUI LE PONGO LOS GOLES QUE SE INDICARON EN EL FORMULARIO DE EN ESE JUEGO, EN LA TABLA JUGADOR
                                        if (opciongoles == 0)                // 0 ES QUE NO CAMBIO LA OPCION
                                        {
                                            if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == 1)            // ES QUE EL JUEGO CUENTA PARA GOLES PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oJugadorGolesCLS.idjugador).First();
                                                oJugador.Goles = oJugador.Goles + oJugadorGolesCLS.goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            if (opciongoles == 2)                 // CAMBIO DE, NO CUENTAN A SI CUENTAN LOS GOLES DEL JUEGO PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oJugadorGolesCLS.idjugador).First();
                                                oJugador.Goles = oJugador.Goles + oJugadorGolesCLS.goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }
                                    }
                                }

                                // LO MISMO QUE SE ANALIZO ARRIBA CON EQUIPO01

                                // AQUI ES UNA LISTA DE LOS GOLES QUE ANOTO EL EQUIPO02 EN ESE JUEGO, LOS QUE TENIA ORIGINALMENTE ANTES DE EDITAR
                                List<Gol> listaGoles02 = (from gol in baseDatos.Gol
                                                          where gol.Idjuego == oJuego.Idjuego && gol.Idequipo == int.Parse(oJuegoCLS.idequipo02)
                                                          && gol.Habilitado == 1
                                                          select gol).ToList();

                                if (listaGoles02 != null && listaGoles02.Count > 0)
                                {
                                    foreach (Gol oGol in listaGoles02)
                                    {
                                        // AQUI BORRAMOS LOS GOLES DE LOS JUGADORES QUE YA ESTABAN EN EL JUEGO
                                        oGol.Habilitado = 0;
                                        baseDatos.SaveChanges();

                                        // AQUI LE QUITAMOS LOS GOLES QUE TENIAN LOS JUGADORES EN ESE JUEGO
                                        if (opciongoles == 0)       // 0 ES QUE NO CAMBIO LA OPCION
                                        {
                                            if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == 1)        // ES QUE EL JUEGO CUENTA PARA GOLES PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oGol.Idjugador).First();
                                                oJugador.Goles = oJugador.Goles - oGol.Goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            if (opciongoles == 1)        // CAMBIO DE SI CUENTAN A NO CUENTAN LOS GOLES DEL JUEGO PARA LA TABLA DE GOLEO
                                            {
                                                Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oGol.Idjugador).First();
                                                oJugador.Goles = oJugador.Goles - oGol.Goles;
                                                baseDatos.SaveChanges();
                                            }
                                        }
                                    }
                                }

                                foreach (JugadorGolesCLS oJugadorGolesCLS in oJuegoCLS.ListaJugadorGoles02)
                                {
                                    Gol oRegistroGolJugadorEquipo02 = new Gol();
                                    if (oJugadorGolesCLS.goles > 0)
                                    {
                                        Gol oGoL02 = new Gol();
                                        nrepetido = baseDatos.Gol.Where(p => p.Idjuego == oJuegoCLS.idjuego
                                        && p.Idequipo == int.Parse(oJuegoCLS.idequipo02) && p.Idjugador == oJugadorGolesCLS.idjugador).Count();

                                        // SI ES MAYOR A 0 ES QUE LO ENCONTRO EN LA TABLA GOL
                                        if (nrepetido > 0)
                                        {
                                            oRegistroGolJugadorEquipo02 = baseDatos.Gol.Where(p => p.Idjuego == oJuegoCLS.idjuego
                                            && p.Idequipo == int.Parse(oJuegoCLS.idequipo02) && p.Idjugador == oJugadorGolesCLS.idjugador).First();

                                            oRegistroGolJugadorEquipo02.Goles = oJugadorGolesCLS.goles;
                                            oRegistroGolJugadorEquipo02.Habilitado = 1;
                                        }
                                        else
                                        {
                                            // SI NO LO ENCONTRO ENTONCES LO AGREGO A LA TABLA GOL
                                            oGoL02.Idjuego = oJuego.Idjuego;
                                            oGoL02.Idequipo = oJugadorGolesCLS.idequipo;
                                            oGoL02.Idjugador = oJugadorGolesCLS.idjugador;
                                            oGoL02.Goles = oJugadorGolesCLS.goles;
                                            oGoL02.Habilitado = 1;
                                            baseDatos.Gol.Add(oGoL02);
                                        }

                                        baseDatos.SaveChanges();

                                    }
                                    // AQUI LE PONEMOS LOS GOLES QUE SE INDICARON EN EL FORMULARIO DE EN ESE JUEGO
                                    if (opciongoles == 0)                // 0 ES QUE NO CAMBIO LA OPCION
                                    {
                                        if (Convert.ToInt32(oJuegoCLS.cuentaparagoles) == 1)            // ES QUE EL JUEGO CUENTA PARA GOLES PARA LA TABLA DE GOLEO
                                        {
                                            Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oJugadorGolesCLS.idjugador).First();
                                            oJugador.Goles = oJugador.Goles + oJugadorGolesCLS.goles;
                                            baseDatos.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        if (opciongoles == 2)                 // CAMBIO DE NO CUENTAN A SI CUENTAN LOS GOLES DEL JUEGO PARA LA TABLA DE GOLEO
                                        {
                                            Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oJugadorGolesCLS.idjugador).First();
                                            oJugador.Goles = oJugador.Goles + oJugadorGolesCLS.goles;
                                            baseDatos.SaveChanges();
                                        }
                                    }
                                }

                                baseDatos.SaveChanges();            // A LO MEJOR ESTE NO ES NECESARIO

                                // ACTUALIZAR EL EQUIPO01 DE LA TABLA EQUIPO, OBTENGO TODOS LOS DATOS DEL EQUIPO01
                                Equipo oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == int.Parse(oJuegoCLS.idequipo01)).First();

                                // AQUI OBTENGO EL ID DE EL ESTADO JUGADO DE TORNEO SELECCIONADO (SOLO PARTIDOS CON EL ESTATUS DE JUGADO)
                                //int idestatusjugado = baseDatos.Estatusjuego.Where(p => p.Nombre.Contains("JUGADO") && p.Torneo == oJuegoCLS.torneo).First().Idestatusjuego;
                                int idestatusjugado = baseDatos.Estatusjuego.Where(p => p.Nombre.Trim().Equals("JUGADO") && p.Idtorneo == oJuegoCLS.idtorneo).First().Idestatusjuego;

                                // AQUI SACO UNA LISTA DE LOS JUEGOS DEL EQUIPO01 DEL TORNEO SELECCIONADO Y QUE TENGA ESTADOS DE JUGADO (QUE ESTE DE LOCAL O DE VISITANTE)
                                List<Juego> oJuegoPuntos01 = baseDatos.Juego.Where(p => p.Idequipo01 == int.Parse(oJuegoCLS.idequipo01) && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();
                                List<Juego> oJuegoPuntos02 = baseDatos.Juego.Where(p => p.Idequipo02 == int.Parse(oJuegoCLS.idequipo01) && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();

                                // AQUI SACO UNA LISTA DE LOS JUEGOS DEL EQUIPO02 DEL TORNEO SELECCIONADO Y QUE TENGA ESTADOS DE JUGADO (QUE ESTE DE LOCAL O DE VISITANTE)
                                List<Juego> oJuegoGoles01 = baseDatos.Juego.Where(p => p.Idequipo01 == int.Parse(oJuegoCLS.idequipo01) && p.Idestatusjuego == idestatusjugado && p.Cuentaparagoles == 1 && p.Habilitado == 1).ToList();
                                List<Juego> oJuegoGoles02 = baseDatos.Juego.Where(p => p.Idequipo02 == int.Parse(oJuegoCLS.idequipo01) && p.Idestatusjuego == idestatusjugado && p.Cuentaparagoles == 1 && p.Habilitado == 1).ToList();

                                // CHECAR SI oJuegoPuntos01 ES IGUAL A oJuegoGoles01 PORQUE A LO MEJOR ESTO REPITIENDO Y PUEDO USAR SOLO UNA DE ELLAS
                                // CHECAR SI oJuegoPuntos02 ES IGUAL A oJuegoGoles02 PORQUE A LO MEJOR ESTO REPITIENDO Y PUEDO USAR SOLO UNA DE ELLAS


                                // AQUI PREPARO LAS VARIABLES QUE VOY ASIGNAR A LA TABLA EQUIPO PARA ACTUALIZARLA
                                int sumagolesfavor = (int)oJuegoGoles01.Sum(x => x.Golesequipo01) + (int)oJuegoGoles02.Sum(x => x.Golesequipo02);
                                int sumagolescontra = (int)oJuegoGoles01.Sum(x => x.Golesequipo02) + (int)oJuegoGoles02.Sum(x => x.Golesequipo01);
                                int diferenciagoles = sumagolesfavor - sumagolescontra;

                                int jugados = oJuegoPuntos01.Count() + oJuegoPuntos02.Count();
                                int ganados = oJuegoPuntos01.Where(p => p.Resequipo01 == "G").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "G").Count();
                                int empatados = oJuegoPuntos01.Where(p => p.Resequipo01 == "E").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "E").Count();
                                int perdidos = oJuegoPuntos01.Where(p => p.Resequipo01 == "P").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "P").Count();
                                int ganadospenales = oJuegoPuntos01.Where(p => p.Resequipo01 == "GP").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "GP").Count();
                                int ganadosadmo = oJuegoPuntos01.Where(p => p.Resequipo01 == "GA").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "GA").Count();
                                int totalganados = ganados + ganadosadmo;
                                int puntos = (int)oJuegoPuntos01.Sum(x => x.Puntosequipo01) + (int)oJuegoPuntos02.Sum(x => x.Puntosequipo02);

                                int peequipo01 = (int)oJuegoPuntos01.Sum(x => x.Peequipo01) + (int)oJuegoPuntos02.Sum(x => x.Peequipo02);
                               
                                oEquipo.Jugados = jugados;
                                oEquipo.Ganados = totalganados;
                                oEquipo.Empatados = empatados;
                                oEquipo.Perdidos = perdidos;
                                oEquipo.Empatadosganados = ganadospenales;
                                oEquipo.Golesafavor = sumagolesfavor;
                                oEquipo.Golesencontra = sumagolescontra;
                                oEquipo.Difgoles = diferenciagoles;
                                oEquipo.Puntos = puntos;

                                oEquipo.Puntosextras = peequipo01;

                                baseDatos.SaveChanges();


                                // LO MISMO DE ARRIBA
                                // // ACTUALIZAR EL EQUIPO02 DE LA TABLA EQUIPO, OBTENGO TODOS LOS DATOS DEL EQUIPO01
                                oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == int.Parse(oJuegoCLS.idequipo02)).First();

                                // AQUI SACO UNA LISTA DE LOS JUEGOS DEL EQUIPO02 DEL TORNEO SELECCIONADO Y QUE TENGA ESTADOS DE JUGADO (QUE ESTE DE LOCAL O DE VISITANTE)
                                oJuegoPuntos01 = baseDatos.Juego.Where(p => p.Idequipo01 == int.Parse(oJuegoCLS.idequipo02) && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();
                                oJuegoPuntos02 = baseDatos.Juego.Where(p => p.Idequipo02 == int.Parse(oJuegoCLS.idequipo02) && p.Idestatusjuego == idestatusjugado && p.Cuentaparapuntos == 1 && p.Habilitado == 1).ToList();

                                oJuegoGoles01 = baseDatos.Juego.Where(p => p.Idequipo01 == int.Parse(oJuegoCLS.idequipo02) && p.Idestatusjuego == idestatusjugado && p.Cuentaparagoles == 1 && p.Habilitado == 1).ToList();
                                oJuegoGoles02 = baseDatos.Juego.Where(p => p.Idequipo02 == int.Parse(oJuegoCLS.idequipo02) && p.Idestatusjuego == idestatusjugado && p.Cuentaparagoles == 1 && p.Habilitado == 1).ToList();


                                sumagolesfavor = (int)oJuegoGoles01.Sum(x => x.Golesequipo01) + (int)oJuegoGoles02.Sum(x => x.Golesequipo02);
                                sumagolescontra = (int)oJuegoGoles01.Sum(x => x.Golesequipo02) + (int)oJuegoGoles02.Sum(x => x.Golesequipo01);
                                diferenciagoles = sumagolesfavor - sumagolescontra;

                                jugados = oJuegoPuntos01.Count() + oJuegoPuntos02.Count();
                                ganados = oJuegoPuntos01.Where(p => p.Resequipo01 == "G").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "G").Count();
                                empatados = oJuegoPuntos01.Where(p => p.Resequipo01 == "E").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "E").Count();
                                perdidos = oJuegoPuntos01.Where(p => p.Resequipo01 == "P").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "P").Count();
                                ganadospenales = oJuegoPuntos01.Where(p => p.Resequipo01 == "GP").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "GP").Count();
                                ganadosadmo = oJuegoPuntos01.Where(p => p.Resequipo01 == "GA").Count() + oJuegoPuntos02.Where(p => p.Resequipo02 == "GA").Count();
                                totalganados = ganados + ganadosadmo;
                                puntos = (int)oJuegoPuntos01.Sum(x => x.Puntosequipo01) + (int)oJuegoPuntos02.Sum(x => x.Puntosequipo02);

                                int peequipo02 = (int)oJuegoPuntos01.Sum(x => x.Peequipo01) + (int)oJuegoPuntos02.Sum(x => x.Peequipo02);


                                oEquipo.Jugados = jugados;
                                oEquipo.Ganados = totalganados;
                                oEquipo.Empatados = empatados;
                                oEquipo.Perdidos = perdidos;
                                oEquipo.Empatadosganados = ganadospenales;
                                oEquipo.Golesafavor = sumagolesfavor;
                                oEquipo.Golesencontra = sumagolescontra;
                                oEquipo.Difgoles = diferenciagoles;
                                oEquipo.Puntos = puntos;

                                oEquipo.Puntosextras = peequipo02;

                                baseDatos.SaveChanges();


                                // COMENTARIOS
                                rpta = 1;

                                bool comentariovacio = false;
                                int nveces = baseDatos.Comentario.Where(p => p.Idjuego == oJuegoCLS.idjuego).Count();
                                if (oJuegoCLS.comentario.Trim() == string.Empty)
                                {
                                    comentariovacio = true;
                                    rpta = 7;
                                }

                                if (nveces > 50)
                                {
                                    rpta = 8;
                                }
                                else if (!comentariovacio)      // SI NO ESTA VACIO GRABA
                                {
                                    Comentario oComentario = new Comentario();
                                    oComentario.Idjuego = oJuegoCLS.idjuego;
                                    oComentario.Comentario1 = oJuegoCLS.comentario;
                                    oComentario.Idusuario = oJuegoCLS.idusuario;
                                    oComentario.Fechacomentario = DateTime.Now;
                                    oComentario.Habilitado = 1;
                                    baseDatos.Comentario.Add(oComentario);
                                    baseDatos.SaveChanges();
                                    rpta = 6;
                                }

                                transaccion.Complete();
                            }
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


        public static int ValidaFinal(string reseq01, string reseq02, int goleq01, int goleq02)
        {
            int resultado = 0;
            bool error = false;
            if (reseq01 == "G")
            {
                if (reseq02 != "P" || goleq01 <= goleq02)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq01 == "E" && !error)
            {
                if ((reseq02 != "E" && reseq02 != "GP") || goleq01 != goleq02)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq01 == "P" && !error)
            {
                if ((reseq02 != "G" && reseq02 != "GA") || goleq01 >= goleq02)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq01 == "GP" && !error)
            {
                if (reseq02 != "E" || goleq01 != goleq02)
                {
                    resultado = 10;
                    error = true;
                }
            }

            // ESTOY COMENTANDO PORQUE NO SE SI SIEMPRE LOS GANADOS ADMINISTRATIVAMENTE SON DE 2-0
            //if (reseq01 == "GA" && !error)
            //{
            //    if (goleq01 != 2 || goleq02 != 0)
            //    {
            //        resultado = 11;
            //        error = true;
            //    }
            //    else if (reseq02 != "P" && !error)
            //    {
            //        resultado = 10;
            //        error = true;
            //    }
            //}

            // EQUIPO02
            if (reseq02 == "G" && !error)
            {
                if (reseq01 != "P" || goleq02 <= goleq01)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq02 == "E" && !error)
            {
                if ((reseq01 != "E" && reseq01 != "GP") || goleq02 != goleq01)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq02 == "P" && !error)
            {
                if ((reseq01 != "G" && reseq01 != "GA") || goleq02 >= goleq01)
                {
                    resultado = 10;
                    error = true;
                }
            }

            if (reseq02 == "GP" && !error)
            {
                if (reseq01 != "E" || goleq02 != goleq01)
                {
                    resultado = 10;
                    error = true;
                }
            }

            // ESTOY COMENTANDO PORQUE NO SE SI SIEMPRE LOS GANADOS ADMINISTRATIVAMENTE SON DE 2-0
            //if (reseq02 == "GA" && !error)
            //{
            //    if (goleq01 != 0 || goleq02 != 2)
            //    {
            //        resultado = 12;
            //        error = true;
            //    }
            //    else if (reseq01 != "P")
            //    {
            //        resultado = 10;
            //        error = true;
            //    }
            //}

            return resultado;
        }


        [HttpGet]
        [Route("api/Juego/RecuperarInformacionJuego/{p_idjuego}")]
        public JuegoCLS RecuperarInformacionJuego(int p_idjuego)
        {
            JuegoCLS oJuegoCLS = new JuegoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oJuegoCLS = (from juego in baseDatos.Juego
                             join jornada in baseDatos.Jornada
                             on juego.Idjornada equals jornada.Idjornada
                             join equipo01 in baseDatos.Equipo
                             on juego.Idequipo01 equals equipo01.Idequipo
                             join equipo02 in baseDatos.Equipo
                             on juego.Idequipo02 equals equipo02.Idequipo
                             where juego.Idjuego == p_idjuego
                             select new JuegoCLS
                             {
                                 idjuego = juego.Idjuego,
                                 idjornada = juego.Idjornada.ToString(),
                                 idequipo01 = juego.Idequipo01.ToString(),
                                 idequipo02 = juego.Idequipo02.ToString(),
                                 idcampo = juego.Idcampo.ToString(),
                                 idarbitro = juego.Idarbitro.ToString(),
                                 idestatusjuego = juego.Idestatusjuego.ToString(),
                                 fhorario = (DateTime)juego.Fhorario,
                                 equipo01cadena = equipo01.Nombre,
                                 equipo02cadena = equipo02.Nombre,
                                 golesequipo01 = (int)juego.Golesequipo01,
                                 golesequipo02 = (int)juego.Golesequipo02,
                                 jornadacadena = jornada.Nombre,
                                 resequipo01 = juego.Resequipo01,
                                 resequipo02 = juego.Resequipo02,
                                 cuentaparapuntos = Convert.ToBoolean(juego.Cuentaparapuntos),
                                 cuentaparagoles = Convert.ToBoolean(juego.Cuentaparagoles),
                                 torneo = juego.Torneo,
                                 idtorneo = (int)juego.Idtorneo,
                                 peequipo01 = (int)juego.Peequipo01,
                                 peequipo02 = (int)juego.Peequipo02

                             }).First();

                int idequipo01 = int.Parse(oJuegoCLS.idequipo01);           // ESTO POSIBLEMENTE YA LO QUTE
                int idequipo02 = int.Parse(oJuegoCLS.idequipo02);

                // AQUI PRIMERO HAY QUE IR A LA TABLA GOL PARA TRAER LOS JUGADORES QUE ANOTARON EN ESE JUEGO, AL PRINCIPIO CLARO QUE ESTARA EN BLANCO
                // PERO SI ES UNA CONSULTA POSTERIOR PUEDE QUE HAYA GOLES REGISTRADOS DE JUGADORES QUE YA NO ESTAN EN EL EQUIPO Y NO LOS TRAERIA LA
                // CONSULTA DE ABAJO.
                // SE TIENE QUE VALIDAR QUE LOS JUGADORES QUE SALGAN DE LA CONSULTA DE ARRIBA YA NO LOS REPITA EN LA CONSULTA DE ABAJO.


                List<JugadorGolesCLS> listagoleadorescongoles01 = (from jugadoresgoles in baseDatos.Gol
                                                                       //join equipo in baseDatos.Equipo
                                                                       //on jugadoresgoles.Idequipo equals equipo.Idequipo
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



                // UNION    UNION NOS DA LA UNION DE LOS CONTENEDORES SIN REPETICIONES (LA DE GOL Y LA DE JUGADORES)


                List<JugadorGolesCLS> listagoleadoressingoles01 = (from jugador in baseDatos.Jugador
                                                                   join equipo in baseDatos.Equipo
                                                                   on jugador.Idequipo equals equipo.Idequipo
                                                                   orderby jugador.Nombre
                                                                   where jugador.Idequipo == idequipo01 && jugador.Habilitado == 1
                                                                   select new JugadorGolesCLS
                                                                   {
                                                                       idjugador = jugador.Idjugador,
                                                                       idequipo = idequipo01,
                                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                                       goles = RegresaGoles(p_idjuego, jugador.Idjugador)
                                                                   }).ToList();

                foreach (JugadorGolesCLS jug in listagoleadorescongoles01)
                {
                    oJuegoCLS.ListaJugadorGoles01.Add(jug);
                }
                foreach (JugadorGolesCLS jug in listagoleadoressingoles01)
                {
                    if (oJuegoCLS.ListaJugadorGoles01.Where(p => p.idjugador == jug.idjugador).Count() == 0)
                    {
                        oJuegoCLS.ListaJugadorGoles01.Add(jug);
                    }
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
                                                                       idequipo = idequipo02,
                                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                                       goles = (int)jugadoresgoles.Goles
                                                                   }).ToList();


                List<JugadorGolesCLS> listagoleadoressingoles02 = (from jugador in baseDatos.Jugador
                                                                   join equipo in baseDatos.Equipo
                                                                   on jugador.Idequipo equals equipo.Idequipo
                                                                   orderby jugador.Nombre
                                                                   where jugador.Idequipo == idequipo02 && jugador.Habilitado == 1
                                                                   select new JugadorGolesCLS
                                                                   {
                                                                       idjugador = jugador.Idjugador,
                                                                       idequipo = idequipo02,
                                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                                       goles = RegresaGoles(p_idjuego, jugador.Idjugador)
                                                                   }).ToList();

                foreach (JugadorGolesCLS jug in listagoleadorescongoles02)
                {
                    oJuegoCLS.ListaJugadorGoles02.Add(jug);
                }
                foreach (JugadorGolesCLS jug in listagoleadoressingoles02)
                {
                    if (oJuegoCLS.ListaJugadorGoles02.Where(p => p.idjugador == jug.idjugador).Count() == 0)
                    {
                        oJuegoCLS.ListaJugadorGoles02.Add(jug);
                    }
                }
            }
            return oJuegoCLS;
        }

        public static int RegresaGoles(int p_Juego, int p_Juagador)
        {
            int goles = 0;
            using (var bDatos = new FUTBOLEANDOContext())
            {
                int nveces = bDatos.Gol.Where(p => p.Idjuego == p_Juego && p.Idjugador == p_Juagador && p.Habilitado == 1).Count();
                if (nveces > 0)
                {
                    goles = (int)bDatos.Gol.Where(p => p.Idjuego == p_Juego && p.Idjugador == p_Juagador && p.Habilitado == 1).First().Goles;
                }
            }

            return goles;
        }

    }
}

