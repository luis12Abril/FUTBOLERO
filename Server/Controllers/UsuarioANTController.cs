using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using FUTBOLERO.Server.Models;
using FUTBOLERO.Shared;


namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class UsuarioController : Controller
    {
        [HttpGet]
        [Route("api/Usuario/ListarUsuario")]
        public List<UsuarioCLS> ListarUsuario()
        {
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaUsuario = (from usuario in baseDatos.Usuario
                                join tipousuario in baseDatos.Tipousuario
                                on usuario.Idtipousuario equals tipousuario.Idtipousuario
                                orderby usuario.Idtipousuario, usuario.Visitas descending, usuario.Nombre
                                where usuario.Habilitado == 1
                                select new UsuarioCLS
                                {
                                    idusuario = usuario.Idusuario,
                                    nombre = usuario.Nombre,
                                    tipousuariocadena = tipousuario.Nombre,
                                    idtipousuario = usuario.Idtipousuario.ToString(),
                                    visitas = (int)usuario.Visitas

                                    //contraseña = usuario.Contraseña
                                    //contraseña = RegresaContraseña(usuario.Contraseña)
                                }).ToList();
            }
            return listaUsuario;
        }


        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/Usuario/FiltrarUsuario/{p_idtipousuario?}")]
        public List<UsuarioCLS> FiltrarUsuario(string p_idtipousuario)
        {
            List<UsuarioCLS> listaUsuario = new List<UsuarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (p_idtipousuario == null || p_idtipousuario == "--- Seleccione ---")
                {
                    listaUsuario = (from usuario in baseDatos.Usuario
                                    join tipousuario in baseDatos.Tipousuario
                                    on usuario.Idtipousuario equals tipousuario.Idtipousuario
                                    orderby usuario.Nombre, usuario.Visitas descending, usuario.Nombre
                                    where usuario.Habilitado == 1
                                    select new UsuarioCLS
                                    {
                                        idusuario = usuario.Idusuario,
                                        nombre = usuario.Nombre,
                                        tipousuariocadena = tipousuario.Nombre,
                                        visitas = (int)usuario.Visitas
                                    }).ToList();
                }
                else
                {
                    listaUsuario = (from usuario in baseDatos.Usuario
                                    join tipousuario in baseDatos.Tipousuario
                                    on usuario.Idtipousuario equals tipousuario.Idtipousuario
                                    orderby usuario.Nombre, usuario.Visitas descending, usuario.Nombre
                                    where usuario.Habilitado == 1 && usuario.Idtipousuario == int.Parse(p_idtipousuario)
                                    select new UsuarioCLS
                                    {
                                        idusuario = usuario.Idusuario,
                                        nombre = usuario.Nombre,
                                        tipousuariocadena = tipousuario.Nombre,
                                        visitas = (int)usuario.Visitas
                                    }).ToList();
                }
            }
            return listaUsuario;
        }


        [HttpPost]
        [Route("api/Usuario/LoginIidUsuario")]
        public int LoginIidUsuario([FromBody] UsuarioCLS oUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    string clave = oUsuarioCLS.contraseña;
                    SHA256Managed sha = new SHA256Managed();
                    byte[] buffer = Encoding.Default.GetBytes(clave);
                    byte[] dataCifrada = sha.ComputeHash(buffer);
                    string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                    int nveces = baseDatos.Usuario.Where(p => p.Nombre == oUsuarioCLS.nombre && p.Contraseña == dataCifradaCadena && p.Habilitado == 1).Count();
                    if (nveces == 0)
                    {
                        rpta = 0;
                    }
                    else
                    {
                        rpta = baseDatos.Usuario.Where(p => p.Nombre == oUsuarioCLS.nombre && p.Contraseña == dataCifradaCadena && p.Habilitado == 1).First().Idusuario;
                    }
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }


        [HttpPost]
        [Route("api/Usuario/GuardarDatosUsuario")]
        public int GuardarDatosUsuario([FromBody] UsuarioCLS oUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        // SI ES 0, ES USUARIO NUEVO
                        if (oUsuarioCLS.idusuario == 0)
                        {
                            int yaexiste = 0;
                            yaexiste = baseDatos.Usuario.Where(p => p.Nombre == oUsuarioCLS.nombre && p.Habilitado == 1).Count();
                            if (yaexiste > 0)
                            {
                                rpta = 2;           // EL USUARIO YA EXISTE
                            }
                            else
                            {
                                string clave = oUsuarioCLS.contraseña;
                                SHA256Managed sha = new SHA256Managed();
                                byte[] buffer = Encoding.Default.GetBytes(clave);
                                byte[] dataCifrada = sha.ComputeHash(buffer);
                                string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                                Usuario oUsuario = new Usuario();
                                oUsuario.Idpersona = 1;                 // ESTO LO VOY A QUITAR
                                oUsuario.Contraseña = dataCifradaCadena;
                                oUsuario.Idtipousuario = int.Parse(oUsuarioCLS.idtipousuario);
                                oUsuario.Nombre = oUsuarioCLS.nombre;
                                oUsuario.Idarbitrocolegio = 0;
                                oUsuario.Visitas = 0;

                                oUsuario.Token = "";       // NO LO VOY A USAR

                                oUsuario.Habilitado = 1;

                                baseDatos.Usuario.Add(oUsuario);
                                baseDatos.SaveChanges();

                                int idusuarionuevo = oUsuario.Idusuario;
                                int idtipousuario = (int) oUsuario.Idtipousuario;

                                // CUANDO ES NUEVO NO TIENE NINGUN TORNEO ASOCIADO

                                if (oUsuarioCLS.listaidtorneo.Count > 0)
                                {
                                    foreach (int num in oUsuarioCLS.listaidtorneo)
                                    {
                                        int nVeces = baseDatos.Usuariotorneo.Where(p => p.Idusuario == idusuarionuevo
                                        && p.Idtorneo == num).Count();
                                        if (nVeces == 0)
                                        {
                                            Usuariotorneo oUsuariotorneo = new Usuariotorneo();
                                            oUsuariotorneo.Idusuario = idusuarionuevo;
                                            oUsuariotorneo.Idtorneo = num;
                                            oUsuariotorneo.Habilitado = 1;

                                            baseDatos.Usuariotorneo.Add(oUsuariotorneo);
                                            baseDatos.SaveChanges();
                                        }
                                        else
                                        {
                                            Usuariotorneo oUsuariotorneo = baseDatos.Usuariotorneo.Where(p => p.Idusuario == oUsuarioCLS.idusuario
                                            && p.Idtorneo == num).First();
                                            oUsuariotorneo.Habilitado = 1;

                                            baseDatos.SaveChanges();
                                        }
                                    }
                                }

                                // SI ES UN TIPO USUARIO PRESIDENTE
                                if (idtipousuario == 5)
                                {

                                }

                                transaccion.Complete();
                                rpta = 1;
                            }
                        }
                        else
                        {
                            // SI ES UN EDIATAR
                            Usuario oUsuario = baseDatos.Usuario.Where(p => p.Idusuario == oUsuarioCLS.idusuario).First();

                            string clave = oUsuarioCLS.contraseña;
                            if (clave != "12345678*")
                            {
                                // SI ENTRA AQUI ES PORQUE LA CLAVE SE MODIFICO
                                SHA256Managed sha = new SHA256Managed();
                                byte[] buffer = Encoding.Default.GetBytes(clave);
                                byte[] dataCifrada = sha.ComputeHash(buffer);
                                string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                                oUsuario.Contraseña = dataCifradaCadena;
                            }

                            oUsuario.Idtipousuario = int.Parse(oUsuarioCLS.idtipousuario);
                            baseDatos.SaveChanges();

                            // PRIMERO DESHABILITAMOS TODAS LOS TORNEOS ASOCIADOS AL USUARIO

                            List<Usuariotorneo> lista = (from usuariotorneo in baseDatos.Usuariotorneo
                                                         where usuariotorneo.Idusuario == oUsuarioCLS.idusuario
                                                         select usuariotorneo).ToList();

                            if (lista != null && lista.Count > 0)
                            {
                                foreach (Usuariotorneo oUsuariotorneo in lista)
                                {
                                    oUsuariotorneo.Habilitado = 0;
                                    baseDatos.SaveChanges();
                                }
                            }

                            if (oUsuarioCLS.listaidtorneo.Count > 0)
                            {
                                foreach (int num in oUsuarioCLS.listaidtorneo)
                                {
                                    int nVeces = baseDatos.Usuariotorneo.Where(p => p.Idusuario == oUsuarioCLS.idusuario
                                    && p.Idtorneo == num).Count();
                                    if (nVeces == 0)
                                    {
                                        Usuariotorneo oUsuariotorneo = new Usuariotorneo();
                                        oUsuariotorneo.Idusuario = oUsuarioCLS.idusuario;
                                        oUsuariotorneo.Idtorneo = num;
                                        oUsuariotorneo.Habilitado = 1;

                                        baseDatos.Usuariotorneo.Add(oUsuariotorneo);
                                        baseDatos.SaveChanges();
                                    }
                                    else
                                    {
                                        Usuariotorneo oUsuariotorneo = baseDatos.Usuariotorneo.Where(p => p.Idusuario == oUsuarioCLS.idusuario
                                        && p.Idtorneo == num).First();
                                        oUsuariotorneo.Habilitado = 1;

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

        [HttpPost]
        [Route("api/Usuario/GuardarDatosNuevoUsuario")]
        public int GuardarDatosNuevoUsuario([FromBody] NuevoUsuarioCLS oNuevoUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {

                    int yaexiste = 0;
                    yaexiste = baseDatos.Usuario.Where(p => p.Nombre.Trim().ToUpper() == oNuevoUsuarioCLS.nombreusuario.Trim().ToUpper()).Count();
                    if (yaexiste > 0)
                    {
                        rpta = 2;           // EL USUARIO YA EXISTE
                    }
                    else
                    {

                        string clave = oNuevoUsuarioCLS.contra;
                        SHA256Managed sha = new SHA256Managed();
                        byte[] buffer = Encoding.Default.GetBytes(clave);
                        byte[] dataCifrada = sha.ComputeHash(buffer);
                        string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                        Usuario oUsuario = new Usuario();
                        oUsuario.Idpersona = 1;                 // ESTO LO VOY A QUITAR
                        oUsuario.Contraseña = dataCifradaCadena;
                        oUsuario.Idtipousuario = 2;             // ES UN USUARIO INVITADO, PARA LOS QUE SE REGISTRAN DE LA PAGINA PRINCIPAL
                        oUsuario.Nombre = oNuevoUsuarioCLS.nombreusuario;
                        oUsuario.Visitas = 1;
                        oUsuario.Idarbitrocolegio = 0;
                        // oUsuario.Visitascel = 0;     ES PARA PONER LAS VISITAS DEL CELULAR EN 0, FALTA VOLVER A ACTUALIZAR LA FUTBOLEANDOContex.cs CON LA INSTRUCCION NUGET DE CONSOLA


                        oUsuario.Token = "";       // NO LO VOY A USAR

                        oUsuario.Habilitado = 1;

                        baseDatos.Usuario.Add(oUsuario);
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
        [Route("api/Usuario/EliminarUsuario/{idUsuario}")]
        public int EliminarUsuario(int idUsuario)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Usuario oUsuario = baseDatos.Usuario.Where(p => p.Idusuario == idUsuario).First();
                    oUsuario.Habilitado = 0;
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
        [Route("api/Usuario/RecuperarInformacionUsuario/{idusuario}")]
        public UsuarioCLS RecuperarInformacionUsuario(int idusuario)
        {
            UsuarioCLS oUsuarioCLS = new UsuarioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oUsuarioCLS = (from usuario in baseDatos.Usuario
                               where usuario.Idusuario == idusuario
                               select new UsuarioCLS
                               {
                                   idusuario = usuario.Idusuario,
                                   nombre = usuario.Nombre,
                                   contraseña = "12345678*",
                                   idtipousuario = usuario.Idtipousuario.ToString()
                               }).First();

                List<int> ListaIdTorneo = baseDatos.Usuariotorneo.Where(p => p.Idusuario == idusuario
                && p.Habilitado == 1).Select(p => p.Idtorneo).Cast<int>().ToList();

                oUsuarioCLS.listaidtorneo = ListaIdTorneo;

                return oUsuarioCLS;
            }
        }


        [HttpGet]
        [Route("api/Usuario/ListarPaginaPorTipoUsuario/{iidusuario}/{idtorneoseleccionado}")]
        public List<PaginaCLS> ListarPaginaPorTipoUsuario(int iidusuario, string idtorneoseleccionado)
        {
            int nveces = 0;
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {

                    // TENGO QUE SACAR EL IIDTIPOUSUARIO
                    int iidTipoUsuario = (int)baseDatos.Usuario.Where(p => p.Idusuario == iidusuario).First().Idtipousuario;

                    // TENGO QUE VER SI ESTE iidusuario PUEDE ADMINISTRAR EL TORNEO SELECCIONADO, SI ES UN ADMINISTRADOR, PERO NO ESTA ASIGNADO A idtorneoseleccionado
                    // PASARA A SER UN USUARIO INVITADO
                    nveces = baseDatos.Usuariotorneo.Where(p => p.Idusuario == iidusuario && p.Idtorneo == int.Parse(idtorneoseleccionado) && p.Habilitado == 1).Count();

                    // SI nveces ES IGUAL A 0 INDEPENDIENTEMENTE DEL TIPO DE USUARIO QUE SEA PASARA A SER UN TIPO DE USUARIO INVITADO PORQUE NO ESTA AUTORIZADO
                    // A ADMINISTRAR ESE TORNEO   1 = ADMINISTRADOR TOTA   2 = INVITADO   3 = ADMINISTRADOR DE TORNEOS      4 = ARBITRO     5 = PRESIDENTE ARBITRO

                    //if (iidTipoUsuario != 5)         // EL USUARIO 5 ES UN PRESIDENTE O ADMINIOSTRADOR DEL COLEGIO DE ARBITROS
                    //{
                    //    if (nveces == 0)
                    //    {
                    //        iidTipoUsuario = 2;     // EL USUARIO 2 ES UN INVITADO O ADMINISTRADOR DEL COLEGIO DE ARBITROS
                    //    }
                    //}


                    if (nveces == 0)
                    {
                        if (iidTipoUsuario == 3)
                        {
                            iidTipoUsuario = 2;
                        }
                    }

                        listaPagina = (from paginaTipoUsuario in baseDatos.Paginatipousuario
                                   join pagina in baseDatos.Pagina on paginaTipoUsuario.Idpagina equals pagina.Idpagina
                                   orderby pagina.Ordenmenu
                                   where paginaTipoUsuario.Idtipousuario == iidTipoUsuario && pagina.Habilitado == 1
                                   && paginaTipoUsuario.Habilitado == 1 && pagina.Visible == 1
                                   select new PaginaCLS
                                   {
                                       idpagina = pagina.Idpagina,
                                       mensaje = pagina.Mensaje,
                                       accion = pagina.Accion + "/" + pagina.Idpagina
                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                listaPagina = null;
            }

            return listaPagina;
        }


        [HttpGet]
        [Route("api/Usuario/VisitasUsuario/{idUsuario}")]
        public int VisitasUsuario(int idUsuario)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Usuario oUsuario = baseDatos.Usuario.Where(p => p.Idusuario == idUsuario).First();
                    oUsuario.Visitas = oUsuario.Visitas + 1;
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
        [Route("api/Usuario/TotalVisitasUsuarios")]
        public int TotalVisitasUsuarios()
        {
            int visitastotales = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    visitastotales =(int) baseDatos.Usuario.Where(p => p.Habilitado == 1).Sum(x=> x.Visitas);
                }
            }
            catch (Exception ex)
            {
                visitastotales = 0;
            }

            return visitastotales;
        }

        [HttpGet]
        [Route("api/Usuario/RegresaIdTipoUsuario/{idusuario}")]
        public int RegresaIdColegio(int idusuario)
        {
            int rpta = 0;
                      
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (int)baseDatos.Usuario.Where(p => p.Idusuario == idusuario && p.Habilitado == 1).First().Idtipousuario;                    
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        [HttpGet]
        [Route("api/Usuario/RegresaUsuario2/{idusuario}")]
        public string RegresaUsuario2(int idusuario)
        {
            string rpta = "";

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (string)baseDatos.Usuario.Where(p => p.Idusuario == idusuario && p.Habilitado == 1).First().Nombre;
                }
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return "ARB001VB";
        }


        [HttpGet]
        [Route("api/Usuario/RegresaUsuario/{idusuario}")]
        public string RegresaUsuario(int idusuario)
        {
            string rpta = "";

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (string)baseDatos.Usuario.Where(p => p.Idusuario == idusuario && p.Habilitado == 1).First().Nombre;
                }
            }
            catch (Exception ex)
            {
                rpta = "";
            }

            return rpta;
        }



    }
}
