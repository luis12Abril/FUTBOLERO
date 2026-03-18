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
    public class TorneoSeleccionadoController : Controller
    {
        [HttpGet]
        [Route("api/TorneoSeleccionado/EstadoSeleccionado2")]
        public List<EstadoCLS> EstadoSeleccionado2()
        {
            List<EstadoCLS> listaEstado = new List<EstadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEstado = (from estado in baseDatos.Estado
                               orderby estado.Nombre
                               where estado.Habilitado == 1
                               select new EstadoCLS
                               {
                                   idestado = estado.Idestado,
                                   nombre = estado.Nombre,
                               }).ToList();
            }
            return listaEstado;
        }


        [HttpGet]
        [Route("api/TorneoSeleccionado/MunicipioEstado2/{p_idestado}")]
        public List<MunicipioCLS> MunicipioEstado2(string p_idestado)
        {
            List<MunicipioCLS> listaMunicipio = new List<MunicipioCLS>();
            listaMunicipio = null;
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                //if (p_idestado == null)
                //{
                //    listaMunicipio = (from municipio in baseDatos.Municipio
                //                      orderby municipio.Nombre
                //                      where municipio.Habilitado == 1
                //                      select new MunicipioCLS
                //                      {
                //                          idmunicipio = municipio.Idmunicipio,
                //                          nombre = municipio.Nombre

                //                      }).ToList();
                //}
                //else
                //{
                    listaMunicipio = (from municipio in baseDatos.Municipio

                                      orderby municipio.Nombre
                                      where municipio.Habilitado == 1 && municipio.Idestado == int.Parse(p_idestado)
                                      select new MunicipioCLS
                                      {
                                          idmunicipio = municipio.Idmunicipio,
                                          nombre = municipio.Nombre
                                      }).ToList();
                }
            //}
            return listaMunicipio;
        }


        [HttpGet]
        [Route("api/TorneoSeleccionado/LigaMunicipio/{p_idmunicipio}")]
        public List<LigaCLS> LigaMunicipio(string p_idmunicipio)
        {
            List<LigaCLS> listaLiga = new List<LigaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                //if (p_idmunicipio == null || p_idmunicipio == "--- Seleccione ---")
                //{
                //    listaLiga = (from liga in baseDatos.Liga
                //                 orderby liga.Nombre
                //                 where liga.Habilitado == 1
                //                 select new LigaCLS
                //                 {
                //                     idliga = liga.Idliga,
                //                     nombre = liga.Nombre
                //                 }).ToList();
                //}
                //else
                //{
                    listaLiga = (from liga in baseDatos.Liga
                                 orderby liga.Nombre
                                 where liga.Habilitado == 1 && liga.Idmunicipio == int.Parse(p_idmunicipio)
                                 select new LigaCLS
                                 {
                                     idliga = liga.Idliga,
                                     nombre = liga.Nombre
                                 }).ToList();
                }
            //}
            return listaLiga;
        }



        [HttpGet]
        [Route("api/TorneoSeleccionado/MiTorneo/{p_idliga}")]
        public List<TorneoCLS> MiTorneo(string p_idliga)
        {
            List<TorneoCLS> listaTorneo = new List<TorneoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {

                listaTorneo = (from torneo in baseDatos.Torneo
                               orderby torneo.Ordentorneo
                               where torneo.Visible == 1 && torneo.Idliga == int.Parse(p_idliga)
                               select new TorneoCLS
                               {
                                   idtorneo = torneo.Idtorneo,
                                   nombre = torneo.Nombre
                               }).ToList();
            }


            return listaTorneo;
        }












        [HttpGet]
        [Route("api/TorneoSeleccionado/EstadoSeleccionado")]
        public List<TorneoSeleccionadoCLS> EstadoSeleccionado()
        {
            List<TorneoSeleccionadoCLS> listaEstado = new List<TorneoSeleccionadoCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEstado = (from estado in baseDatos.Estado
                               orderby estado.Nombre
                               where estado.Habilitado == 1
                               select new TorneoSeleccionadoCLS
                               {
                                   idestado = estado.Idestado.ToString(),
                                   estado = estado.Nombre,
                               }).ToList();
            }
            return listaEstado;
        }



        [HttpGet]
        [Route("api/TorneoSeleccionado/MunicipioEstado/{p_idestado}")]
        public List<TorneoSeleccionadoCLS> MunicipioEstado(string p_idestado)
        {
            List<TorneoSeleccionadoCLS> listaMunicipio = new List<TorneoSeleccionadoCLS>();
            listaMunicipio = null;
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idestado == null)
                {
                    listaMunicipio = (from municipio in baseDatos.Municipio
                                      join estado in baseDatos.Estado
                                      on municipio.Idestado equals estado.Idestado
                                      orderby estado.Nombre
                                      where municipio.Habilitado == 1
                                      select new TorneoSeleccionadoCLS
                                      {
                                          idmunicipio = municipio.Idmunicipio.ToString(),
                                          municipio = municipio.Nombre
                                         
                                      }).ToList();
                }
                else
                {
                    listaMunicipio = (from municipio in baseDatos.Municipio

                                      orderby municipio.Nombre
                                      where municipio.Habilitado == 1 && municipio.Idestado == int.Parse(p_idestado)
                                      select new TorneoSeleccionadoCLS
                                      {
                                          idmunicipio = municipio.Idmunicipio.ToString(),
                                          municipio = municipio.Nombre
                                      }).ToList();
                }
            }
            return listaMunicipio;
        }
    }
}
