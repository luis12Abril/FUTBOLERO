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
//using FUTBOLEANDO.Server.Clases;    ESTO NO VA PORQUE NO VOY A MANDAR NINGUN CORREO

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class TorneoController : Controller
    {
        [HttpGet]
        [Route("api/Torneo/ListarTorneo")]
        public List<TorneoCLS> ListarTorneo()
        {
            List<TorneoCLS> listaTorneo = new List<TorneoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaTorneo = (from torneo in baseDatos.Torneo
                               orderby torneo.Ordentorneo
                               where torneo.Habilitado == 1
                               select new TorneoCLS
                               {
                                   idtorneo = torneo.Idtorneo,
                                   nombre = torneo.Nombre,
                                   //clavetorneo = torneo.Ordentorneo.ToString(),
                                   ordentorneo = (int)torneo.Ordentorneo,
                                   torneovisible = torneo.Visible == 1 ? "Si" : "No",
                                   visitas = (int)torneo.Visitas
                               }).ToList();
            }
            return listaTorneo;
        }



        [HttpGet]
        [Route("api/Torneo/ListarTorneoSeleccione")]
        public List<TorneoCLS> ListarTorneoSeleccione()
        {
            List<TorneoCLS> listaTorneo = new List<TorneoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaTorneo = (from torneo in baseDatos.Torneo
                               orderby torneo.Ordentorneo
                               where torneo.Visible == 1
                               select new TorneoCLS
                               {
                                   idtorneo = torneo.Idtorneo,
                                   nombre = torneo.Nombre
                               }).ToList();
            }
            return listaTorneo;
        }


        [HttpGet]
        [Route("api/Torneo/FiltrarTorneo/{data?}")]
        public List<TorneoCLS> FiltrarTorneo(string data)
        {
            List<TorneoCLS> listaTorneo = new List<TorneoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (data == null)
                {
                    listaTorneo = (from torneo in baseDatos.Torneo
                                   orderby torneo.Ordentorneo
                                   where torneo.Habilitado == 1
                                   select new TorneoCLS
                                   {
                                       idtorneo = torneo.Idtorneo,
                                       nombre = torneo.Nombre,
                                       //clavetorneo = torneo.Ordentorneo.ToString(),
                                       ordentorneo = (int)torneo.Ordentorneo,
                                       torneovisible = torneo.Visible == 1 ? "Si" : "No",
                                       visitas = (int)torneo.Visitas
                                   }).ToList();
                }
                else
                {
                    listaTorneo = (from torneo in baseDatos.Torneo
                                   orderby torneo.Ordentorneo
                                   where torneo.Habilitado == 1 && torneo.Nombre.Contains(data)
                                   select new TorneoCLS
                                   {
                                       idtorneo = torneo.Idtorneo,
                                       nombre = torneo.Nombre,
                                       //clavetorneo = torneo.Ordentorneo.ToString(),
                                       ordentorneo = (int)torneo.Ordentorneo,
                                       torneovisible = torneo.Visible == 1 ? "Si" : "No",
                                       visitas = (int)torneo.Visitas
                                   }).ToList();
                }

            }
            return listaTorneo;
        }


        [HttpPost]
        [Route("api/Torneo/GuardarDatosTorneo")]
        public int GuardarDatosTorneo([FromBody] TorneoCLS oTorneoCLS)
        {
            int rpta = 100;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oTorneoCLS.idtorneo == 0)
                        {
                            Torneo oTorneo = new Torneo();
                            oTorneo.Nombre = oTorneoCLS.nombre;
                            oTorneo.Clavetorneo = "";       // NO LO VOY A USAR
                            oTorneo.Ordentorneo = oTorneoCLS.ordentorneo;
                            oTorneo.Visible = Convert.ToInt32(oTorneoCLS.visible);
                            oTorneo.Habilitado = 1;
                            oTorneo.Visitas = 0;
                            oTorneo.Visitascel = 0;
                            oTorneo.Idliga = Convert.ToInt32(oTorneoCLS.idliga);
                            //oTorneo.Nivel1 = 1;
                            //oTorneo.Nivel2 = 1;
                            //oTorneo.Nivel3 = 1;

                            baseDatos.Torneo.Add(oTorneo);
                            baseDatos.SaveChanges();

                            // ES EL IDTORNEO DEL TORNEO QUE SE ACABA DE CREAR
                            int idnuevotorneo = oTorneo.Idtorneo;

                            Equipo oEquipo = new Equipo();
                            oEquipo.Nombre = "_SIN EQUIPO";
                            oEquipo.Representante = " ";
                            oEquipo.Fotoequipo = " ";
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
                            oEquipo.Torneo = "";       // NO LO VOY A USAR
                            oEquipo.Idtorneo = idnuevotorneo;
                            baseDatos.Equipo.Add(oEquipo);
                            baseDatos.SaveChanges();

                            Campo oCampo = new Campo();
                            oCampo.Nombre = " PENDIENTE";
                            oCampo.Ubicacion = " ";
                            oCampo.Habilitado = 1;
                            oCampo.Torneo = "";       // NO LO VOY A USAR
                            oCampo.Idtorneo = idnuevotorneo;
                            baseDatos.Campo.Add(oCampo);
                            baseDatos.SaveChanges();

                            Arbitro oArbitro = new Arbitro();
                            oArbitro.Nombre = " PENDIENTE";
                            oArbitro.Appaterno = " ";
                            oArbitro.Apmaterno = " ";
                            oArbitro.Habilitado = 1;
                            oArbitro.Torneo = "";       // NO LO VOY A USAR
                            oArbitro.Idtorneo = idnuevotorneo;
                            baseDatos.Arbitro.Add(oArbitro);
                            baseDatos.SaveChanges();

                            Estatusjuego oEstatusjuego = new Estatusjuego();
                            oEstatusjuego.Nombre = " PENDIENTE";
                            oEstatusjuego.Habilitado = 1;
                            oEstatusjuego.Torneo = "";       // NO LO VOY A USAR
                            oEstatusjuego.Idtorneo = idnuevotorneo;
                            baseDatos.Estatusjuego.Add(oEstatusjuego);
                            baseDatos.SaveChanges();

                            oEstatusjuego = new Estatusjuego();
                            oEstatusjuego.Nombre = " JUGADO";
                            oEstatusjuego.Habilitado = 1;
                            oEstatusjuego.Torneo = "";       // NO LO VOY A USAR
                            oEstatusjuego.Idtorneo = idnuevotorneo;
                            baseDatos.Estatusjuego.Add(oEstatusjuego);
                            baseDatos.SaveChanges();

                            rpta = 1;
                        }
                        else
                        {
                            Torneo oTorneo = baseDatos.Torneo.Where(p => p.Idtorneo == oTorneoCLS.idtorneo).First();
                            oTorneo.Nombre = oTorneoCLS.nombre;
                            oTorneo.Clavetorneo = "";       // NO LO VOY A USAR
                            oTorneo.Ordentorneo = oTorneoCLS.ordentorneo;
                            oTorneo.Visible = Convert.ToInt32(oTorneoCLS.visible);
                            oTorneo.Habilitado = 1;
                            oTorneo.Idliga = Convert.ToInt32(oTorneoCLS.idliga);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }

                        transaccion.Complete();
                        rpta = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = 100;
            }
            return rpta;
        }


        [HttpGet]
        [Route("api/Torneo/RecuperarInformacionTorneo/{idTorneo}")]
        public TorneoCLS RecuperarInformacionTorneo(int idTorneo)
        {
            TorneoCLS oTorneoCLS = new TorneoCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oTorneoCLS = (from torneo in baseDatos.Torneo
                              where torneo.Idtorneo == idTorneo
                              select new TorneoCLS
                              {
                                  idtorneo = torneo.Idtorneo,
                                  nombre = torneo.Nombre,

                                  clavetorneo = torneo.Ordentorneo.ToString(),
                                  ordentorneo = (int)torneo.Ordentorneo,

                                  visible = Convert.ToBoolean(torneo.Visible),
                                  torneovisible = torneo.Visible == 1 ? "Si" : "No",
                                  visitas = (int)torneo.Visitas,
                                  idliga = torneo.Idliga.ToString()
                              }).First();

                return oTorneoCLS;
            }
        }

        [HttpGet]
        [Route("api/Torneo/VisitasTorneo/{idtorneo}")]
        public int VisitasTorneo(string idtorneo)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Torneo oTorneo = baseDatos.Torneo.Where(p => p.Idtorneo == int.Parse(idtorneo)).First();
                    oTorneo.Visitas = oTorneo.Visitas + 1;
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


        [HttpGet]
        [Route("api/Torneo/TotalVisitasTorneos")]
        public int TotalVisitasTorneos()
        {
            int visitastotales = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    visitastotales = (int)baseDatos.Torneo.Where(p => p.Habilitado == 1).Sum(x => x.Visitas);
                }
            }
            catch (Exception ex)
            {
                visitastotales = 0;
            }

            return visitastotales;
        }


    }
}

