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
    public class ArbitroColegioController : Controller
    {
        [HttpGet]
        [Route("api/ArbitroColegio/ListarArbitroColegio/{idcolegioarbitros}")]
        public List<ArbitroColegioCLS> ListarArbitroColegio(string idcolegioarbitros)
        {
            List<ArbitroColegioCLS> listaArbitroColegio = new List<ArbitroColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaArbitroColegio = (from ArbitroColegio in baseDatos.Arbitrocolegio
                                       orderby ArbitroColegio.Nombre                                         
                                       where ArbitroColegio.Idcolegioarbitro == int.Parse(idcolegioarbitros) && ArbitroColegio.Habilitado == 1
                                       select new ArbitroColegioCLS
                                       {
                                           idarbitrocolegio = ArbitroColegio.Idarbitrocolegio,
                                           nombrecompleto = ArbitroColegio.Nombre + " " + ArbitroColegio.Appaterno + " " + ArbitroColegio.Apmaterno,                                           
                                           fnacimientocadena = regfechanacimientojugador(ArbitroColegio.Fnacimiento),
                                           pesocadena = ArbitroColegio.Peso.ToString()
                                       }).ToList();
            }
            return listaArbitroColegio;
        }

        [HttpGet]
        [Route("api/ArbitroColegio/ListarArbitroColegio01")]
        public List<ArbitroColegioCLS> ListarArbitroColegio01()
        {
            List<ArbitroColegioCLS> listaArbitroColegio = new List<ArbitroColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaArbitroColegio = (from ArbitroColegio in baseDatos.Arbitrocolegio
                                       orderby ArbitroColegio.Nombre
                                       where ArbitroColegio.Habilitado == 1
                                       select new ArbitroColegioCLS
                                       {
                                           idarbitrocolegio = ArbitroColegio.Idarbitrocolegio,
                                           nombrecompleto = ArbitroColegio.Nombre + " " + ArbitroColegio.Appaterno + " " + ArbitroColegio.Apmaterno,
                                           fnacimientocadena = regfechanacimientojugador(ArbitroColegio.Fnacimiento),
                                           pesocadena = ArbitroColegio.Peso.ToString()
                                       }).ToList();
            }
            return listaArbitroColegio;
        }
               

        [HttpGet]
        [Route("api/ArbitroColegio/FiltrarArbitroColegio/{idcolegio}")]
        public List<ArbitroColegioCLS> FiltrarArbitroColegio(string idcolegio)
        {
            List<ArbitroColegioCLS> listaArbitroColegio = new List<ArbitroColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (idcolegio == null || idcolegio == "--- Seleccione ---")
                {
                    listaArbitroColegio = (from ArbitroColegio in baseDatos.Arbitrocolegio
                                           orderby ArbitroColegio.Nombre
                                           where ArbitroColegio.Idcolegioarbitro == int.Parse(idcolegio) && ArbitroColegio.Habilitado == 1
                                           select new ArbitroColegioCLS
                                           {
                                               idarbitrocolegio = ArbitroColegio.Idarbitrocolegio,
                                               nombrecompleto = ArbitroColegio.Nombre + " " + ArbitroColegio.Appaterno + " " + ArbitroColegio.Apmaterno,
                                               fnacimientocadena = regfechanacimientojugador(ArbitroColegio.Fnacimiento),
                                               pesocadena = ArbitroColegio.Peso.ToString()
                                           }).ToList();
                }
                else
                {
                    listaArbitroColegio = (from ArbitroColegio in baseDatos.Arbitrocolegio
                                           orderby ArbitroColegio.Nombre
                                           where ArbitroColegio.Idcolegioarbitro == int.Parse(idcolegio) && ArbitroColegio.Habilitado == 1
                                           select new ArbitroColegioCLS
                                           {
                                               idarbitrocolegio = ArbitroColegio.Idarbitrocolegio,
                                               nombrecompleto = ArbitroColegio.Nombre + " " + ArbitroColegio.Appaterno + " " + ArbitroColegio.Apmaterno,
                                               fnacimientocadena = regfechanacimientojugador(ArbitroColegio.Fnacimiento),
                                               pesocadena = ArbitroColegio.Peso.ToString()
                                           }).ToList();
                }
            }
            return listaArbitroColegio;
        }



        [HttpGet]
        [Route("api/ArbitroColegio/RecuperarInformacionArbitroColegio/{idArbitroColegio}")]
        public ArbitroColegioCLS RecuperarInformacionArbitroColegio(int idArbitroColegio)
        {
            ArbitroColegioCLS oArbitroColegioCLS = new ArbitroColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oArbitroColegioCLS = (from ArbitroColegio in baseDatos.Arbitrocolegio
                                      where ArbitroColegio.Idarbitrocolegio == idArbitroColegio
                                      select new ArbitroColegioCLS
                                      {
                                          idarbitrocolegio = ArbitroColegio.Idarbitrocolegio,
                                          idcolegioarbitro = ArbitroColegio.Idcolegioarbitro.ToString(),
                                          nombre = ArbitroColegio.Nombre,
                                          appaterno = ArbitroColegio.Appaterno,
                                          apmaterno = ArbitroColegio.Apmaterno,
                                          fnacimiento = (DateTime)ArbitroColegio.Fnacimiento,
                                          peso = (int)ArbitroColegio.Peso,
                                          nomusuario = ArbitroColegio.Nomusuario,
                                          nomusuariocopia = ArbitroColegio.Nomusuario,
                                          codigo = "12345678*",                     //ArbitroColegio.Codigoactual,
                                          fotoarbitro = ArbitroColegio.Fotoarbitro
                                      }).First();              
                return oArbitroColegioCLS;
            }
        }



        [HttpPost]
        [Route("api/ArbitroColegio/GuardarDatosArbitroColegio")]
        public int GuardarDatosArbitroColegio([FromBody] ArbitroColegioCLS oArbitroColegioCLS)
        {
            int rpta = 0;
            int nveces = 0;
            int yaexiste = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        string nombrecompleto;
                        string apellidomaterno = (oArbitroColegioCLS.apmaterno == null ? " " : oArbitroColegioCLS.apmaterno);
                        nombrecompleto = oArbitroColegioCLS.nombre.Trim() + " " + oArbitroColegioCLS.appaterno.Trim() + " " + apellidomaterno.Trim();

                        if (oArbitroColegioCLS.idarbitrocolegio == 0)
                        {                       
                            
                            // VER SI ESTA EN LA TABLA ARBITROCOLEGIO, ESE NOMBRE DE CAMPO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Arbitrocolegio.Where(p => (p.Nombre.Trim() + " " +  p.Appaterno.Trim() + " " + p.Apmaterno.Trim()).Equals(nombrecompleto)
                            && p.Idcolegioarbitro == int.Parse(oArbitroColegioCLS.idcolegioarbitro) && p.Habilitado == 1).Count();

                            if (nveces > 0)
                            {
                                rpta = 3;
                            }
                            else
                            {                               
                                yaexiste = baseDatos.Usuario.Where(p => p.Nombre == oArbitroColegioCLS.nomusuario && p.Habilitado == 1).Count();
                                if (yaexiste > 0)
                                {
                                    rpta = 2;           // EL USUARIO YA EXISTE
                                }
                                else
                                {
                                    string clave = oArbitroColegioCLS.codigo;
                                    byte[] dataCifrada;
                                    using (SHA256 sha = SHA256.Create())
                                    {
                                        byte[] buffer = Encoding.Default.GetBytes(clave);
                                        dataCifrada = sha.ComputeHash(buffer);
                                    }
                                    string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                                    Arbitrocolegio oArbitroColegio = new Arbitrocolegio();
                                    oArbitroColegio.Idcolegioarbitro = int.Parse(oArbitroColegioCLS.idcolegioarbitro);
                                    oArbitroColegio.Nombre = oArbitroColegioCLS.nombre;
                                    oArbitroColegio.Appaterno = oArbitroColegioCLS.appaterno;
                                    oArbitroColegio.Apmaterno = (oArbitroColegioCLS.apmaterno == null ? " " : oArbitroColegioCLS.apmaterno); //oArbitroColegioCLS.apmaterno;
                                    oArbitroColegio.Fnacimiento = oArbitroColegioCLS.fnacimiento;
                                    oArbitroColegio.Peso = oArbitroColegioCLS.peso;
                                    oArbitroColegio.Nomusuario = oArbitroColegioCLS.nomusuario;
                                    oArbitroColegio.Codigoactual = oArbitroColegioCLS.codigo;                                   
                                    oArbitroColegio.Fotoarbitro = oArbitroColegioCLS.fotoarbitro;
                                    oArbitroColegio.Habilitado = 1;

                                    baseDatos.Arbitrocolegio.Add(oArbitroColegio);
                                    baseDatos.SaveChanges();
                                    int idarbitro = oArbitroColegio.Idarbitrocolegio;

                                    Usuario oUsuario = new Usuario();
                                    oUsuario.Idpersona = 1;                 // ESTO LO VOY A QUITAR
                                    oUsuario.Contraseña = dataCifradaCadena;
                                    oUsuario.Idtipousuario = 4;             // EL NUMERO CUATRO ES UN TIPO DE USUARIO ARBITRO
                                    oUsuario.Nombre = oArbitroColegioCLS.nomusuario;
                                    oUsuario.Idarbitrocolegio = idarbitro;

                                    oUsuario.Visitas = 0;
                                    oUsuario.Visitascel = 0;

                                    oUsuario.Token = "";       // NO LO VOY A USAR

                                    oUsuario.Habilitado = 1;

                                    baseDatos.Usuario.Add(oUsuario);
                                    baseDatos.SaveChanges();

                                    rpta = 1;
                                }                                
                            }
                        }
                        else
                        {

                            // VER SI ESTA EN LA TABLA ARBITRO, ESE NOMBRE DEL ARBITRO, EN ESE TORNEO Y QUE ESTE HABILITADO
                            nveces = baseDatos.Arbitrocolegio.Where(p => (p.Nombre.Trim() + " " + p.Appaterno.Trim() + " " + p.Apmaterno.Trim()).Equals(nombrecompleto)
                            && p.Idcolegioarbitro == int.Parse(oArbitroColegioCLS.idcolegioarbitro) && p.Idarbitrocolegio != oArbitroColegioCLS.idarbitrocolegio && p.Habilitado == 1).Count();

                            if (nveces > 0)
                            {
                                rpta = 3;       // EL USUARIO YA EXISTE
                            }
                            else
                            {

                                string clave = oArbitroColegioCLS.codigo;
                                byte[] dataCifrada;
                                using (SHA256 sha = SHA256.Create())
                                {
                                    byte[] buffer = Encoding.Default.GetBytes(clave);
                                    dataCifrada = sha.ComputeHash(buffer);
                                }
                                string dataCifradaCadena = BitConverter.ToString(dataCifrada).Replace("-", "");

                                Arbitrocolegio oArbitroColegio = baseDatos.Arbitrocolegio.Where(p => p.Idarbitrocolegio == oArbitroColegioCLS.idarbitrocolegio).First();
                                oArbitroColegio.Idcolegioarbitro = int.Parse(oArbitroColegioCLS.idcolegioarbitro);
                                oArbitroColegio.Nombre = oArbitroColegioCLS.nombre;
                                oArbitroColegio.Appaterno = oArbitroColegioCLS.appaterno;
                                oArbitroColegio.Apmaterno = (oArbitroColegioCLS.apmaterno == null ? " " : oArbitroColegioCLS.apmaterno);
                                oArbitroColegio.Fnacimiento = oArbitroColegioCLS.fnacimiento;
                                oArbitroColegio.Peso = oArbitroColegioCLS.peso;
                                oArbitroColegio.Nomusuario = oArbitroColegioCLS.nomusuario;
                                oArbitroColegio.Fotoarbitro = oArbitroColegioCLS.fotoarbitro;
                                if (oArbitroColegioCLS.codigo != "12345678*")
                                {
                                    oArbitroColegio.Codigoactual = oArbitroColegioCLS.codigo;
                                }

                                baseDatos.SaveChanges();

                                if (oArbitroColegioCLS.codigo != "12345678*")
                                {
                                    // SI ES 12345678* QUIERE DECIR QUE NO LE MOVIERON Y LA CLAVE SE QUEDA IGUAL, SI ES DIFERENTE ENTONCES LA CAMBIARON Y SE GRABA
                                    Usuario oUsuario = baseDatos.Usuario.Where(p => p.Nombre == oArbitroColegioCLS.nomusuariocopia).First();
                                    oUsuario.Nombre = oArbitroColegioCLS.nomusuario;
                                    oUsuario.Contraseña = dataCifradaCadena;
                                    baseDatos.SaveChanges();
                                }

                                rpta = 1;
                                                                
                            }
                        }
                        transaccion.Complete();                       
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
        [Route("api/ArbitroColegio/EliminarArbitroColegio/{idArbitroColegio}")]
        public int EliminarArbitroColegio(int idArbitroColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Arbitrocolegio oArbitrocolegio = baseDatos.Arbitrocolegio.Where(p => p.Idarbitrocolegio == idArbitroColegio).First();

                    oArbitrocolegio.Habilitado = 0;
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


        public static string regfechanacimientojugador(DateTime? fnacimiento)
        {
            string rfecha = "";

            rfecha = fnacimiento.Value.Day.ToString() + " de ";

            switch (fnacimiento.Value.Month)
            {
                case 1:
                    rfecha = rfecha + "enero";
                    break;
                case 2:
                    rfecha = rfecha + "febrero";
                    break;
                case 3:
                    rfecha = rfecha + "marzo";
                    break;
                case 4:
                    rfecha = rfecha + "abril";
                    break;
                case 5:
                    rfecha = rfecha + "mayo";
                    break;
                case 6:
                    rfecha = rfecha + "junio";
                    break;
                case 7:
                    rfecha = rfecha + "julio";
                    break;
                case 8:
                    rfecha = rfecha + "agosto";
                    break;
                case 9:
                    rfecha = rfecha + "septiembre";
                    break;
                case 10:
                    rfecha = rfecha + "octubre";
                    break;
                case 11:
                    rfecha = rfecha + "noviembre";
                    break;
                case 12:
                    rfecha = rfecha + "diciembre";
                    break;
                default:
                    break;
            }

            if (fnacimiento.Value.Year >= 2000)
            {
                rfecha = rfecha + " del " + fnacimiento.Value.Year.ToString();
            }
            else
            {
                rfecha = rfecha + " de " + fnacimiento.Value.Year.ToString();
            }

            return rfecha;
        }

        [HttpGet]
        [Route("api/Arbitro/RegresaUsuario/{nombreusuario}")]
        public int RegresaUsuario(string nombreusuario)
        {
            int rpta = 0;

            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    rpta = (int)baseDatos.Arbitrocolegio.Where(p => p.Nomusuario == nombreusuario && p.Habilitado == 1).First().Idarbitrocolegio;
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
