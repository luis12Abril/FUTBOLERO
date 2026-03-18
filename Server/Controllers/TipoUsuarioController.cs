using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class TipoUsuarioController : Controller
    {
        [HttpGet]
        [Route("api/TipoUsuario/listarTipoUsuario")]
        public List<TipoUsuarioCLS> listarTipoUsuario()
        {
            List<TipoUsuarioCLS> lista = new List<TipoUsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                lista = (from tipoUsuario in baseDatos.Tipousuario
                         orderby tipoUsuario.Nombre
                         where tipoUsuario.Habilitado == 1
                         select new TipoUsuarioCLS
                         {
                             iidTipoUsuario = tipoUsuario.Idtipousuario,
                             nombre = tipoUsuario.Nombre,
                             descripcion = tipoUsuario.Descripcion
                         }).ToList();
            }
            return lista;
        }

        [HttpGet]
        [Route("api/TipoUsuario/FiltrarTipoUsuario/{dataBuscar?}")]
        public List<TipoUsuarioCLS> FiltrarTipoUsuario(string dataBuscar)
        {
            List<TipoUsuarioCLS> lista = new List<TipoUsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (dataBuscar == "" || dataBuscar == null)
                {
                    lista = (from tipoUsuario in baseDatos.Tipousuario
                             orderby tipoUsuario.Nombre
                             where tipoUsuario.Habilitado == 1
                             select new TipoUsuarioCLS
                             {
                                 iidTipoUsuario = tipoUsuario.Idtipousuario,
                                 nombre = tipoUsuario.Nombre,
                                 descripcion = tipoUsuario.Descripcion
                             }).ToList();
                }
                else
                {
                    lista = (from tipoUsuario in baseDatos.Tipousuario
                             orderby tipoUsuario.Nombre
                             where tipoUsuario.Habilitado == 1 && tipoUsuario.Nombre.Contains(dataBuscar)
                             select new TipoUsuarioCLS
                             {
                                 iidTipoUsuario = tipoUsuario.Idtipousuario,
                                 nombre = tipoUsuario.Nombre,
                                 descripcion = tipoUsuario.Descripcion
                             }).ToList();
                }

            }
            return lista;
        }


        [HttpPost]
        [Route("api/TipoUsuario/GuardarDatosTipoUsuario")]
        public int GuardarDatosTipoUsuario([FromBody] TipoUsuarioCLS oTipoUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oTipoUsuarioCLS.iidTipoUsuario == 0)            // SI ES IGUAL A 0 ES QUE ES NUEVO
                        {
                            Tipousuario oTipoUsuario = new Tipousuario();
                            oTipoUsuario.Nombre = oTipoUsuarioCLS.nombre;
                            oTipoUsuario.Descripcion = oTipoUsuarioCLS.descripcion;
                            oTipoUsuario.Habilitado = 1;
                            baseDatos.Tipousuario.Add(oTipoUsuario);
                            baseDatos.SaveChanges();
                            int idTipoUsuario = oTipoUsuario.Idtipousuario;
                            if (oTipoUsuarioCLS.listaid.Count > 0)
                            {
                                foreach (int num in oTipoUsuarioCLS.listaid)
                                {
                                    Paginatipousuario oPaginaTipoUsuario = new Paginatipousuario();
                                    oPaginaTipoUsuario.Idpagina = num;
                                    oPaginaTipoUsuario.Idtipousuario = idTipoUsuario;
                                    oPaginaTipoUsuario.Habilitado = 1;
                                    baseDatos.Paginatipousuario.Add(oPaginaTipoUsuario);
                                    baseDatos.SaveChanges();
                                }
                            }

                            transaccion.Complete();
                            rpta = 1;
                        }
                        else                                                // SINO ES QUE ES UN EDITAR
                        {
                            int iidTipo = oTipoUsuarioCLS.iidTipoUsuario;
                            Tipousuario oTipoUsuario = baseDatos.Tipousuario.Where(p => p.Idtipousuario == iidTipo).First();
                            oTipoUsuario.Nombre = oTipoUsuarioCLS.nombre;
                            oTipoUsuario.Descripcion = oTipoUsuarioCLS.descripcion;
                            baseDatos.SaveChanges();

                            // PRIMERO DESHABILITAMOS TODAS LAS PAGINAS ASOCIADAS AL TIPO USUARIO

                            List<Paginatipousuario> lista = (from paginaTipoUsuario in baseDatos.Paginatipousuario
                                                             where paginaTipoUsuario.Idtipousuario == iidTipo
                                                             select paginaTipoUsuario).ToList();

                            if (lista != null && lista.Count > 0)
                            {
                                foreach (Paginatipousuario oPaginaTipoUsuario1 in lista)
                                {
                                    oPaginaTipoUsuario1.Habilitado = 0;
                                    baseDatos.SaveChanges();
                                }
                            }

                            if (oTipoUsuarioCLS.listaid.Count > 0)
                            {
                                foreach (int num in oTipoUsuarioCLS.listaid)
                                {
                                    int nVeces = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == oTipoUsuarioCLS.iidTipoUsuario
                                    && p.Idpagina == num).Count();
                                    if (nVeces == 0)
                                    {
                                        Paginatipousuario oPaginaTipoUsuario = new Paginatipousuario();
                                        oPaginaTipoUsuario.Idpagina = num;
                                        oPaginaTipoUsuario.Idtipousuario = iidTipo;
                                        oPaginaTipoUsuario.Habilitado = 1;
                                        baseDatos.Paginatipousuario.Add(oPaginaTipoUsuario);
                                        baseDatos.SaveChanges();
                                    }
                                    else
                                    {
                                        Paginatipousuario oPaginaTipoUsuario = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == iidTipo
                                          && p.Idpagina == num).First();
                                        oPaginaTipoUsuario.Habilitado = 1;
                                        baseDatos.SaveChanges();
                                    }

                                }
                            }
                            transaccion.Complete();
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
        [Route("api/TipoUsuario/RecuperarPaginasTipoUsuario/{iidTipoUsuario}")]
        public TipoUsuarioCLS RecuperarPaginasTipoUsuario(int iidTipoUsuario)
        {
            TipoUsuarioCLS oTipoUsuarioCLS = new TipoUsuarioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oTipoUsuarioCLS = (from tipoUsuario in baseDatos.Tipousuario
                                   where tipoUsuario.Idtipousuario == iidTipoUsuario
                                   select new TipoUsuarioCLS
                                   {
                                       iidTipoUsuario = tipoUsuario.Idtipousuario,
                                       nombre = tipoUsuario.Nombre,
                                       descripcion = tipoUsuario.Descripcion
                                   }).First();

                List<int> listaIds = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == iidTipoUsuario && p.Habilitado == 1)
                    .Select(p => p.Idpagina).Cast<int>().ToList();

                oTipoUsuarioCLS.listaid = listaIds;

                return oTipoUsuarioCLS;
            }
        }


        [HttpGet]
        [Route("api/TipoUsuario/EliminarTipoUsuario/{iidTipoUsuario}")]
        public int EliminarTipoUsuario(int iidTipoUsuario)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Tipousuario oTipoUsuario = baseDatos.Tipousuario.Where(p => p.Idtipousuario == iidTipoUsuario).First();
                    oTipoUsuario.Habilitado = 0;
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



