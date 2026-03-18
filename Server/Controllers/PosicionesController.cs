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
//using FUTBOLEANDO.Server.Clases;        No LO VOY A OCUPAR PORQUE NO VOY A MANDAR CORREOS

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class PosicionesController : Controller
    {
        [HttpGet]
        [Route("api/Posiciones/ListarPosiciones/{idtorneoseleccionado}")]
        public List<PosicionesCLS> ListarPosiciones(string idtorneoseleccionado)
        {
            List<PosicionesCLS> listaPosiciones = new List<PosicionesCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaPosiciones = (from posiciones in baseDatos.Equipo
                                   orderby (posiciones.Puntos + posiciones.Puntosextras) descending,
                                   posiciones.Jugados,
                                   posiciones.Difgoles descending,
                                   posiciones.Golesafavor descending,
                                   posiciones.Nombre
                                   where posiciones.Habilitado == 1 && posiciones.Idtorneo == int.Parse(idtorneoseleccionado) && !posiciones.Nombre.Contains("_SIN EQUIPO")
                                   select new PosicionesCLS
                                   {
                                       nombre = posiciones.Nombre,
                                       jugados = (int)posiciones.Jugados,
                                       ganados = (int)posiciones.Ganados,
                                       empatados = (int)posiciones.Empatados,
                                       perdidos = (int)posiciones.Perdidos,
                                       empatadosganados = (int)posiciones.Empatadosganados,
                                       golesafavor = (int)posiciones.Golesafavor,
                                       golesencontra = (int)posiciones.Golesencontra,
                                       diferenciagoles = (int)posiciones.Difgoles,
                                       puntos = (int)posiciones.Puntos + (int)posiciones.Puntosextras,
                                       torneo = posiciones.Torneo,
                                       idtorneo = (int)posiciones.Idtorneo,
                                       puntosextras = (int)posiciones.Puntosextras


                                   }).ToList();
            }
            return listaPosiciones;
        }

    }
}


