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
    public class JugadorController : Controller
    {
        [HttpGet]
        [Route("api/Jugador/ListarJugador/{idtorneoseleccionado}")]
        public List<JugadorCLS> ListarJugador(string idtorneoseleccionado)
        {
            List<JugadorCLS> listaJugador = new List<JugadorCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaJugador = (from jugador in baseDatos.Jugador
                                join equipo in baseDatos.Equipo
                                 on jugador.Idequipo equals equipo.Idequipo
                                orderby jugador.Fnacimiento, equipo.Nombre
                                where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                select new JugadorCLS
                                {
                                    idjugador = jugador.Idjugador,
                                    nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                    equipo = equipo.Nombre,
                                    fnacimientocadena = regfechanacimientojugador(jugador.Fnacimiento),                //jugador.Fnacimiento.Value.ToLongDateString(),
                                    años = regresaños(jugador.Fnacimiento)
                                }).ToList();
            }
            return listaJugador;
        }

        public static int regresaños(DateTime? fnac)
        {
            int años = (DateTime.Now.Year - fnac.Value.Year);
            if (fnac.Value.AddYears(años) > DateTime.Now)
            {
                return años - 1;
            }
            else
            {
                return años;
            }
        }

        [HttpGet]
        [Route("api/Jugador/ListarTotalesPorAños/{idtorneoseleccionado}")]
        public List<JugadoresAñosCLS> ListarTotalesPorAños(string idtorneoseleccionado)
        {
            List<JugadoresAñosCLS> listaJugador = new List<JugadoresAñosCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                // QUE ESTE HABILITADO, QUE SEA DEL TORNEO SELECCIONADO, QUE NO SE LLAME GOL A FAVOR Y QUE SEA DIFERENTE A _SIN EQUIPO (INNER JOIN A LA TABLA EQUIPOS)
                listaJugador = (from jugador in baseDatos.Jugador
                                where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                //orderby jugador.Fnacimiento.Value.Year
                                group jugador by  new {jugador.Fnacimiento.Value.Year} into añoNacimiento                                

                                select new JugadoresAñosCLS
                                {
                                    años = añoNacimiento.Key.Year.ToString()
                                    //contadoraños = añoNacimiento.Select(x => x.Fnacimiento.Value.Year).Count()
                                }).ToList();

                int sumaJUgadores = 0;
                foreach(JugadoresAñosCLS JugOrden in listaJugador)
                {
                    JugOrden.contadoraños = baseDatos.Jugador.Where(j=> j.Habilitado == 1 && j.Idtorneo == int.Parse(idtorneoseleccionado) && int.Parse(JugOrden.años) == j.Fnacimiento.Value.Year ).Count().ToString();
                    sumaJUgadores = sumaJUgadores + int.Parse(JugOrden.contadoraños);
                }
                listaJugador.Add(new JugadoresAñosCLS { años = "--------", contadoraños = "------" });
                listaJugador.Add(new JugadoresAñosCLS { años = "TOTAL", contadoraños =sumaJUgadores.ToString()});
            }

            return listaJugador;
        }

        [HttpGet]
        [Route("api/Jugador/FiltrarTotalesPorAños/{p_idequipo?}/{idtorneoseleccionado}")]
        public List<JugadoresAñosCLS> FiltrarTotalesPorAños(string p_idequipo, string idtorneoseleccionado)
        {
            List<JugadoresAñosCLS> listaJugador = new List<JugadoresAñosCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idequipo == null || p_idequipo == "--- Seleccione ---")
                {
                    listaJugador = (from jugador in baseDatos.Jugador
                                    where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                    //orderby jugador.Fnacimiento.Value.Year
                                    group jugador by new { jugador.Fnacimiento.Value.Year } into añoNacimiento

                                    select new JugadoresAñosCLS
                                    {
                                        años = añoNacimiento.Key.Year.ToString()
                                        //contadoraños = añoNacimiento.Select(x => x.Fnacimiento.Value.Year).Count()
                                    }).ToList();

                    int sumaJUgadores = 0;
                    foreach (JugadoresAñosCLS JugOrden in listaJugador)
                    {
                        JugOrden.contadoraños = baseDatos.Jugador.Where(j => j.Habilitado == 1 && j.Idtorneo == int.Parse(idtorneoseleccionado) && int.Parse(JugOrden.años) == j.Fnacimiento.Value.Year).Count().ToString();
                        sumaJUgadores = sumaJUgadores + int.Parse(JugOrden.contadoraños);
                    }
                    listaJugador.Add(new JugadoresAñosCLS { años = "--------", contadoraños = "------" });
                    listaJugador.Add(new JugadoresAñosCLS { años = "TOTAL", contadoraños = sumaJUgadores.ToString() });
                }
                else
                {
                    listaJugador = (from jugador in baseDatos.Jugador
                                    where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado)
                                    && jugador.Idequipo == int.Parse(p_idequipo) && !jugador.Nombre.Contains("GOL A FAVOR")
                          
                                    group jugador by new { jugador.Fnacimiento.Value.Year } into añoNacimiento

                                    select new JugadoresAñosCLS
                                    {
                                        años = añoNacimiento.Key.Year.ToString()
                                        //contadoraños = añoNacimiento.Select(x => x.Fnacimiento.Value.Year).Count()
                                    }).ToList();

                    int sumaJugadores = 0;
                    foreach (JugadoresAñosCLS JugOrden in listaJugador)
                    {
                        JugOrden.contadoraños = baseDatos.Jugador.Where(j => j.Habilitado == 1 && j.Idtorneo == int.Parse(idtorneoseleccionado)
                        && j.Idequipo == int.Parse(p_idequipo) && int.Parse(JugOrden.años) == j.Fnacimiento.Value.Year).Count().ToString();
                        sumaJugadores = sumaJugadores + int.Parse(JugOrden.contadoraños);
                    }
                    listaJugador.Add(new JugadoresAñosCLS { años = "--------", contadoraños = "------" });
                    listaJugador.Add(new JugadoresAñosCLS { años = "TOTAL", contadoraños = sumaJugadores.ToString() });
                }
                   
            }

            return listaJugador;
        }



        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Jugador/FiltrarJugador/{p_idequipo?}/{idtorneoseleccionado}")]
        public List<JugadorCLS> FiltrarJugador(string p_idequipo, string idtorneoseleccionado)
        {
            List<JugadorCLS> listaJugador = new List<JugadorCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idequipo == null || p_idequipo == "--- Seleccione ---")
                {
                    listaJugador = (from jugador in baseDatos.Jugador
                                    join equipo in baseDatos.Equipo
                                    on jugador.Idequipo equals equipo.Idequipo
                                    orderby jugador.Fnacimiento, equipo.Nombre
                                    where jugador.Habilitado == 1 && jugador.Idtorneo == int.Parse(idtorneoseleccionado) && !jugador.Nombre.Contains("GOL A FAVOR")
                                    select new JugadorCLS
                                    {
                                        idjugador = jugador.Idjugador,
                                        nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                        equipo = equipo.Nombre,
                                        //fnacimientocadena = jugador.Fnacimiento.Value.ToLongDateString()
                                        fnacimientocadena = regfechanacimientojugador(jugador.Fnacimiento),     //jugador.Fnacimiento.Value.ToLongDateString(),
                                        años = regresaños(jugador.Fnacimiento)
                                    }).ToList();
                }
                else
                {
                    listaJugador = (from jugador in baseDatos.Jugador
                                    join equipo in baseDatos.Equipo
                                     on jugador.Idequipo equals equipo.Idequipo
                                    orderby jugador.Fnacimiento, equipo.Nombre
                                    where jugador.Habilitado == 1 && jugador.Idequipo == int.Parse(p_idequipo) && jugador.Idtorneo == int.Parse(idtorneoseleccionado)
                                    && !jugador.Nombre.Contains("GOL A FAVOR")
                                    select new JugadorCLS
                                    {
                                        idjugador = jugador.Idjugador,
                                        nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                        equipo = equipo.Nombre,
                                        fnacimientocadena = regfechanacimientojugador(jugador.Fnacimiento),     //jugador.Fnacimiento.Value.ToLongDateString(),
                                        años = regresaños(jugador.Fnacimiento)
                                    }).ToList();
                }
            }
            return listaJugador;
        }

        [HttpGet]
        [Route("api/Jugador/FiltrarJugadorParaGoles/{p_idequipo?}")]
        public List<JugadorGolesCLS> FiltrarJugadorParaGoles(string p_idequipo)
        {
            List<JugadorGolesCLS> listaJugador = new List<JugadorGolesCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {

                listaJugador = (from jugador in baseDatos.Jugador
                                join equipo in baseDatos.Equipo
                                on jugador.Idequipo equals equipo.Idequipo
                                orderby jugador.Nombre
                                where jugador.Habilitado == 1 && jugador.Idequipo == int.Parse(p_idequipo)
                                select new JugadorGolesCLS
                                {
                                    idjugador = jugador.Idjugador,
                                    idequipo = int.Parse(p_idequipo),
                                    nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                    goles = 0
                                }).ToList();
            }

            return listaJugador;
        }


        [HttpPost]
        [Route("api/Jugador/GuardarDatosJugador")]
        public int GuardarDatosJugador([FromBody] JugadorCLS oJugadorCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    // ESTO LO PONGO PORQUE EL APELLIDO MATERNO NO ES OBLIGATORIO
                    string apellidomaterno = (oJugadorCLS.apmaterno == null ? " " : oJugadorCLS.apmaterno);
                    string nombrecompletoformulario = oJugadorCLS.nombre.Trim() + oJugadorCLS.appaterno.Trim() + apellidomaterno.Trim();
                    if (oJugadorCLS.idjugador == 0)
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Jugador.Where(p => (p.Nombre.Trim() + p.Appaterno.Trim() + p.Apmaterno.Trim()).Equals(nombrecompletoformulario)
                        && p.Idtorneo == oJugadorCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Jugador oJugador = new Jugador();
                            oJugador.Nombre = oJugadorCLS.nombre;
                            oJugador.Appaterno = oJugadorCLS.appaterno;
                            oJugador.Apmaterno = oJugadorCLS.apmaterno;
                            oJugador.Fnacimiento = oJugadorCLS.fnacimiento;
                            oJugador.Idequipo = int.Parse(oJugadorCLS.idequipo);
                            oJugador.Goles = 0;
                            oJugador.Torneo = "";       // NO LO VOY A USAR
                            oJugador.Idtorneo = oJugadorCLS.idtorneo;
                            oJugador.Habilitado = 1;
                            baseDatos.Jugador.Add(oJugador);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Jugador.Where(p => (p.Nombre.Trim() + p.Appaterno.Trim() + p.Apmaterno.Trim()).Equals(nombrecompletoformulario)
                         && p.Idjugador != oJugadorCLS.idjugador && p.Idtorneo == oJugadorCLS.idtorneo && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == oJugadorCLS.idjugador).First();
                            oJugador.Nombre = oJugadorCLS.nombre;
                            oJugador.Appaterno = oJugadorCLS.appaterno;
                            oJugador.Apmaterno = oJugadorCLS.apmaterno;
                            oJugador.Fnacimiento = oJugadorCLS.fnacimiento;
                            oJugador.Idequipo = int.Parse(oJugadorCLS.idequipo);
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
        [Route("api/Jugador/RecuperarInformacionJugador/{idJugador}")]
        public JugadorCLS RecuperarInformacionJugador(int idJugador)
        {
            JugadorCLS oJugadorCLS = new JugadorCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oJugadorCLS = (from Jugador in baseDatos.Jugador
                               where Jugador.Idjugador == idJugador
                               select new JugadorCLS
                               {
                                   idjugador = Jugador.Idjugador,
                                   nombre = Jugador.Nombre,
                                   appaterno = Jugador.Appaterno,
                                   apmaterno = Jugador.Apmaterno,
                                   fnacimiento = (DateTime)Jugador.Fnacimiento,
                                   idequipo = Jugador.Idequipo.ToString()

                               }).First();

                return oJugadorCLS;
            }
        }


        [HttpGet]
        [Route("api/Jugador/EliminarJugador/{idJugador}/{idtorneoseleccionado}")]
        public int EliminarJugador(int idJugador, string idtorneoseleccionado)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Jugador oJugador = baseDatos.Jugador.Where(p => p.Idjugador == idJugador).First();
                    int idequipo = (int)baseDatos.Equipo.Where(p => p.Nombre.Contains("_SIN EQUIPO") && p.Idtorneo == int.Parse(idtorneoseleccionado)).First().Idequipo;
                    oJugador.Idequipo = idequipo;      // UN JUGADOR NO SE ELIMINA SOLO SE MANDA A _SIN EQUIPO/DISPONIBLE
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



        public static string regfechanacimientojugador(DateTime? fnacimiento)
        {
            string rfecha = "";

            rfecha = fnacimiento.Value.Day.ToString() + " de ";

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


        [HttpGet]
        [Route("api/Jugador/ListarJugadorEquipo/{idequipo}")]
        public List<JugadorCLS> ListarJugadorEquipo(string idequipo)
        {
            List<JugadorCLS> listaJugador = new List<JugadorCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaJugador = (from jugador in baseDatos.Jugador
                                join equipo in baseDatos.Equipo
                                 on jugador.Idequipo equals equipo.Idequipo
                                orderby jugador.Fnacimiento, equipo.Nombre
                                where jugador.Habilitado == 1 && jugador.Idequipo == int.Parse(idequipo) && !jugador.Nombre.Contains("GOL A FAVOR")
                                select new JugadorCLS
                                {
                                    idjugador = jugador.Idjugador,
                                    nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                    equipo = equipo.Nombre,
                                    fnacimientocadena = regfechanacimientojugador(jugador.Fnacimiento),                //jugador.Fnacimiento.Value.ToLongDateString(),
                                    años = regresaños(jugador.Fnacimiento)
                                }).ToList();
            }
            return listaJugador;
        }



        [HttpGet]
        [Route("api/Jugador/RecEA/{idusuario}")]
        public UsuarioEACLS RecEA(int idusuario)
        {
            UsuarioEACLS oUsuarioEACLS = new UsuarioEACLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oUsuarioEACLS = (from usuario in baseDatos.Usuario
                                 where usuario.Idusuario == idusuario
                                 select new UsuarioEACLS
                                 {
                                     idusuario = usuario.Idusuario,
                                     usuequipo = usuario.Nombre,
                                     idtipousuario = usuario.Idtipousuario
                                 }).First();


            }
            return oUsuarioEACLS;
        }


        [HttpGet]
        [Route("api/Jugador/RegresaUsuarioEA/{idusuario}")]
        public string RegresaUsuarioEA(int idusuario)
        {
            string rpta = "";

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (string)baseDatos.Usuario.Where(p => p.Idusuario == idusuario && p.Habilitado == 1).First().Nombre;
                }
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta.Trim();
        }



    }
}



