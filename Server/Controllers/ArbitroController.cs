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

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class ArbitroController : Controller
    {
        [HttpGet]
        [Route("api/Arbitro/ListarArbitro/{idtorneoseleccionado}")]
        public List<ArbitroCLS> ListarArbitro(string idtorneoseleccionado)
        {
            List<ArbitroCLS> listaArbitro = new List<ArbitroCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaArbitro = (from arbitro in baseDatos.Arbitro
                                orderby arbitro.Nombre
                                where arbitro.Habilitado == 1 && arbitro.Idtorneo == int.Parse(idtorneoseleccionado)
                                select new ArbitroCLS
                                {
                                    idarbitro = arbitro.Idarbitro,
                                    nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno + " " + arbitro.Apmaterno
                                    //nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno + " " + (arbitro.Apmaterno == null ? " " : arbitro.Apmaterno)
                                }).ToList();
            }


            return listaArbitro;
        }

        [HttpGet]
        [Route("api/Arbitro/FiltrarArbitro/{mensaje?}/{idtorneoseleccionado}")]
        public List<ArbitroCLS> FiltrarArbitro(string mensaje, string idtorneoseleccionado)
        {
            List<ArbitroCLS> listaArbitro = new List<ArbitroCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaArbitro = (from arbitro in baseDatos.Arbitro
                                    orderby arbitro.Nombre
                                    where arbitro.Habilitado == 1 && arbitro.Idtorneo == int.Parse(idtorneoseleccionado)
                                    select new ArbitroCLS
                                    {
                                        idarbitro = arbitro.Idarbitro,
                                        nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno + " " + arbitro.Apmaterno
                                    }).ToList();
                }
                else
                {
                    listaArbitro = (from arbitro in baseDatos.Arbitro
                                    orderby arbitro.Nombre
                                    where arbitro.Habilitado == 1
                                    && (arbitro.Nombre.Contains(mensaje) || arbitro.Appaterno.Contains(mensaje) || arbitro.Apmaterno.Contains(mensaje))
                                    && arbitro.Idtorneo == int.Parse(idtorneoseleccionado)
                                    select new ArbitroCLS
                                    {
                                        idarbitro = arbitro.Idarbitro,
                                        nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno + " " + arbitro.Apmaterno
                                    }).ToList();
                }
            }
            return listaArbitro;
        }



        [HttpPost]
        [Route("api/Arbitro/GuardarDatosArbitro")]
        public int GuardarDatosArbitro([FromBody] ArbitroCLS oArbitroCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    // ESTO LO PONGO PORQUE EL APLLIDO MATERNO NO ES OBLIGATORIO
                    string apellidomaterno = (oArbitroCLS.apmaterno == null ? " " : oArbitroCLS.apmaterno);
                    string nombrecompletoformulario = oArbitroCLS.nombre.Trim() + oArbitroCLS.appaterno.Trim() + apellidomaterno.Trim();
                    if (oArbitroCLS.idarbitro == 0)
                    {
                        if (oArbitroCLS.nombre.Trim().ToUpper() == "PENDIENTE")
                        {
                            rpta = 4;
                        }
                        else
                        {
                            // VER SI ESTA EN LA TABLA ARBITRO, ESE NOMBRE COMPLETO DEL ARBITRO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Arbitro.Where(p => (p.Nombre.Trim() + p.Appaterno.Trim() + p.Apmaterno.Trim()).Equals(nombrecompletoformulario)
                            && p.Idtorneo == oArbitroCLS.idtorneo && p.Habilitado == 1).Count();
                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {
                                Arbitro oArbitro = new Arbitro();
                                oArbitro.Nombre = oArbitroCLS.nombre;
                                oArbitro.Appaterno = oArbitroCLS.appaterno;
                                oArbitro.Apmaterno = (oArbitroCLS.apmaterno == null ? " " : oArbitroCLS.apmaterno);
                                oArbitro.Torneo = "";       // NO LO VOY A USAR
                                oArbitro.Idtorneo = oArbitroCLS.idtorneo;
                                oArbitro.Habilitado = 1;
                                baseDatos.Arbitro.Add(oArbitro);
                                baseDatos.SaveChanges();
                                rpta = 1;
                            }
                        }

                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA ARBITRO, ESE NOMBRE COMPLETO DEL ARBITRO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Arbitro.Where(p => (p.Nombre.Trim() + p.Appaterno.Trim() + p.Apmaterno.Trim()).Equals(nombrecompletoformulario)
                        && p.Idarbitro != oArbitroCLS.idarbitro && p.Idtorneo == oArbitroCLS.idtorneo && p.Habilitado == 1).Count();

                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Arbitro oArbitro = baseDatos.Arbitro.Where(p => p.Idarbitro == oArbitroCLS.idarbitro).First();
                            oArbitro.Nombre = oArbitroCLS.nombre;
                            oArbitro.Appaterno = oArbitroCLS.appaterno;
                            oArbitro.Apmaterno = (oArbitroCLS.apmaterno == null ? " " : oArbitroCLS.apmaterno);
                            oArbitro.Habilitado = 1;
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
        [Route("api/Arbitro/EliminarArbitro/{idarbitro}")]
        public int EliminarArbitro(int idarbitro)
        {
            int rpta = 0;
            int nveces = 0;
            // EL IDARBITRO = 1 ES EL PENDIENTE, ESE NO LO VAMOS A ELIMINAR

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Arbitro oArbitro = baseDatos.Arbitro.Where(p => p.Idarbitro == idarbitro).First();
                    if (oArbitro.Nombre.Trim().ToUpper() == "PENDIENTE")
                    {
                        rpta = 3;
                    }
                    else
                    {
                        nveces = baseDatos.Juego.Where(p => p.Idarbitro == idarbitro && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 2;
                        }
                        else
                        {
                            oArbitro.Habilitado = 0;
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
        [Route("api/Arbitro/RecuperarInformacionArbitro/{idarbitro}")]
        public ArbitroCLS RecuperarInformacionArbitro(int idarbitro)
        {
            ArbitroCLS oArbitroCLS = new ArbitroCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oArbitroCLS = (from arbitro in baseDatos.Arbitro
                               where arbitro.Idarbitro == idarbitro
                               select new ArbitroCLS
                               {
                                   idarbitro = arbitro.Idarbitro,
                                   nombre = arbitro.Nombre,
                                   appaterno = arbitro.Appaterno,
                                   apmaterno = arbitro.Apmaterno,
                                   //apmaterno = (arbitro.Apmaterno == null ? " " : arbitro.Apmaterno)
                               }).First();

                return oArbitroCLS;
            }
        }

    }
}
