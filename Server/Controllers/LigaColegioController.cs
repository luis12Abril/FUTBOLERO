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
    public class LigaColegioController : Controller
    {
        [HttpGet]
        [Route("api/LigaColegio/ListaLigaColegio")]
        public List<LigaColegioCLS> ListaLigaColegio()
        {
            List<LigaColegioCLS> listaLigaColegio = new List<LigaColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaLigaColegio = (from ligacolegio in baseDatos.Ligacolegio
                                    join colegio in baseDatos.Colegioarbitro
                                    on ligacolegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                    orderby ligacolegio.Nombre
                                    where ligacolegio.Habilitado == 1
                                    select new LigaColegioCLS
                                    {
                                        idligacolegio = ligacolegio.Idligacolegio,
                                        nombre = ligacolegio.Nombre,
                                        colegio = colegio.Nombre
                                    }).ToList();
            }
            return listaLigaColegio;
        }


        //// LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        //[HttpGet]
        //[Route("api/Municipio/FiltrarMunicipio/{p_idestado}")]
        //public List<MunicipioCLS> FiltrarMunicipio(string p_idestado)
        //{
        //    List<MunicipioCLS> listaMunicipio = new List<MunicipioCLS>();
        //    using (var baseDatos = new FUTBOLEANDOContext())
        //    {
        //        if (p_idestado == null || p_idestado == "--- Seleccione ---")
        //        {
        //            listaMunicipio = (from municipio in baseDatos.Municipio
        //                              join estado in baseDatos.Estado
        //                              on municipio.Idestado equals estado.Idestado
        //                              orderby estado.Nombre
        //                              where municipio.Habilitado == 1
        //                              select new MunicipioCLS
        //                              {
        //                                  idmunicipio = municipio.Idmunicipio,
        //                                  nombre = municipio.Nombre,
        //                                  estado = estado.Nombre
        //                              }).ToList();
        //        }
        //        else
        //        {
        //            listaMunicipio = (from municipio in baseDatos.Municipio
        //                              join estado in baseDatos.Estado
        //                              on municipio.Idestado equals estado.Idestado
        //                              orderby estado.Nombre
        //                              where municipio.Habilitado == 1 && municipio.Idestado == int.Parse(p_idestado)
        //                              select new MunicipioCLS
        //                              {
        //                                  idmunicipio = municipio.Idmunicipio,
        //                                  nombre = municipio.Nombre,
        //                                  estado = estado.Nombre
        //                              }).ToList();
        //        }
        //    }
        //    return listaMunicipio;
        //}



        [HttpPost]
        [Route("api/LigaColegio/GuardarDatosLigaColegio")]
        public int GuardarDatosLigaColegio([FromBody] LigaColegioCLS oLigaColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oLigaColegioCLS.idligacolegio == 0)
                    {
                        // VER SI ESTA EN LA TABLA LIGACOLEGIO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Ligacolegio.Where(p => (p.Nombre.Trim()).Equals(oLigaColegioCLS.nombre.Trim()) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Ligacolegio oLigaColegio = new Ligacolegio();
                            oLigaColegio.Nombre = oLigaColegioCLS.nombre;
                            oLigaColegio.Idcolegioarbitro = int.Parse(oLigaColegioCLS.idcolegioarbitro);
                            oLigaColegio.Habilitado = 1;
                            baseDatos.Ligacolegio.Add(oLigaColegio);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Ligacolegio.Where(p => (p.Nombre.Trim()).Equals(oLigaColegioCLS.nombre.Trim())
                      && p.Idligacolegio != oLigaColegioCLS.idligacolegio && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Ligacolegio oLigaColegio = baseDatos.Ligacolegio.Where(p => p.Idligacolegio == oLigaColegioCLS.idligacolegio).First();
                            oLigaColegio.Nombre = oLigaColegioCLS.nombre;
                            oLigaColegio.Idcolegioarbitro = int.Parse(oLigaColegioCLS.idcolegioarbitro);
                            oLigaColegio.Habilitado = 1;
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
        [Route("api/LigaColegio/RecuperarInformacionLigaColegio/{idLigaColegio}")]
        public LigaColegioCLS RecuperarInformacionLigaColegio(int idLigaColegio)
        {
            LigaColegioCLS oLigaColegioCLS = new LigaColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oLigaColegioCLS = (from ligacolegio in baseDatos.Ligacolegio
                                 where ligacolegio.Idligacolegio == idLigaColegio
                                   select new LigaColegioCLS
                                 {
                                     idligacolegio = ligacolegio.Idligacolegio,
                                     nombre = ligacolegio.Nombre,
                                     idcolegioarbitro = ligacolegio.Idcolegioarbitro.ToString()
                                 }).First();

                return oLigaColegioCLS;
            }
        }




        [HttpGet]
        [Route("api/LigaColegio/EliminarLigaColegio/{idLigaColegio}")]
        public int EliminarLigaColegio(int idLigaColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Ligacolegio oLigacolegio = baseDatos.Ligacolegio.Where(p => p.Idligacolegio == idLigaColegio).First();
                    oLigacolegio.Habilitado = 0;
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

    }
}


