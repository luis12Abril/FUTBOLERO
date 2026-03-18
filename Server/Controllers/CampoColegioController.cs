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
    public class CampoColegioController : Controller
    {
        [HttpGet]
        [Route("api/CampoColegio/ListarCampoColegio/{idcolegio}")]
        public List<CampoColegioCLS> ListarCampoColegio(string idcolegio)
        {
            List<CampoColegioCLS> listaCampoColegio = new List<CampoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaCampoColegio = (from campocolegio in baseDatos.Campocolegio
                                    join colegio in baseDatos.Colegioarbitro
                                    on campocolegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                    orderby campocolegio.Nombre
                                    where campocolegio.Habilitado == 1 && campocolegio.Idcolegioarbitro == int.Parse(idcolegio)
                                     select new CampoColegioCLS
                                    {
                                        idcampocolegio = campocolegio.Idcampocolegio,
                                        nombre = campocolegio.Nombre,
                                        ubicacion = campocolegio.Ubicacion,
                                        colegio = colegio.Nombre
                                    }).ToList();
            }
            return listaCampoColegio;
        }

        [HttpGet]
        [Route("api/CampoColegio/ListarCampoColegio1")]
        public List<CampoColegioCLS> ListarCampoColegio1()
        {
            List<CampoColegioCLS> listaCampoColegio = new List<CampoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaCampoColegio = (from campocolegio in baseDatos.Campocolegio
                                     join colegio in baseDatos.Colegioarbitro
                                     on campocolegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                     orderby campocolegio.Nombre
                                     where campocolegio.Habilitado == 1
                                     select new CampoColegioCLS
                                     {
                                         idcampocolegio = campocolegio.Idcampocolegio,
                                         nombre = campocolegio.Nombre,
                                         ubicacion = campocolegio.Ubicacion,
                                         colegio = colegio.Nombre
                                     }).ToList();
            }
            return listaCampoColegio;
        }



        [HttpGet]
        [Route("api/CampoColegio/FiltrarCampoColegio/{idcolegio}")]
        public List<CampoColegioCLS> FiltrarCampoColegio(string idcolegio)
        {
            List<CampoColegioCLS> listaCampoColegio = new List<CampoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (idcolegio == null || idcolegio == "--- Seleccione ---")
                {
                    listaCampoColegio = (from campocolegio in baseDatos.Campocolegio
                                         join colegio in baseDatos.Colegioarbitro
                                         on campocolegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                         orderby campocolegio.Nombre
                                         where campocolegio.Habilitado == 1 && campocolegio.Idcolegioarbitro == int.Parse(idcolegio)
                                         select new CampoColegioCLS
                                         {
                                             idcampocolegio = campocolegio.Idcampocolegio,
                                             nombre = campocolegio.Nombre,
                                             ubicacion = campocolegio.Ubicacion,
                                             colegio = colegio.Nombre
                                         }).ToList();
                }
                else
                {
                    listaCampoColegio = (from campocolegio in baseDatos.Campocolegio
                                         join colegio in baseDatos.Colegioarbitro
                                         on campocolegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                         orderby campocolegio.Nombre
                                         where campocolegio.Habilitado == 1 && campocolegio.Idcolegioarbitro == int.Parse(idcolegio)
                                         select new CampoColegioCLS
                                         {
                                             idcampocolegio = campocolegio.Idcampocolegio,
                                             nombre = campocolegio.Nombre,
                                             ubicacion = campocolegio.Ubicacion,
                                             colegio = colegio.Nombre
                                         }).ToList();
                }
            }
            return listaCampoColegio;
        }



        [HttpPost]
        [Route("api/CampoColegio/GuardarDatosCampoColegio")]
        public int GuardarDatosCampoColegio([FromBody] CampoColegioCLS oCampoColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oCampoColegioCLS.idcampocolegio == 0)
                    {
                        // VER SI ESTA EN LA TABLA CAMPOCOLEGIO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Campocolegio.Where(p => (p.Nombre.Trim()).Equals(oCampoColegioCLS.nombre.Trim())
                        && p.Idcolegioarbitro == int.Parse(oCampoColegioCLS.idcolegioarbitro) &&  p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Campocolegio oCampoColegio = new Campocolegio();
                            oCampoColegio.Nombre = oCampoColegioCLS.nombre;
                            oCampoColegio.Ubicacion = oCampoColegioCLS.ubicacion == null ? "" : oCampoColegioCLS.ubicacion;
                            oCampoColegio.Idcolegioarbitro = int.Parse(oCampoColegioCLS.idcolegioarbitro);
                            oCampoColegio.Habilitado = 1;
                            baseDatos.Campocolegio.Add(oCampoColegio);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA CAMPOCOLEGIO, ESE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Campocolegio.Where(p => (p.Nombre.Trim()).Equals(oCampoColegioCLS.nombre.Trim())
                      && p.Idcampocolegio != oCampoColegioCLS.idcampocolegio && p.Idcolegioarbitro == int.Parse(oCampoColegioCLS.idcolegioarbitro) 
                      && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Campocolegio oCampoColegio = baseDatos.Campocolegio.Where(p => p.Idcampocolegio == oCampoColegioCLS.idcampocolegio).First();
                            oCampoColegio.Nombre = oCampoColegioCLS.nombre;
                            oCampoColegio.Ubicacion = oCampoColegioCLS.ubicacion == null ? "" : oCampoColegioCLS.ubicacion;
                            oCampoColegio.Idcolegioarbitro = int.Parse(oCampoColegioCLS.idcolegioarbitro);
                            oCampoColegio.Habilitado = 1;                           
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
        [Route("api/CampoColegio/RecuperarInformacionCampoColegio/{idCampoColegio}")]
        public CampoColegioCLS RecuperarInformacionCampoColegio(int idCampoColegio)
        {
            CampoColegioCLS oCampoColegioCLS = new CampoColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oCampoColegioCLS = (from campocolegio in baseDatos.Campocolegio
                                   where campocolegio.Idcampocolegio == idCampoColegio
                                   select new CampoColegioCLS
                                   {
                                       idcampocolegio = campocolegio.Idcampocolegio,
                                       nombre = campocolegio.Nombre,
                                       ubicacion = campocolegio.Ubicacion,
                                       idcolegioarbitro = campocolegio.Idcolegioarbitro.ToString()
                                   }).First();

                return oCampoColegioCLS;
            }
        }

        [HttpGet]
        [Route("api/CampoColegio/EliminarCampoColegio/{idCampoColegio}")]
        public int EliminarCampoColegio(int idCampoColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Campocolegio oCampocolegio = baseDatos.Campocolegio.Where(p => p.Idcampocolegio == idCampoColegio).First();
                    oCampocolegio.Habilitado = 0;
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
