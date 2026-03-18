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
    public class EquipoInvitadoController : Controller
    {
        [HttpGet]
        [Route("api/EquipoInvitado/ListarEquipoInvitado/{idtorneoseleccionado}")]
        public List<EquipoInvitadoCLS> ListarEquipoInvitado(string idtorneoseleccionado)
        {
            List<EquipoInvitadoCLS> listaEquipoInvitado = new List<EquipoInvitadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipoInvitado = (from Equipo in baseDatos.Equipo
                                       orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                         Equipo.Jugados,
                                         Equipo.Difgoles descending,
                                         Equipo.Golesafavor descending,
                                         Equipo.Nombre
                                       where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                                       select new EquipoInvitadoCLS
                                       {
                                           idequipo = Equipo.Idequipo,
                                           nombre = Equipo.Nombre,
                                           representante = Equipo.Representante,
                                           puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                                       }).ToList();
            }
            return listaEquipoInvitado;
        }


        [HttpGet]
        [Route("api/EquipoInvitado/FiltrarEquipoInvitado/{mensaje?}/{idtorneoseleccionado}")]
        public List<EquipoInvitadoCLS> FiltrarEquipoInvitado(string mensaje, string idtorneoseleccionado)
        {
            List<EquipoInvitadoCLS> listaEquipoInvitado = new List<EquipoInvitadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaEquipoInvitado = (from Equipo in baseDatos.Equipo
                                           orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                            Equipo.Jugados,
                                            Equipo.Difgoles descending,
                                            Equipo.Golesafavor descending,
                                            Equipo.Nombre
                                           where Equipo.Habilitado == 1 && Equipo.Idtorneo == int.Parse(idtorneoseleccionado) && !Equipo.Nombre.Contains("_SIN EQUIPO")
                                           select new EquipoInvitadoCLS
                                           {
                                               idequipo = Equipo.Idequipo,
                                               nombre = Equipo.Nombre,
                                               representante = Equipo.Representante,
                                               puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras
                                           }).ToList();
                }
                else
                {
                    listaEquipoInvitado = (from Equipo in baseDatos.Equipo
                                           orderby (Equipo.Puntos + Equipo.Puntosextras) descending,
                                             Equipo.Jugados,
                                             Equipo.Difgoles descending,
                                             Equipo.Golesafavor descending,
                                             Equipo.Nombre
                                           where Equipo.Habilitado == 1
                                           && Equipo.Nombre.Contains(mensaje)
                                           && Equipo.Idtorneo == int.Parse(idtorneoseleccionado)
                                           && !Equipo.Nombre.Contains("_SIN EQUIPO")
                                           select new EquipoInvitadoCLS
                                           {
                                               idequipo = Equipo.Idequipo,
                                               nombre = Equipo.Nombre,
                                               representante = Equipo.Representante,
                                               puntos = (int)Equipo.Puntos + (int)Equipo.Puntosextras   
                                           }).ToList();
                }
            }
            return listaEquipoInvitado;
        }


        [HttpGet]
        [Route("api/EquipoInvitado/RecuperarInformacionEquipoInvitado/{idEquipo}")]
        public EquipoInvitadoCLS RecuperarInformacionEquipoInvitado(int idEquipo)
        {
            EquipoInvitadoCLS oEquipoInvitadoCLS = new EquipoInvitadoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oEquipoInvitadoCLS = (from Equipo in baseDatos.Equipo
                                      where Equipo.Idequipo == idEquipo
                                      select new EquipoInvitadoCLS
                                      {
                                          idequipo = Equipo.Idequipo,
                                          nombre = Equipo.Nombre,
                                          representante = Equipo.Representante,
                                          fotoequipo = Equipo.Fotoequipo
                                      }).First();


                int con = 0;
                int meses = 0;
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
                                                       años = regresañosjugadorequipoinvitado(jugador.Fnacimiento)
                                                   }).ToList();

                foreach (JugadorCLS jug in listajugadores)
                {
                    con = con + 1;
                    jug.numero = con;
                    meses = meses + ((DateTime.Now.Month + DateTime.Now.Year * 12) - (jug.fnacimiento.Month + jug.fnacimiento.Year * 12));
                    oEquipoInvitadoCLS.ListaJugadorEquipoInvitado.Add(jug);
                }

                if (con == 0)
                {
                    oEquipoInvitadoCLS.años = 0;
                    oEquipoInvitadoCLS.meses = 0;
                }
                else
                {
                    meses = meses / con;
                    oEquipoInvitadoCLS.años = meses / 12;
                    oEquipoInvitadoCLS.meses = meses % 12;
                }


                return oEquipoInvitadoCLS;
            }
        }


        public static int regresañosjugadorequipoinvitado(DateTime? fnac)
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


    }
}


