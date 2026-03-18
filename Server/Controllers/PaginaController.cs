using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class PaginaController : Controller
    {
        [HttpGet]
        [Route("api/pagina/listarPagina")]
        public List<PaginaCLS> listarPagina()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaPagina = (from pagina in baseDatos.Pagina
                               orderby pagina.Ordenmenu
                               where pagina.Habilitado == 1
                               select new PaginaCLS
                               {
                                   idpagina = pagina.Idpagina,
                                   mensaje = pagina.Mensaje,
                                   accion = pagina.Accion,
                                   ordenmenu = (int)pagina.Ordenmenu,
                                   nombrevisible = pagina.Visible == 1 ? "Si" : "No"
                               }).ToList();
            }
            return listaPagina;
        }

        [HttpGet]
        [Route("api/Pagina/filtrarPagina/{mensaje?}")]
        public List<PaginaCLS> filtrarPagina(string mensaje)
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaPagina = (from pagina in baseDatos.Pagina
                                   orderby pagina.Ordenmenu
                                   where pagina.Habilitado == 1
                                   select new PaginaCLS
                                   {
                                       idpagina = pagina.Idpagina,
                                       mensaje = pagina.Mensaje,
                                       accion = pagina.Accion,
                                       ordenmenu = (int)pagina.Ordenmenu,
                                       nombrevisible = pagina.Visible == 1 ? "Si" : "No"

                                   }).ToList();
                }
                else
                {
                    listaPagina = (from pagina in baseDatos.Pagina
                                   orderby pagina.Ordenmenu
                                   where pagina.Habilitado == 1
                                   && pagina.Mensaje.Contains(mensaje)
                                   select new PaginaCLS
                                   {
                                       idpagina = pagina.Idpagina,
                                       mensaje = pagina.Mensaje,
                                       accion = pagina.Accion,
                                       ordenmenu = (int)pagina.Ordenmenu,
                                       nombrevisible = pagina.Visible == 1 ? "Si" : "No"
                                   }).ToList();
                }
            }
            return listaPagina;
        }

        [HttpPost]
        [Route("api/Pagina/GuardarDatosPagina")]
        public int GuardarDatosPagina([FromBody] PaginaCLS oPaginaCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    if (oPaginaCLS.idpagina == 0)
                    {
                        Pagina oPagina = new Pagina();
                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Ordenmenu = oPaginaCLS.ordenmenu;
                        oPagina.Visible = Convert.ToInt32(oPaginaCLS.visible);
                        oPagina.Habilitado = 1;
                        baseDatos.Pagina.Add(oPagina);
                        baseDatos.SaveChanges();
                        rpta = 1;
                    }
                    else
                    {
                        Pagina oPagina = baseDatos.Pagina.Where(p => p.Idpagina == oPaginaCLS.idpagina).First();
                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Ordenmenu = oPaginaCLS.ordenmenu;
                        oPagina.Visible = Convert.ToInt32(oPaginaCLS.visible);
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
        [Route("api/pagina/eliminarPagina/{idpagina}")]
        public int eliminarPagina(int idpagina)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Pagina oPagina = baseDatos.Pagina.Where(p => p.Idpagina == idpagina).First();
                    oPagina.Habilitado = 0;
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
        [Route("api/Pagina/RecuperarInformacionPagina/{idPagina}")]
        public PaginaCLS RecuperarInformacionPagina(int idPagina)
        {
            PaginaCLS oPaginaCLS = new PaginaCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oPaginaCLS = (from pagina in baseDatos.Pagina
                              where pagina.Idpagina == idPagina
                              select new PaginaCLS
                              {
                                  idpagina = pagina.Idpagina,
                                  mensaje = pagina.Mensaje,
                                  accion = pagina.Accion,
                                  ordenmenu = (int)pagina.Ordenmenu,
                                  visible = Convert.ToBoolean(pagina.Visible)
                              }).First();

                return oPaginaCLS;
            }
        }

        // OBTENER EL ID DE LA PAGINA (ESTE METODO SE VA A LLAMAR DE TODOS LOS FORMULARIOS)
        [HttpGet]
        [Route("api/Pagina/RecuperarIdPagina/{nombrePagina}")]
        public int RecuperarIdPagagina(string nombrePagina)
        {
            int idPagina = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    idPagina = (from pagina in baseDatos.Pagina
                                where pagina.Accion == "/" + nombrePagina
                                select pagina.Idpagina).First();
                }

            }
            catch (Exception ex)
            {
                idPagina = 0;
            }

            return idPagina;
        }


    }
}
