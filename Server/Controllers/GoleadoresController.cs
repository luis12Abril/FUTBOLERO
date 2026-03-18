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
    public class GoleadoresController : Controller
    {
        [HttpGet]
        [Route("api/Goleadores/ListarGoleadores/{idtorneoseleccionado}")]
        public List<GoleadoresCLS> ListarGoleadores(string idtorneoseleccionado)
        {
            List<GoleadoresCLS> listaGoleadores = new List<GoleadoresCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaGoleadores = (from goleadores in baseDatos.Jugador
                                   join equipo in baseDatos.Equipo
                                   on goleadores.Idequipo equals equipo.Idequipo
                                   orderby goleadores.Goles descending, equipo.Nombre
                                   where goleadores.Habilitado == 1 && goleadores.Goles > 0 && goleadores.Idtorneo == int.Parse(idtorneoseleccionado)
                                   && !goleadores.Nombre.Contains("GOL A FAVOR")
                                   select new GoleadoresCLS
                                   {
                                       nombre = goleadores.Nombre + " " + goleadores.Appaterno + " " + goleadores.Apmaterno,
                                       equipo = equipo.Nombre,
                                       goles = (int)goleadores.Goles

                                   }).ToList();
            }
            return listaGoleadores;
        }

        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Goleadores/ListarGoleadores/{p_idequipo?}/{idtorneoseleccionado}")]
        public List<GoleadoresCLS> FiltrarGoleadores(string p_idequipo, string idtorneoseleccionado)
        {
            List<GoleadoresCLS> listaGoleadores = new List<GoleadoresCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idequipo == null || p_idequipo == "--- Seleccione ---")
                {
                    listaGoleadores = (from goleadores in baseDatos.Jugador
                                       join equipo in baseDatos.Equipo
                                       on goleadores.Idequipo equals equipo.Idequipo
                                       orderby goleadores.Goles descending, equipo.Nombre
                                       where goleadores.Habilitado == 1 && goleadores.Goles > 0 && goleadores.Idtorneo == int.Parse(idtorneoseleccionado)
                                       && !goleadores.Nombre.Contains("GOL A FAVOR")
                                       select new GoleadoresCLS
                                       {
                                           nombre = goleadores.Nombre + " " + goleadores.Appaterno + " " + goleadores.Apmaterno,
                                           equipo = equipo.Nombre,
                                           goles = (int)goleadores.Goles

                                       }).ToList();
                }
                else
                {
                    listaGoleadores = (from goleadores in baseDatos.Jugador
                                       join equipo in baseDatos.Equipo
                                       on goleadores.Idequipo equals equipo.Idequipo
                                       orderby goleadores.Goles descending, equipo.Nombre
                                       where goleadores.Habilitado == 1 && goleadores.Goles > 0 && goleadores.Idtorneo == int.Parse(idtorneoseleccionado)
                                       && goleadores.Idequipo == int.Parse(p_idequipo)
                                       select new GoleadoresCLS
                                       {
                                           nombre = goleadores.Nombre + " " + goleadores.Appaterno + " " + goleadores.Apmaterno,
                                           equipo = equipo.Nombre,
                                           goles = (int)goleadores.Goles

                                       }).ToList();
                }
            }
            return listaGoleadores;
        }


    }
}
