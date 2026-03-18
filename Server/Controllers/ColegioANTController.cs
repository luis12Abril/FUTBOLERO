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
//using FUTBOLEANDO.Server.Clases;

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class ColegioController : Controller
    {
        [HttpGet]
        [Route("api/Colegio/ListarColegio")]
        public List<ColegioCLS> ListarColegio()
        {
            List<ColegioCLS> listaColegio = new List<ColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                //orderby colegio.Nombre
                listaColegio = (from colegio in baseDatos.Colegioarbitro                                
                                where colegio.Habilitado == 1
                                select new ColegioCLS
                                {
                                    idcolegio = colegio.Idcolegioarbitro,
                                    nombre = colegio.Nombre
                                }).ToList();
            }
            return listaColegio;
        }


        // LO USO EN LA LISTA DE ARBITROS
        [HttpGet]
        [Route("api/Colegio/ListarColegio2/{idcolegio}")]
        public List<ColegioCLS> ListarColegio2(int idcolegio)
        {
            List<ColegioCLS> listaColegio = new List<ColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                //orderby colegio.Nombre
                listaColegio = (from colegio in baseDatos.Colegioarbitro
                                where colegio.Idpresidente == idcolegio && colegio.Habilitado == 1
                                select new ColegioCLS
                                {
                                    idcolegio = colegio.Idcolegioarbitro,
                                    nombre = colegio.Nombre
                                }).ToList();
            }
            return listaColegio;
        }

        // LO USO EN EN EL EDITAR PROGRAMACIONCOLEGIO
        [HttpGet]
        [Route("api/Colegio/ListarColegio3/{idcolegio}")]
        public List<ColegioCLS> ListarColegio3(int idcolegio)
        {
            List<ColegioCLS> listaColegio = new List<ColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                //orderby colegio.Nombre
                listaColegio = (from colegio in baseDatos.Colegioarbitro
                                where colegio.Idcolegioarbitro == idcolegio && colegio.Habilitado == 1
                                select new ColegioCLS
                                {
                                    idcolegio = colegio.Idcolegioarbitro,
                                    nombre = colegio.Nombre
                                }).ToList();
            }
            return listaColegio;
        }


        [HttpGet]
        [Route("api/Colegio/FiltrarColegio/{mensaje?}/{idtorneoseleccionado}")]
        public List<ColegioCLS> FiltrarColegio(string mensaje, string idtorneoseleccionado)
        {
            List<ColegioCLS> listaColegio = new List<ColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (mensaje == null || mensaje == "")
                {
                    listaColegio = (from colegio in baseDatos.Colegioarbitro
                                  orderby colegio.Nombre
                                  where colegio.Habilitado == 1 
                                  select new ColegioCLS
                                  {
                                      idcolegio = colegio.Idcolegioarbitro,
                                      nombre = colegio.Nombre
                                  }).ToList();
                }
                else
                {
                    listaColegio = (from colegio in baseDatos.Colegioarbitro
                                    orderby colegio.Nombre
                                    where colegio.Habilitado == 1 && colegio.Nombre.Contains(mensaje)
                                    select new ColegioCLS
                                    {
                                        idcolegio = colegio.Idcolegioarbitro,
                                        nombre = colegio.Nombre
                                    }).ToList();
                }
            }
            return listaColegio;
        }


        [HttpPost]
        [Route("api/Colegio/GuardarDatosColegio")]
        public int GuardarDatosColegio([FromBody] ColegioCLS oColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oColegioCLS.idcolegio == 0)
                        {
                            // VER SI ESTA EN LA TABLA COLEGIO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Colegioarbitro.Where(p => p.Nombre.Trim().Equals(oColegioCLS.nombre) && p.Habilitado == 1).Count();
                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {
                                Colegioarbitro oColegio = new Colegioarbitro();
                                oColegio.Nombre = oColegioCLS.nombre;
                                oColegio.Idpresidente = int.Parse(oColegioCLS.idpresidente);
                                oColegio.Habilitado = 1;
                                baseDatos.Colegioarbitro.Add(oColegio);
                                baseDatos.SaveChanges();
                                int idcolegio = oColegio.Idcolegioarbitro;
                                string usuario = baseDatos.Usuario.Where(p => p.Idusuario == int.Parse(oColegioCLS.idpresidente)).First().Nombre;                               

                                Usuario oUsuario = baseDatos.Usuario.Where(p => p.Nombre == usuario).First();
                                oUsuario.Idarbitrocolegio = idcolegio;
                                baseDatos.SaveChanges();
                                rpta = 1;
                            }                                                 

                        }
                        else
                        {
                            // VER SI ESTA EN LA TABLA COLEGIO, ESE NOMBRE DE CAMPO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Colegioarbitro.Where(p => p.Nombre.Trim().Equals(oColegioCLS.nombre)
                            && p.Idcolegioarbitro != oColegioCLS.idcolegio && p.Habilitado == 1).Count();

                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {
                                Colegioarbitro oColegio = baseDatos.Colegioarbitro.Where(p => p.Idcolegioarbitro == oColegioCLS.idcolegio).First();
                                oColegio.Nombre = oColegioCLS.nombre;
                                oColegio.Idpresidente = int.Parse(oColegioCLS.idpresidente);
                                oColegio.Habilitado = 1;
                                baseDatos.SaveChanges();
                                int idcolegio = oColegioCLS.idcolegio;
                                string usuario = baseDatos.Usuario.Where(p => p.Idusuario == int.Parse(oColegioCLS.idpresidente)).First().Nombre;                               

                                Usuario oUsuario = baseDatos.Usuario.Where(p => p.Nombre == usuario).First();
                                oUsuario.Idarbitrocolegio = idcolegio;
                                baseDatos.SaveChanges();
                                rpta = 1;
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
        [Route("api/Colegio/RecuperarInformacionColegio/{idcolegio}")]
        public ColegioCLS RecuperarInformacionColegio(int idcolegio)
        {
            ColegioCLS oColegioCLS = new ColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oColegioCLS = (from colegio in baseDatos.Colegioarbitro
                             where colegio.Idcolegioarbitro == idcolegio
                             select new ColegioCLS
                             {
                                 idcolegio = colegio.Idcolegioarbitro,
                                 nombre = colegio.Nombre,
                                 idpresidente = colegio.Idpresidente.ToString()
                             }).First();

                return oColegioCLS;
            }
        }


        [HttpGet]
        [Route("api/Colegio/EliminarColegio/{idcolegio}")]
        public int EliminarColegio(int idcolegio)
        {
            int rpta = 0;
           
            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Colegioarbitro oColegio = baseDatos.Colegioarbitro.Where(p => p.Idcolegioarbitro == idcolegio).First();
                    oColegio.Habilitado = 0;
                    baseDatos.SaveChanges();
                    rpta = 1;


                    // ** ESTA PARTE ESTA PENDIENTE HASTA QUE SE CREEN LAS OTRAS TABLAS PARA PODER VALIDAR LA ELIMINACION
                    //Colegioarbitro oColegio = baseDatos.Colegioarbitro.Where(p => p.Idcolegioarbitro == idcolegio).First();
                    //if (oColegio.Nombre.Trim().Equals("PENDIENTE"))
                    //{
                    //    rpta = 3;
                    //}
                    //else
                    //{
                    //    nveces = baseDatos.Juego.Where(p => p.Idcampo == idcampo && p.Habilitado == 1).Count();
                    //    if (nveces > 0)
                    //    {
                    //        rpta = 2;
                    //    }
                    //    else
                    //    {
                    //        oCampo.Habilitado = 0;
                    //        baseDatos.SaveChanges();
                    //        rpta = 1;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        [HttpGet]
        [Route("api/Colegio/RegresaIdColegio/{idusuario}")]
        public int RegresaIdColegio(int idusuario)
        {
            int rpta = 0;

            int nveces = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    nveces = baseDatos.Colegioarbitro.Where(p => p.Idpresidente== idusuario && p.Habilitado == 1).Count();
                    if(nveces == 0)
                    {
                        rpta = 0;
                    }
                    else
                    {
                        rpta = (int)baseDatos.Colegioarbitro.Where(p => p.Idpresidente == idusuario && p.Habilitado == 1).First().Idcolegioarbitro; 
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
        [Route("api/Colegio/ListarPresidentes")]
        public List<PresidenteCLS> ListarPresidentes()
        {
            List<PresidenteCLS> ListaPresidente = new List<PresidenteCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                // orderby colegio.Nombre
                // EL IDTIPOUSUARIO 5 ES EL PRESIDENTE DE ARBITROS
                ListaPresidente = (from presidente in baseDatos.Usuario
                                where presidente.Habilitado == 1 && presidente.Idtipousuario == 5
                                select new PresidenteCLS
                                {
                                    idpresidente = presidente.Idusuario,
                                    usuario = presidente.Nombre
                                }).ToList();
            }
            return ListaPresidente;
        }



    }
}
