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
    public class EquipoColegioController : Controller
    {
        [HttpGet]
        [Route("api/EquipoColegio/ListaEquipoColegio/{idcolegioarbitros}")]
        public List<EquipoColegioCLS> ListaEquipoColegio(string idcolegioarbitros)
        {
            List<EquipoColegioCLS> listaEquipoColegio = new List<EquipoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipoColegio = (from equipocolegio in baseDatos.Equipocolegio                                     
                                      orderby equipocolegio.Nombre
                                      where equipocolegio.Idcolegioarbitro == int.Parse(idcolegioarbitros) && equipocolegio.Habilitado == 1
                                      select new EquipoColegioCLS
                                      {
                                          idequipocolegio = equipocolegio.Idequipocolegio,
                                          nombre = equipocolegio.Nombre
                                      }).ToList();
            }
            return listaEquipoColegio;
        }

        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/EquipoColegio/FiltrarEquipoColegio/{idcolegio}")]
        public List<EquipoColegioCLS> FiltrarEquipoColegio(string idcolegio)
        {
            List<EquipoColegioCLS> listaEquipo = new List<EquipoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (idcolegio == null || idcolegio == "--- Seleccione ---")
                {
                    listaEquipo = (from equipocolegio in baseDatos.Equipocolegio
                                      orderby equipocolegio.Nombre
                                      where equipocolegio.Habilitado == 1
                                      select new EquipoColegioCLS
                                      {
                                          idequipocolegio = equipocolegio.Idequipocolegio,
                                          nombre = equipocolegio.Nombre
                                      }).ToList();
                }
                else
                {
                    listaEquipo = (from equipocolegio in baseDatos.Equipocolegio
                                   orderby equipocolegio.Nombre
                                   where equipocolegio.Habilitado == 1 && equipocolegio.Idcolegioarbitro == int.Parse(idcolegio)
                                   select new EquipoColegioCLS
                                   {
                                       idequipocolegio = equipocolegio.Idequipocolegio,
                                       nombre = equipocolegio.Nombre
                                   }).ToList();
                }
            }
            return listaEquipo;
        }



        [HttpPost]
        [Route("api/EquipoColegio/GuardarDatosEquipoColegio")]
        public int GuardarDatosEquipoColegio([FromBody] EquipoColegioCLS oEquipoColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oEquipoColegioCLS.idequipocolegio == 0)
                    {
                        // VER SI ESTA EN LA TABLA EQUIPOCOLEGIO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Equipocolegio.Where(p => (p.Nombre.Trim()).Equals(oEquipoColegioCLS.nombre.Trim())
                        && p.Idcolegioarbitro == int.Parse(oEquipoColegioCLS.idcolegio) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Equipocolegio oEquipoColegio = new Equipocolegio();
                            oEquipoColegio.Nombre = oEquipoColegioCLS.nombre;
                            oEquipoColegio.Idcolegioarbitro = int.Parse(oEquipoColegioCLS.idcolegio);
                            oEquipoColegio.Habilitado = 1;
                            baseDatos.Equipocolegio.Add(oEquipoColegio);
                            baseDatos.SaveChanges();
                            rpta = 1;
                        }
                    }
                    else
                    {
                        // VER SI ESTA EN LA TABLA JUGADOR, ESE NOMBRE COMPLETO DEL JUGADOR, EN ESE TORNEO Y QUE ESTE HABILITADO
                        nveces = baseDatos.Equipocolegio.Where(p => (p.Nombre.Trim()).Equals(oEquipoColegioCLS.nombre.Trim())
                        && p.Idequipocolegio != oEquipoColegioCLS.idequipocolegio
                        && p.Idcolegioarbitro == int.Parse(oEquipoColegioCLS.idcolegio) && p.Habilitado == 1).Count();
                        if (nveces > 0)
                        {
                            rpta = 3;
                        }
                        else
                        {
                            Equipocolegio oEquipoColegio = baseDatos.Equipocolegio.Where(p => p.Idequipocolegio == oEquipoColegioCLS.idequipocolegio).First();
                            oEquipoColegio.Nombre = oEquipoColegioCLS.nombre;
                            oEquipoColegio.Idcolegioarbitro = int.Parse(oEquipoColegioCLS.idcolegio);
                            oEquipoColegio.Habilitado = 1;
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
        [Route("api/EquipoColegio/RecuperarInformacionEquipoColegio/{idEquipoColegio}")]
        public EquipoColegioCLS RecuperarInformacionEquipoColegio(int idEquipoColegio)
        {
            EquipoColegioCLS oEquipoColegioCLS = new EquipoColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oEquipoColegioCLS = (from equipocolegio in baseDatos.Equipocolegio
                                     where equipocolegio.Idequipocolegio == idEquipoColegio
                                     select new EquipoColegioCLS
                                     {
                                         idequipocolegio = equipocolegio.Idequipocolegio,
                                         nombre = equipocolegio.Nombre,
                                         idcolegio = equipocolegio.Idcolegioarbitro.ToString()
                                     }).First();

                return oEquipoColegioCLS;
            }
        }


        [HttpGet]
        [Route("api/EquipoColegio/EliminarEquipoColegio/{idEquipoColegio}")]
        public int EliminarEquipoColegio(int idEquipoColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Equipocolegio oEquipoColegio = baseDatos.Equipocolegio.Where(p => p.Idequipocolegio == idEquipoColegio).First();
                    oEquipoColegio.Habilitado = 0;
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
