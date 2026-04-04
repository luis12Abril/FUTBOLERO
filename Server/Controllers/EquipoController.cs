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
    public class EquipoController : Controller
    {
        [HttpGet]
        [Route("api/Equipo/ListarEquipo/{idtorneoseleccionado}")]
        public List<EquipoCLS> ListarEquipo(string idtorneoseleccionado)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipo = (from Equipo in baseDatos.Equipo
                               orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                Equipo.Jugados,
                                Equipo.Difgoles descending,
                                Equipo.Golesafavor descending,
                                Equipo.Nombre
                               where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                               select new EquipoCLS
                               {
                                   idequipo = Equipo.Idequipo,
                                   nombre = Equipo.Nombre,
                                   representante = Equipo.Representante,
                                   puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                               }).ToList();
            }
            return listaEquipo;
        }

        [HttpGet]
        [Route("api/Equipo/ListarEquipoTodos/{idtorneoseleccionado}")]
        public List<EquipoCLS> ListarEquipoTodos(string idtorneoseleccionado)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipo = (from Equipo in baseDatos.Equipo
                               orderby Equipo.Nombre
                               where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado)
                               select new EquipoCLS
                               {
                                   idequipo = Equipo.Idequipo,
                                   nombre = Equipo.Nombre,
                                   representante = Equipo.Representante,
                                   puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                               }).ToList();
            }
            return listaEquipo;
        }

        [HttpGet]
        [Route("api/Equipo/ListarEquiposParticipantes/{idtorneoseleccionado}")]
        public List<EquipoCLS> ListarEquiposParticipantes(string idtorneoseleccionado)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipo = (from Equipo in baseDatos.Equipo
                               orderby Equipo.Nombre
                               where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                               select new EquipoCLS
                               {
                                   idequipo = Equipo.Idequipo,
                                   nombre = Equipo.Nombre,
                                   representante = Equipo.Representante,
                                   puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                               }).ToList();
            }
            return listaEquipo;
        }

        [HttpGet]
        [Route("api/Equipo/ListarEquiporalfabetico/{idtorneoseleccionado}")]
        public List<EquipoCLS> ListarEquipoalfabetico(string idtorneoseleccionado)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipo = (from Equipo in baseDatos.Equipo
                               orderby Equipo.Nombre
                               where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                               select new EquipoCLS
                               {
                                   idequipo = Equipo.Idequipo,
                                   nombre = Equipo.Nombre,
                                   representante = Equipo.Representante,
                                   puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                               }).ToList();
            }
            return listaEquipo;
        }

        [HttpGet]
        [Route("api/Equipo/FiltrarEquipo/{mensaje?}/{idtorneoseleccionado}")]
        public List<EquipoCLS> FiltrarEquipo(string mensaje, string idtorneoseleccionado)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaEquipo = (from Equipo in baseDatos.Equipo
                                   orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                     Equipo.Jugados,
                                     Equipo.Difgoles descending,
                                     Equipo.Golesafavor descending,
                                     Equipo.Nombre
                                   where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                                   select new EquipoCLS
                                   {
                                       idequipo = Equipo.Idequipo,
                                       nombre = Equipo.Nombre,
                                       representante = Equipo.Representante,
                                       puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                                   }).ToList();
                }
                else
                {
                    listaEquipo = (from Equipo in baseDatos.Equipo
                                   orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                     Equipo.Jugados,
                                     Equipo.Difgoles descending,
                                     Equipo.Golesafavor descending,
                                     Equipo.Nombre
                                   where Equipo.Habilitado == 1
                                   && Equipo.Nombre.Contains(mensaje) && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                                   select new EquipoCLS
                                   {
                                       idequipo = Equipo.Idequipo,
                                       nombre = Equipo.Nombre,
                                       representante = Equipo.Representante,
                                       puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                                   }).ToList();
                }
            }
            return listaEquipo;
        }




        [HttpPost]
        [Route("api/Equipo/GuardarDatosEquipo")]
        public int GuardarDatosEquipo([FromBody] EquipoCLS oEquipoCLS)
        {
            int rpta = 0;
            int nveces = 0;
            int yaexiste = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oEquipoCLS.idequipo == 0)
                        {
                            // VER SI ESTA EN LA TABLA EQUIPO, ESE NOMBRE DE EQUIPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Equipo.Where(p => p.Nombre.Trim().Equals(oEquipoCLS.nombre) && p.Idtorneo == oEquipoCLS.idtorneo && p.Habilitado == 1).Count();

                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(oEquipoCLS.usuequipo))
                                {
                                    if (string.IsNullOrWhiteSpace(oEquipoCLS.codigo))
                                    {
                                        rpta = 4;
                                    }
                                    else
                                    {
                                        // NO NECESITO VALIDARLO SI YA ESTA EN LA TABLA EQUIPO PORQUE TODOS LOS USUARIOS SE AGREGAN EN LA TABLA USUARIO
                                        yaexiste = baseDatos.Usuario.Where(p => p.Nombre.Trim() == oEquipoCLS.usuequipo.Trim() && p.Habilitado == 1).Count();
                                    }
                                        
                                }

                               
                                if (yaexiste > 0)
                                {
                                    rpta = 2;           // EL USUARIO YA EXISTE
                                }
                                else 
                                {
                                    if (rpta != 4)
                                    {
                                        string clave;
                                        if (oEquipoCLS.codigo== null)
                                        {
                                             clave = "";
                                        }
                                        else
                                        {
                                             clave = oEquipoCLS.codigo;
                                        }

                                        byte[] dataCifrada;
                                        using (SHA256 sha = SHA256.Create())
                                        {
                                            byte[] buffer = Encoding.Default.GetBytes(clave);
                                            dataCifrada = sha.ComputeHash(buffer);
                                        }
                                        string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                                        Equipo oEquipo = new Equipo();
                                        oEquipo.Nombre = oEquipoCLS.nombre;
                                        oEquipo.Representante = oEquipoCLS.representante;
                                        oEquipo.Fotoequipo = oEquipoCLS.fotoequipo;
                                        oEquipo.Torneo = "";       // NO LO VOY A USAR
                                        oEquipo.Idtorneo = oEquipoCLS.idtorneo;
                                        oEquipo.Jugados = 0;
                                        oEquipo.Ganados = 0;
                                        oEquipo.Empatados = 0;
                                        oEquipo.Perdidos = 0;
                                        oEquipo.Empatadosganados = 0;
                                        oEquipo.Empatadosperdidos = 0;
                                        oEquipo.Ganadosadmo = 0;
                                        oEquipo.Perdidosadmo = 0;
                                        oEquipo.Golesafavor = 0;
                                        oEquipo.Golesencontra = 0;
                                        oEquipo.Difgoles = 0;
                                        oEquipo.Puntos = 0;
                                        oEquipo.Habilitado = 1;

                                        oEquipo.Puntosextras = 0;
                                        oEquipo.Usuequipo = oEquipoCLS.usuequipo.Trim();
                                        oEquipo.Vigencia = oEquipoCLS.vigencia;  
                                        if (oEquipoCLS.codigo != "12345678*")
                                        {
                                            oEquipo.Claequipo = oEquipoCLS.codigo;
                                        }
                                        else
                                        {
                                            oEquipo.Claequipo = "";
                                        }


                                        baseDatos.Equipo.Add(oEquipo);
                                        baseDatos.SaveChanges();
                                        int idequipo = oEquipo.Idequipo;

                                        Jugador oJugador = new Jugador();
                                        oJugador.Nombre = " GOL A FAVOR DEL EQUIPO";        // TIENE UN ESPACIO AL INICIO, ESTO ES PARA QUE AL MOSTRAR LA LISTA SALGA PRIMERO
                                        oJugador.Appaterno = " ";
                                        oJugador.Apmaterno = " ";
                                        oJugador.Fnacimiento = DateTime.Now;
                                        oJugador.Idequipo = idequipo;
                                        oJugador.Goles = 0;
                                        oJugador.Torneo = "";       // NO LO VOY A USAR
                                        oJugador.Idtorneo = oEquipoCLS.idtorneo;
                                        oJugador.Habilitado = 1;
                                        baseDatos.Jugador.Add(oJugador);
                                        baseDatos.SaveChanges();


                                        if (oEquipoCLS.codigo != "12345678*")
                                        {
                                            int idarbitro = 0;

                                            Usuario oUsuario = new Usuario();
                                            oUsuario.Idpersona = 1;                 // ESTO LO VOY A QUITAR
                                            oUsuario.Contraseña = dataCifradaCadena;
                                            oUsuario.Idtipousuario = 6;             // EL NUMERO SEIS ES UN TIPO DE USUARIO ARDMINISTRADOR EQUIPO
                                            oUsuario.Nombre = oEquipoCLS.usuequipo.Trim();
                                            oUsuario.Idarbitrocolegio = idarbitro;

                                            oUsuario.Visitas = 0;
                                            oUsuario.Visitascel = 0;

                                            oUsuario.Token = "";       // NO LO VOY A USAR

                                            oUsuario.Habilitado = 1;

                                            oUsuario.Fechaalta = DateTime.Now;
                                            oUsuario.Origenalta = "WEE";

                                            baseDatos.Usuario.Add(oUsuario);
                                            baseDatos.SaveChanges();
                                        }

                                        rpta = 1;
                                    }
                                   
                                }
                                
                            }
                        }
                        else
                        {
                            // VER SI ESTA EN LA TABLA EQUIPO, ESE NOMBRE DEL EQUIPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Equipo.Where(p => p.Nombre.Trim().Equals(oEquipoCLS.nombre) && p.Idequipo != oEquipoCLS.idequipo
                            && p.Idtorneo == oEquipoCLS.idtorneo && p.Habilitado == 1).Count();

                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {

                                if (!string.IsNullOrWhiteSpace(oEquipoCLS.usuequipo))       // QUIERE DECIR QUE EL CAMPO USUEQUIPO NO ESTA VACIO
                                {
                                    if (oEquipoCLS.codigo == "12345678*" && (oEquipoCLS.usuequipo != oEquipoCLS.nomusuariocopia))       //NO SE ESCRIBIO EN CLAVE Y USUCLAVE ES DIFERENTE
                                    {
                                        rpta = 4;
                                    }
                                    else
                                    {
                                        // NO NECESITO VALIDARLO SI YA ESTA EN LA TABLA EQUIPO PORQUE TODOS LOS USUARIOS SE AGREGAN EN LA TABLA USUARIO
                                        if (oEquipoCLS.usuequipo.Trim() != oEquipoCLS.nomusuariocopia.Trim())
                                        {
                                            yaexiste = baseDatos.Usuario.Where(p => p.Nombre.Trim() == oEquipoCLS.usuequipo.Trim() && p.Habilitado == 1).Count();
                                        }                                       
                                    }

                                }

                                if (yaexiste > 0)
                                {
                                    rpta = 2;           // EL USUARIO YA EXISTE
                                }
                                else
                                {
                                    if (rpta != 4)
                                    {
                                        string clave = oEquipoCLS.codigo;
                                        byte[] dataCifrada;
                                        using (SHA256 sha = SHA256.Create())
                                        {
                                            byte[] buffer = Encoding.Default.GetBytes(clave);
                                            dataCifrada = sha.ComputeHash(buffer);
                                        }
                                        string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");


                                        Equipo oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == oEquipoCLS.idequipo).First();
                                        oEquipo.Nombre = oEquipoCLS.nombre;
                                        oEquipo.Representante = oEquipoCLS.representante;
                                        oEquipo.Fotoequipo = oEquipoCLS.fotoequipo;
                                        //oEquipo.Jugados = oEquipoCLS.jugados;
                                        //oEquipo.Ganados = oEquipoCLS.ganados;
                                        //oEquipo.Empatados = oEquipoCLS.empatados;
                                        //oEquipo.Perdidos = oEquipoCLS.perdidos;
                                        //oEquipo.Empatadosganados = oEquipoCLS.empatadosganados;
                                        //oEquipo.Empatadosperdidos = oEquipoCLS.empatadosperdidos;
                                        //oEquipo.Ganadosadmo = oEquipoCLS.ganadosadmo;
                                        //oEquipo.Perdidosadmo = oEquipoCLS.perdidosadmo;
                                        //oEquipo.Golesafavor = oEquipoCLS.golesafavor;
                                        //oEquipo.Golesencontra = oEquipoCLS.golesencontra;
                                        //oEquipo.Difgoles = oEquipoCLS.difgoles;
                                        //oEquipo.Puntos = oEquipoCLS.puntos;
                                        oEquipo.Habilitado = 1;

                                        oEquipo.Usuequipo = oEquipoCLS.usuequipo;
                                        // oEquipo.Claequipo = oEquipoCLS.codigo;
                                        oEquipo.Vigencia = oEquipoCLS.vigencia;

                                        if (oEquipoCLS.codigo != "12345678*")
                                        {
                                            oEquipo.Claequipo = oEquipoCLS.codigo;
                                        }


                                        baseDatos.SaveChanges();

                                        // ES EL NOMBRE DEL USUARIO QUE REGRESA EL CONTROLADOR Y NO SE MODIFICA
                                        // SI LA COPIA ESTA EN BLANCO Y EL USUEQUIPO NO, ENTONCES SE GRABA EN LA TABLA USUARIO
                                        if (string.IsNullOrWhiteSpace(oEquipoCLS.nomusuariocopia) && (!string.IsNullOrWhiteSpace(oEquipoCLS.usuequipo)))
                                        {
                                            int idarbitro = 0;

                                            Usuario oUsuario = new Usuario();
                                            oUsuario.Idpersona = 1;                 // ESTO LO VOY A QUITAR
                                            oUsuario.Contraseña = dataCifradaCadena;
                                            oUsuario.Idtipousuario = 6;             // EL NUMERO SEIS ES UN TIPO DE USUARIO ARDMINISTRADOR EQUIPO
                                            oUsuario.Nombre = oEquipoCLS.usuequipo.Trim();
                                            oUsuario.Idarbitrocolegio = idarbitro;

                                            oUsuario.Visitas = 0;
                                            oUsuario.Visitascel = 0;

                                            oUsuario.Token = "";       // NO LO VOY A USAR

                                            oUsuario.Habilitado = 1;

                                            oUsuario.Fechaalta = DateTime.Now;
                                            oUsuario.Origenalta = "WEE";

                                            baseDatos.Usuario.Add(oUsuario);
                                            baseDatos.SaveChanges();


                                            
                                        }
                                        else
                                        {
                                            if (oEquipoCLS.usuequipo != oEquipoCLS.nomusuariocopia)
                                            {
                                                // SI NO ESTA EN BLANCO EL USUEQUIPO ENTONCES SE BUSCA Y SE ACTUALIZA
                                                Usuario oUsuario = baseDatos.Usuario.Where(p => p.Nombre.Trim() == oEquipoCLS.nomusuariocopia.Trim()).First();
                                                oUsuario.Nombre = oEquipoCLS.usuequipo.Trim();
                                                oUsuario.Contraseña = dataCifradaCadena;
                                                if(oEquipoCLS.usuequipo.Trim() == "")
                                                {
                                                    oUsuario.Habilitado = 0;
                                                }
                                                baseDatos.SaveChanges();
                                            }

                                            //if (string.IsNullOrWhiteSpace(oEquipoCLS.usuequipo) || (string.IsNullOrWhiteSpace(oEquipoCLS.codigo)))
                                            //{
                                            //    Usuario oUsuario = baseDatos.Usuario.Where(p => p.Nombre.Trim() == oEquipoCLS.nomusuariocopia.Trim()).First();
                                            //    //oUsuario.Nombre = oEquipoCLS.usuequipo.Trim();
                                            //    //oUsuario.Contraseña = dataCifradaCadena;
                                            //    oUsuario.Habilitado = 0;
                                            //    baseDatos.SaveChanges();
                                            //}
                                        }


                                       
                                        


                                        rpta = 1;
                                    }
                                }

                                
                            }
                        }
                        transaccion.Complete();
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
        [Route("api/Equipo/RecuperarInformacionEquipo/{idEquipo}")]
        public EquipoCLS RecuperarInformacionEquipo(int idEquipo)
        {
            EquipoCLS oEquipoCLS = new EquipoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oEquipoCLS = (from Equipo in baseDatos.Equipo
                              where Equipo.Idequipo == idEquipo
                              select new EquipoCLS
                              {
                                  idequipo = Equipo.Idequipo,
                                  nombre = Equipo.Nombre,
                                  representante = Equipo.Representante,
                                  fotoequipo = Equipo.Fotoequipo,
                                  usuequipo = Equipo.Usuequipo,
                                  claequipo = Equipo.Claequipo,
                                  vigencia = (DateTime) Equipo.Vigencia,
                                  nomusuario = Equipo.Usuequipo,
                                  nomusuariocopia = Equipo.Usuequipo,
                                  codigo = "12345678*"
                              }).First();


                int con = 0;
                double totalDias = 0;
                DateTime fechaActual = DateTime.Today;
                List<JugadorCLS> listajugadores = (from jugador in baseDatos.Jugador
                                                   join equipo in baseDatos.Equipo
                                                   on jugador.Idequipo equals equipo.Idequipo
                                                   orderby jugador.Fnacimiento, jugador.Nombre
                                                   where jugador.Idequipo == idEquipo && jugador.Habilitado == 1 && !jugador.Nombre.Contains("GOL A FAVOR")
                                                   select new JugadorCLS
                                                   {
                                                       idjugador = jugador.Idjugador,
                                                       nombrecompleto = jugador.Nombre + " " + jugador.Appaterno + " " + jugador.Apmaterno,
                                                       goles = (int)jugador.Goles,
                                                       fnacimientocadena = jugador.Fnacimiento.Value.ToShortDateString(),
                                                       fnacimiento = (DateTime)jugador.Fnacimiento,
                                                       años = regresañosjugadorequipo(jugador.Fnacimiento)
                                                   }).ToList();

                foreach (JugadorCLS jug in listajugadores)
                {
                    con = con + 1;
                    jug.numero = con;
                    totalDias = totalDias + (fechaActual - jug.fnacimiento.Date).TotalDays;
                    oEquipoCLS.ListaJugadorEquipo.Add(jug);
                }

                if (con == 0)
                {

                    oEquipoCLS.años = 0;
                    oEquipoCLS.meses = 0;
                    oEquipoCLS.dias = 0;
                }
                else
                {
                    int promedioDias = (int)Math.Round(totalDias / con, MidpointRounding.AwayFromZero);
                    DateTime fechaNacimientoPromedio = fechaActual.AddDays(-promedioDias);

                    int añosPromedio = fechaActual.Year - fechaNacimientoPromedio.Year;
                    if (fechaNacimientoPromedio.AddYears(añosPromedio) > fechaActual)
                    {
                        añosPromedio--;
                    }

                    DateTime fechaTrasAnios = fechaNacimientoPromedio.AddYears(añosPromedio);

                    int mesesPromedio = 0;
                    while (fechaTrasAnios.AddMonths(mesesPromedio + 1) <= fechaActual)
                    {
                        mesesPromedio++;
                    }

                    DateTime fechaTrasMeses = fechaTrasAnios.AddMonths(mesesPromedio);
                    int diasPromedio = (fechaActual - fechaTrasMeses).Days;

                    oEquipoCLS.años = añosPromedio;
                    oEquipoCLS.meses = mesesPromedio;
                    oEquipoCLS.dias = diasPromedio;
                }


                return oEquipoCLS;
            }
        }


        [HttpGet]
        [Route("api/Equipo/RecuperarJuegosEquipo/{idEquipo}/{idtorneoseleccionado}")]
        public List<JuegosdelequipoCLS> RecuperarJuegosEquipo(int idEquipo, string idtorneoseleccionado)
        {
            List<JuegosdelequipoCLS> oJuegosdelequipoCLS = new List<JuegosdelequipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oJuegosdelequipoCLS = (from juego in baseDatos.Juego
                                       join jornada in baseDatos.Jornada
                                       on juego.Idjornada equals jornada.Idjornada
                                       join equipo01 in baseDatos.Equipo
                                       on juego.Idequipo01 equals equipo01.Idequipo
                                       join equipo02 in baseDatos.Equipo
                                       on juego.Idequipo02 equals equipo02.Idequipo
                                       join estatusjuego in baseDatos.Estatusjuego
                                       on juego.Idestatusjuego equals estatusjuego.Idestatusjuego
                                       orderby juego.Fhorario descending
                                       where (juego.Idequipo01 == idEquipo || juego.Idequipo02 == idEquipo)
                                       && juego.Idtorneo == int.Parse(idtorneoseleccionado) && juego.Habilitado == 1
                                       select new JuegosdelequipoCLS
                                       {
                                           nombrejornada = jornada.Nombre,
                                           fechajuego = juego.Fhorario.Value.ToString(),
                                           equipo1 = equipo01.Nombre,
                                           golequipo1 = (int)juego.Golesequipo01,
                                           golequipo2 = (int)juego.Golesequipo02,
                                           equipo2 = equipo02.Nombre,
                                           estatus = estatusjuego.Nombre
                                       }).ToList();

                return oJuegosdelequipoCLS;
            }
        }


        public static int regresañosjugadorequipo(DateTime? fnac)
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
        [Route("api/Equipo/EliminarEquipo/{idEquipo}")]
        public int EliminarEquipo(int idEquipo)
        {
            // SI UN EQUIPO SE BORRA SE DEBERAN DE BORRAR TODOS LOS JUGADORES QUE ESTAN RELACIONADOS A EL.
            // FALTA HACER ESTE PROCESO

            int rpta = 0;
            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    nveces = baseDatos.Juego.Where(p => (p.Idequipo01 == idEquipo) || (p.Idequipo02 == idEquipo) && p.Habilitado == 1).Count();
                    if (nveces > 0)
                    {
                        rpta = 2;
                    }
                    else
                    {
                        Equipo oEquipo = baseDatos.Equipo.Where(p => p.Idequipo == idEquipo).First();
                        oEquipo.Habilitado = 0;
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
        [Route("api/Equipo/RegresaIdEquipo/{usuariologueado}")]
        public int RegresaIdEquipo(string usuariologueado)
        {
            int rpta = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (int)baseDatos.Equipo.Where(p => p.Usuequipo.Trim() == usuariologueado.Trim() && p.Habilitado == 1).First().Idequipo;
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        // LO UTILIZO PARA MOSTRAR EL EQUIPO DE LOS ADMINISTRADOR DE EQUIPO
        [HttpGet]
        [Route("api/Equipo/ListarEquipoAdminEquipo/{idequipo}")]
        public List<EquipoCLS> ListarEquipoAdminEquipo(int idequipo)
        {
            List<EquipoCLS> listaEquipo = new List<EquipoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipo = (from Equipo in baseDatos.Equipo
                               orderby Equipo.Nombre
                               where Equipo.Habilitado == 1 && Equipo.Idequipo == idequipo
                               select new EquipoCLS
                               {
                                   idequipo = Equipo.Idequipo,
                                   nombre = Equipo.Nombre,
                                   representante = Equipo.Representante,
                                   puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras, 
                                   idtorneo = (int)Equipo.Idtorneo
                               }).ToList();
            }
            return listaEquipo;
        }

        [HttpGet]
        [Route("api/Equipo/RegresaFVigencia/{usuariologueado}")]
        public DateTime RegresaFVigencia(string usuariologueado)
        {
            DateTime rpta =DateTime.Now;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (DateTime) baseDatos.Equipo.Where(p => p.Usuequipo.Trim() == usuariologueado.Trim() && p.Habilitado == 1).First().Vigencia;
                }
            }
            catch (Exception ex)
            {
                rpta = rpta.AddDays(-1);
            }

            return rpta;
        }

    }
}


