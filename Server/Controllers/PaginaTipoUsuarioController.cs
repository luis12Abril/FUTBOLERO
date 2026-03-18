using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class PaginaTipoUsuarioController : Controller
    {
        [HttpGet]
        [Route("api/PaginaTipoUsuario/listarPaginaTipoUsuario")]
        public List<PaginaTipoUsuarioCLS> listarPaginaTipoUsuario()
        {
            List<PaginaTipoUsuarioCLS> lista = new List<PaginaTipoUsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                lista = (from paginatipousuario in baseDatos.Paginatipousuario
                         join pagina in baseDatos.Pagina
                         on paginatipousuario.Idpagina equals pagina.Idpagina
                         join tipousuario in baseDatos.Tipousuario
                         on paginatipousuario.Idtipousuario equals tipousuario.Idtipousuario
                         orderby tipousuario.Nombre, pagina.Mensaje
                         where paginatipousuario.Habilitado == 1
                         select new PaginaTipoUsuarioCLS
                         {
                             idpaginatipousuario = paginatipousuario.Idpaginatipousuario,
                             nombrepagina = pagina.Mensaje,
                             nombretipousuario = tipousuario.Nombre
                         }).ToList();
            }
            return lista;

        }

        [HttpGet]
        [Route("api/PaginaTipoUsuario/FiltrarPaginaTipoUsuario/{iidTipoUsuario?}")]
        public List<PaginaTipoUsuarioCLS> FiltroPaginaTipoUsuario(string iidTipoUsuario)
        {
            List<PaginaTipoUsuarioCLS> lista = new List<PaginaTipoUsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (iidTipoUsuario == null || iidTipoUsuario == "--- Seleccione ---")
                {
                    lista = (from paginatipousuario in baseDatos.Paginatipousuario
                             join pagina in baseDatos.Pagina
                             on paginatipousuario.Idpagina equals pagina.Idpagina
                             join tipousuario in baseDatos.Tipousuario
                             on paginatipousuario.Idtipousuario equals tipousuario.Idtipousuario
                             orderby tipousuario.Nombre, pagina.Mensaje
                             where paginatipousuario.Habilitado == 1
                             select new PaginaTipoUsuarioCLS
                             {
                                 idpaginatipousuario = paginatipousuario.Idpaginatipousuario,
                                 nombrepagina = pagina.Mensaje,
                                 nombretipousuario = tipousuario.Nombre
                             }).ToList();
                }
                else
                {
                    lista = (from paginatipousuario in baseDatos.Paginatipousuario
                             join pagina in baseDatos.Pagina
                             on paginatipousuario.Idpagina equals pagina.Idpagina
                             join tipousuario in baseDatos.Tipousuario
                             on paginatipousuario.Idtipousuario equals tipousuario.Idtipousuario
                             orderby tipousuario.Nombre, pagina.Mensaje
                             where paginatipousuario.Habilitado == 1 && paginatipousuario.Idtipousuario == int.Parse(iidTipoUsuario)
                             select new PaginaTipoUsuarioCLS
                             {
                                 idpaginatipousuario = paginatipousuario.Idpaginatipousuario,
                                 nombrepagina = pagina.Mensaje,
                                 nombretipousuario = tipousuario.Nombre
                             }).ToList();
                }

            }
            return lista;

        }


        [HttpPost]
        [Route("api/PaginaTipoUsuario/GuardarDatosPaginaTipoUsuario")]
        public int GuardarDatosPaginaTipoUsuario([FromBody] PaginaTipoUsuarioCLS oPaginaTipoUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {

                        if (oPaginaTipoUsuarioCLS.idpaginatipousuario != 0)                                                // SINO ES QUE ES UN EDITAR
                        {


                            // PRIMERO DESHABILITAMOS TODAS LAS PAGINAS ASOCIADAS AL TIPO USUARIO

                            List<Paginatipousuarioboton> lista = (from paginaTipoUsoButton in baseDatos.Paginatipousuarioboton
                                                                  where paginaTipoUsoButton.Idpaginatipousuario == oPaginaTipoUsuarioCLS.idpaginatipousuario
                                                                  select paginaTipoUsoButton).ToList();

                            if (lista != null && lista.Count > 0)
                            {
                                foreach (Paginatipousuarioboton oPaginaTipoUsuButton in lista)
                                {
                                    oPaginaTipoUsuButton.Habilitado = 0;
                                    baseDatos.SaveChanges();
                                }
                            }

                            if (oPaginaTipoUsuarioCLS.idboton.Count > 0)
                            {
                                foreach (int num in oPaginaTipoUsuarioCLS.idboton)
                                {
                                    int nVeces = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario == oPaginaTipoUsuarioCLS.idpaginatipousuario
                                    && p.Idboton == num).Count();
                                    if (nVeces == 0)
                                    {
                                        Paginatipousuarioboton oPaginaTipoUsuButton = new Paginatipousuarioboton();
                                        oPaginaTipoUsuButton.Idpaginatipousuario = oPaginaTipoUsuarioCLS.idpaginatipousuario;
                                        oPaginaTipoUsuButton.Idboton = num;
                                        oPaginaTipoUsuButton.Habilitado = 1;
                                        baseDatos.Paginatipousuarioboton.Add(oPaginaTipoUsuButton);
                                        baseDatos.SaveChanges();
                                    }
                                    else
                                    {
                                        Paginatipousuarioboton oPaginaTipoUsuario = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario ==
                                        oPaginaTipoUsuarioCLS.idpaginatipousuario
                                          && p.Idboton == num).First();
                                        oPaginaTipoUsuario.Habilitado = 1;
                                        baseDatos.SaveChanges();
                                    }

                                }
                            }
                        }
                        transaccion.Complete();
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
        [Route("api/PaginaTipoUsuario/ListarBotones/{iidUsuario}/{iidPagina}")]
        public List<int> listarBotones(int iidUsuario, int iidPagina)
        {
            List<int> listaBotones = new List<int>();
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    int iidTipoUsuario = (int)baseDatos.Usuario.Where(p => p.Idusuario == iidUsuario).First().Idtipousuario;
                    int iidPaginaTipoUsuario = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == iidTipoUsuario
                    && p.Idpagina == iidPagina && p.Habilitado == 1).First().Idpaginatipousuario;

                    List<Paginatipousuarioboton> lista = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario == iidPaginaTipoUsuario
                   && p.Habilitado == 1).ToList();
                    if (lista.Count > 0)
                    {
                        listaBotones = new List<int>();
                        listaBotones = lista.Select(p => p.Idboton).Cast<int>().ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                listaBotones = new List<int>();
            }


            return listaBotones;
        }


        [HttpGet]
        [Route("api/PaginaTipoUsuario/ListarBotones2/{iidusuario}/{iidPagina}/{idtorneoseleccionado}")]
        public List<int> listarBotones2(int iidusuario, int iidPagina, string idtorneoseleccionado)
        {
            int nveces = 0;
            List<int> listaBotones = new List<int>();
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    int iidTipoUsuario = (int)baseDatos.Usuario.Where(p => p.Idusuario == iidusuario).First().Idtipousuario;
                    int copiatipousuario = iidTipoUsuario;


                    // TENGO QUE VER SI ESTE iidusuario PUEDE ADMINISTRAR EL TORNEO SELECCIONADO, SI ES UN ADMINISTRADOR, PERO NO ESTA ASIGNADO A idtorneoseleccionado
                    // PASARA A SER UN USUARIO INVITADO
                    nveces = baseDatos.Usuariotorneo.Where(p => p.Idusuario == iidusuario && p.Idtorneo == int.Parse(idtorneoseleccionado) && p.Habilitado == 1).Count();

                    // SI nveces ES IGUAL A 0 INDEPENDIENTEMENTE DEL TIPO DE USUARIO QUE SEA PASARA A SER UN TIPO DE USUARIO INVITADO PORQUE NO ESTA AUTORIZADO
                    // A ADMINISTRAR ESE TORNEO   1 = ADMINISTRADOR TOTA   2 = INVITADO   3 = ADMINISTRADOR DE TORNEOS
                    // 6 ADMINISTRADOR EQUIPO Y SOLO VA A ENTRAR A LA PAGINA JUGADORES PARA DARLOS DE ALTA

                    

                    if (iidTipoUsuario != 5)         // EL USUARIO 5 ES UN PRESIDENTE O ADMINIOSTRADOR DEL COLEGIO DE ARBITROS 
                    {
                        if (nveces == 0)
                        {
                            iidTipoUsuario = 2;     // EL USUARIO 2 ES UN INVITADO O ADMINIOSTRADOR DEL COLEGIO DE ARBITROS
                        }
                    }

                    // ES PARA QUE EL USUARIO TIPO 6 ADMINISTRADOR EQUIPO SIGA COMO 6
                    if (copiatipousuario == 6)
                    {
                        iidTipoUsuario = 6;
                    }

                    int iidPaginaTipoUsuario = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == iidTipoUsuario
                    && p.Idpagina == iidPagina && p.Habilitado == 1).First().Idpaginatipousuario;


                    List<Paginatipousuarioboton> lista = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario == iidPaginaTipoUsuario
                   && p.Habilitado == 1).ToList();
                    if (lista.Count > 0)
                    {
                        listaBotones = new List<int>();
                        listaBotones = lista.Select(p => p.Idboton).Cast<int>().ToList();
                    }


                }

            }
            catch (Exception ex)
            {
                listaBotones = new List<int>();
            }


            return listaBotones;
        }


        [HttpGet]
        [Route("api/PaginaTipoUsuario/ListarBotones3/{idtipousuario}/{iidPagina}")]
        public List<int> listarBotones3(int idtipousuario, int iidPagina)
        {
            List<int> listaBotones = new List<int>();
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    int iidPaginaTipoUsuario = baseDatos.Paginatipousuario.Where(p => p.Idtipousuario == idtipousuario
                    && p.Idpagina == iidPagina && p.Habilitado == 1).First().Idpaginatipousuario;


                    List<Paginatipousuarioboton> lista = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario == iidPaginaTipoUsuario
                   && p.Habilitado == 1).ToList();
                    if (lista.Count > 0)
                    {
                        listaBotones = new List<int>();
                        listaBotones = lista.Select(p => p.Idboton).Cast<int>().ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                listaBotones = new List<int>();
            }
            return listaBotones;
        }


        [HttpGet]
        [Route("api/PaginaTipoUsuario/puedeadministrar/{iidusuario}/{idtorneoseleccionado}")]
        public int puedeadministrar(int iidusuario, string idtorneoseleccionado)
        {
            int idtipousuario = 2;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    // PUEDE SER 1, 2 Ó 3
                    idtipousuario = (int)baseDatos.Usuario.Where(p => p.Idusuario == iidusuario).First().Idtipousuario;

                    // TENGO QUE VER SI ESTE iidusuario PUEDE ADMINISTRAR EL TORNEO SELECCIONADO, SI ES UN ADMINISTRADOR, PERO NO ESTA ASIGNADO A idtorneoseleccionado
                    // PASARA A SER UN USUARIO INVITADO
                    nveces = baseDatos.Usuariotorneo.Where(p => p.Idusuario == iidusuario && p.Idtorneo == int.Parse(idtorneoseleccionado) && p.Habilitado == 1).Count();

                    // SI nveces ES IGUAL A 0 INDEPENDIENTEMENTE DEL TIPO DE USUARIO QUE SEA PASARA A SER UN TIPO DE USUARIO INVITADO PORQUE NO ESTA AUTORIZADO
                    // A ADMINISTRAR ESE TORNEO   1 = ADMINISTRADOR TOTA   2 = INVITADO   3 = ADMINISTRADOR DE TORNEOS
                    if (nveces == 0)
                    {
                        idtipousuario = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                idtipousuario = 2;
            }

            return idtipousuario;
        }



        [HttpGet]
        [Route("api/PaginaTipoUsuario/RecuperarBotones/{iidTipoUsuarioPagina}")]
        public PaginaTipoUsuarioCLS RecuperarBotones(int iidTipoUsuarioPagina)
        {

            using (var baseDatos = new FUTBOLEANDOContext())
            {
                PaginaTipoUsuarioCLS oPaginaTipoUsuarioCLS = new PaginaTipoUsuarioCLS();

                oPaginaTipoUsuarioCLS.idpaginatipousuario = iidTipoUsuarioPagina;

                List<int> ListaIdButton = baseDatos.Paginatipousuarioboton.Where(p => p.Idpaginatipousuario == iidTipoUsuarioPagina
                && p.Habilitado == 1).Select(p => p.Idboton).Cast<int>().ToList();

                oPaginaTipoUsuarioCLS.idboton = ListaIdButton;
                return oPaginaTipoUsuarioCLS;

            }
        }


        [HttpGet]
        [Route("api/PaginaTipoUsuario/ListarBotones")]
        public List<BotonesCLS> ListaBotones()
        {
            List<BotonesCLS> lista = new List<BotonesCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                lista = (from botones in baseDatos.Boton
                         where botones.Habilitado == 1
                         select new BotonesCLS
                         {
                             idboton = botones.Idboton,
                             nombre = botones.Nombre
                         }).ToList();
            }
            return lista;
        }



    }
}

