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
    public class ProgramacionColegioController : Controller
    {
        [HttpGet]
        [Route("api/ProgramacionColegio/ListarProgramacionColegio/{idcolegio}")]
        public List<ProgramacionColegioCLS> ListarProgramacionColegio(string idcolegio)
        {
            List<ProgramacionColegioCLS> listaProgramacionColegio = new List<ProgramacionColegioCLS>();
            List<ProgramacionColegioCLS> ListaOrdenada;
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaProgramacionColegio = (from progColegio in baseDatos.Programacioncolegio                                          
                                            join arbitro in baseDatos.Arbitrocolegio
                                            on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                            join campo in baseDatos.Campocolegio
                                            on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                            join equipo01 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                            join equipo02 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                            //where progColegio.Idcolegioarbitro == int.Parse(idcolegio)
                                            where progColegio.Habilitado == 1 && progColegio.Idcolegioarbitro == int.Parse(idcolegio)
                                            orderby arbitro.Appaterno, progColegio.Fjuegocolegio descending
                                            select new ProgramacionColegioCLS
                                            {
                                                idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                campocadena = campo.Nombre,
                                                equipo01cadena = equipo01.Nombre,
                                                equipo02cadena = equipo02.Nombre,
                                                fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"              //ckbAutorizacion.Checked ? dtpAutorizacion.Value : DateTime.Now,
                                            }).ToList();
                ListaOrdenada = listaProgramacionColegio.OrderBy(o => o.nombrecompleto).ToList();
            }

            return ListaOrdenada;
        }

        [HttpGet]
        [Route("api/ProgramacionColegio/ListarProgramacionColegio1")]
        public List<ProgramacionColegioCLS> ListarProgramacionColegio1()
        {
            List<ProgramacionColegioCLS> listaProgramacionColegio = new List<ProgramacionColegioCLS>();        

            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaProgramacionColegio = (from progColegio in baseDatos.Programacioncolegio

                                               join arbitro in baseDatos.Arbitrocolegio
                                               on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                               join campo in baseDatos.Campocolegio
                                               on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                               join equipo01 in baseDatos.Equipocolegio
                                               on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                               join equipo02 in baseDatos.Equipocolegio
                                               on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                               //where progColegio.Idcolegioarbitro == int.Parse(idcolegio)
                                               where progColegio.Habilitado == 1
                                               orderby progColegio.Fjuegocolegio descending
                                               select new ProgramacionColegioCLS
                                               {
                                                   idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                   nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                   campocadena = campo.Nombre,
                                                   equipo01cadena = equipo01.Nombre,
                                                   equipo02cadena = equipo02.Nombre,
                                                   fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                   comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"              //ckbAutorizacion.Checked ? dtpAutorizacion.Value : DateTime.Now,
                                               }).ToList();               
            }

            return listaProgramacionColegio;
        }


        [HttpGet]
        [Route("api/ProgramacionColegio/ListarProgramacionColegio2/{idarbitro}")]
        public List<ProgramacionColegioCLS> ListarProgramacionColegio2(int idarbitro)
        {
            List<ProgramacionColegioCLS> listaProgramacionColegio = new List<ProgramacionColegioCLS>();
            List<ProgramacionColegioCLS> ListaOrdenada;
          
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaProgramacionColegio = (from progColegio in baseDatos.Programacioncolegio
                                           
                                            join arbitro in baseDatos.Arbitrocolegio
                                            on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                            join campo in baseDatos.Campocolegio
                                            on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                            join equipo01 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                            join equipo02 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                            //where progColegio.Idcolegioarbitro == int.Parse(idcolegio)
                                            where progColegio.Habilitado == 1 && progColegio.Idarbitrocolegio == idarbitro
                                            orderby progColegio.Fjuegocolegio descending
                                            select new ProgramacionColegioCLS
                                            {
                                                idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                campocadena = campo.Nombre,
                                                equipo01cadena = equipo01.Nombre,
                                                equipo02cadena = equipo02.Nombre,
                                                fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"              //ckbAutorizacion.Checked ? dtpAutorizacion.Value : DateTime.Now,
                                            }).ToList();
                ListaOrdenada = listaProgramacionColegio.OrderBy(o => o.nombrecompleto).ToList();
            }

            return ListaOrdenada;
        }


        [HttpGet]
        [Route("api/ProgramacionColegio/ListarProgramacionColegio123")]
        public List<ProgramacionColegio123CLS> ListarProgramacionColegio123()
        {
            List<ProgramacionColegio123CLS> listaProgramacionColegio = new List<ProgramacionColegio123CLS>();
            List<ProgramacionColegio123CLS> listaProgramacionColegio123;

            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaProgramacionColegio123 = (from progColegio in baseDatos.Programacioncolegio

                                            join arbitro in baseDatos.Arbitrocolegio
                                            on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                            join campo in baseDatos.Campocolegio
                                            on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                            join equipo01 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                            join equipo02 in baseDatos.Equipocolegio
                                            on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                            //where progColegio.Idcolegioarbitro == int.Parse(idcolegio)
                                            where progColegio.Habilitado == 1 
                                            orderby progColegio.Fjuegocolegio descending
                                            select new ProgramacionColegio123CLS
                                            {
                                                idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                campocadena = campo.Nombre,
                                                equipo01cadena = equipo01.Nombre,
                                                equipo02cadena = equipo02.Nombre,
                                                fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"              //ckbAutorizacion.Checked ? dtpAutorizacion.Value : DateTime.Now,
                                            }).ToList();
                //ListaOrdenada = listaProgramacionColegio.OrderBy(o => o.nombrecompleto).ToList();
            }

            return listaProgramacionColegio123;
        }


        //}).OrderBy(o=> o.nombrecompleto).OrderByDescending(o => o.fhorario).ToList();

        // LA CONDICION DEL WHERE ESTA ASI PORQUE VA A BUSCAR POR UN COMBO, SI FUERA POR TEXT SIN BOTON SERIA DIFRENTE
        [HttpGet]
        [Route("api/ProgramacionColegio/FiltrarProgramacionColegio/{idcolegio}")]
        public List<ProgramacionColegioCLS> FiltrarProgramacionColegio(string idcolegio)
        {
            List<ProgramacionColegioCLS> listaProgramacionColegio = new List<ProgramacionColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                if (idcolegio == null || idcolegio == "--- Seleccione ---")
                {
                    listaProgramacionColegio = (from progColegio in baseDatos.Programacioncolegio
                                                orderby progColegio.Fjuegocolegio descending
                                                join arbitro in baseDatos.Arbitrocolegio
                                                on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                                join campo in baseDatos.Campocolegio
                                                on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                                join equipo01 in baseDatos.Equipocolegio
                                                on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                                join equipo02 in baseDatos.Equipocolegio
                                                on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                                where progColegio.Habilitado == 1 && progColegio.Idcolegioarbitro == int.Parse(idcolegio)                                                
                                                select new ProgramacionColegioCLS
                                                {
                                                    idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                    nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                    campocadena = campo.Nombre,
                                                    equipo01cadena = equipo01.Nombre,
                                                    equipo02cadena = equipo02.Nombre,
                                                    fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                    comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"
                                                }).ToList();
                }
                else
                {
                    listaProgramacionColegio = (from progColegio in baseDatos.Programacioncolegio
                                                orderby progColegio.Fjuegocolegio descending
                                                join arbitro in baseDatos.Arbitrocolegio
                                                on progColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                                join campo in baseDatos.Campocolegio
                                                on progColegio.Idcampocolegio equals campo.Idcampocolegio
                                                join equipo01 in baseDatos.Equipocolegio
                                                on progColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                                join equipo02 in baseDatos.Equipocolegio
                                                on progColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                                where progColegio.Habilitado == 1 && progColegio.Idcolegioarbitro == int.Parse(idcolegio)                                                
                                                select new ProgramacionColegioCLS
                                                {
                                                    idprogramacioncolegio = progColegio.Idprogramacioncolegio,
                                                    nombrecompleto = arbitro.Nombre + " " + arbitro.Appaterno,
                                                    campocadena = campo.Nombre,
                                                    equipo01cadena = equipo01.Nombre,
                                                    equipo02cadena = equipo02.Nombre,
                                                    fhorariocadena = progColegio.Fjuegocolegio.Value.ToString(),
                                                    comentariojuego = progColegio.Comentario.Length > 4 ? "SI" : "NO"
                                                }).ToList();
                }
            }
            return listaProgramacionColegio;
        }

                
        [HttpPost]
        [Route("api/ProgramacionColegio/GuardarDatosProgramacionColegio")]
        public int GuardarDatosProgramacionColegio([FromBody] ProgramacionColegioCLS oProgramacionColegioCLS)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oProgramacionColegioCLS.idprogramacioncolegio == 0)     // CUANDO ES UN JUEGO NUEVO
                        {
                            Programacioncolegio oProgramacionColegio = new Programacioncolegio();

                            oProgramacionColegio.Idarbitrocolegio = int.Parse(oProgramacionColegioCLS.idarbitrocolegio);
                            oProgramacionColegio.Idcolegioarbitro = int.Parse(oProgramacionColegioCLS.idcolegio);
                            oProgramacionColegio.Idequipocolegio01 = int.Parse(oProgramacionColegioCLS.idequipo01);
                            oProgramacionColegio.Idequipocolegio02 = int.Parse(oProgramacionColegioCLS.idequipo02);
                            oProgramacionColegio.Idcampocolegio = int.Parse(oProgramacionColegioCLS.idcampocolegio);
                            oProgramacionColegio.Fjuegocolegio = oProgramacionColegioCLS.fhorario;
                            oProgramacionColegio.Comentario = oProgramacionColegioCLS.comentariocolegio;
                            oProgramacionColegio.Habilitado = 1;

                            baseDatos.Programacioncolegio.Add(oProgramacionColegio);
                            baseDatos.SaveChanges();

                            rpta = 1;
                        }
                        else
                        {
                            Programacioncolegio oProgramacionColegio = baseDatos.Programacioncolegio.Where(p => p.Idprogramacioncolegio == oProgramacionColegioCLS.idprogramacioncolegio).First();
                            oProgramacionColegio.Idarbitrocolegio = int.Parse(oProgramacionColegioCLS.idarbitrocolegio);
                            oProgramacionColegio.Idcolegioarbitro = int.Parse(oProgramacionColegioCLS.idcolegio);
                            oProgramacionColegio.Idequipocolegio01 = int.Parse(oProgramacionColegioCLS.idequipo01);
                            oProgramacionColegio.Idequipocolegio02 = int.Parse(oProgramacionColegioCLS.idequipo02);
                            oProgramacionColegio.Idcampocolegio = int.Parse(oProgramacionColegioCLS.idcampocolegio);
                            oProgramacionColegio.Fjuegocolegio = oProgramacionColegioCLS.fhorario;
                            oProgramacionColegio.Comentario = oProgramacionColegioCLS.comentariocolegio;

                            baseDatos.SaveChanges();
                            rpta = 1;
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
        [Route("api/ProgramacionColegio/RecuperarInformacionProgramacionColegio/{idProgramacionColegio}")]
        public ProgramacionColegioCLS RecuperarInformacionProgramacionColegio(int idProgramacionColegio)
        {
            ProgramacionColegioCLS oProgramacionColegioCLS = new ProgramacionColegioCLS();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                oProgramacionColegioCLS = (from ProgColegio in baseDatos.Programacioncolegio
                                           join arbitro in baseDatos.Arbitrocolegio
                                           on ProgColegio.Idarbitrocolegio equals arbitro.Idarbitrocolegio
                                           join colegio in baseDatos.Colegioarbitro
                                           on ProgColegio.Idcolegioarbitro equals colegio.Idcolegioarbitro
                                           join equipo01 in baseDatos.Equipocolegio
                                           on ProgColegio.Idequipocolegio01 equals equipo01.Idequipocolegio
                                           join equipo02 in baseDatos.Equipocolegio
                                           on ProgColegio.Idequipocolegio02 equals equipo02.Idequipocolegio
                                           join campo in baseDatos.Campocolegio
                                           on ProgColegio.Idcampocolegio equals campo.Idcampocolegio
                                           where ProgColegio.Idprogramacioncolegio == idProgramacionColegio
                                           select new ProgramacionColegioCLS
                                           {
                                               idprogramacioncolegio = ProgColegio.Idprogramacioncolegio,
                                               idcolegio = ProgColegio.Idcolegioarbitro.ToString(),
                                               nombrecolegio = colegio.Nombre,
                                               idarbitrocolegio = ProgColegio.Idarbitrocolegio.ToString(),
                                               nombrecompleto = arbitro.Nombre+" "+arbitro.Appaterno+" "+arbitro.Apmaterno,
                                               idequipo01 = ProgColegio.Idequipocolegio01.ToString(),
                                               equipo01cadena = equipo01.Nombre,
                                               idequipo02 = ProgColegio.Idequipocolegio02.ToString(),
                                               equipo02cadena = equipo02.Nombre,
                                               idcampocolegio = ProgColegio.Idcampocolegio.ToString(),
                                               campocadena = campo.Nombre,
                                               fhorario = (DateTime)ProgColegio.Fjuegocolegio,
                                               comentariocolegio = ProgColegio.Comentario

                                           }).First();
                return oProgramacionColegioCLS;
            }
        }


        [HttpGet]
        [Route("api/ProgramacionColegio/EliminarProgramacionColegio/{idProgramacionColegio}")]
        public int EliminarProgramacionColegio(int idProgramacionColegio)
        {
            int rpta = 0;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    Programacioncolegio oProgramacioncolegio = baseDatos.Programacioncolegio.Where(p => p.Idprogramacioncolegio == idProgramacionColegio).First();

                    oProgramacioncolegio.Habilitado = 0;
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
