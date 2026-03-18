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
    public class ComentarioController : Controller
    {

        [HttpGet]
        [Route("api/Comentario/RecuperarTodosComentario/{p_idjuego}")]

        public List<ComentarioCLS> RecuperarTodosComentario(int p_idjuego)
        {
            List<ComentarioCLS> oComentarioCLS = new List<ComentarioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {

                oComentarioCLS = (from comentario in baseDatos.Comentario
                                  join juego in baseDatos.Juego
                                  on comentario.Idjuego equals juego.Idjuego
                                  join usuario in baseDatos.Usuario
                                  on comentario.Idusuario equals usuario.Idusuario
                                  orderby comentario.Fechacomentario descending
                                  where comentario.Idjuego == p_idjuego && comentario.Habilitado == 1
                                  select new ComentarioCLS
                                  {
                                      idcomentario = comentario.Idcomentario,
                                      comentario = comentario.Comentario1,
                                      usuario = usuario.Nombre,
                                      fechacomentariocadena = comentario.Fechacomentario.Value.ToLongDateString() + " -- " +
                                      comentario.Fechacomentario.Value.AddHours(-7).ToShortTimeString()     // LE ESTOY RESTANDO 7 HORAS POR QUE ES LAS QUE TIENE DE MAS
                                       //DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local)
                                  }).ToList();
            }
            return oComentarioCLS;
        }   

        [HttpPost]
        [Route("api/Comentario/GuardarComentarioJuegoInvitado")]
        public int GuardarComentarioJuegoInvitado([FromBody] JuegoInvitadoCLS oJuegoInvitadoCLS)
        {
            int rpta = 0;
            int nveces = 0;
            bool comentariovacio = false;
            try
            {
                using (var baseDatos = new FUTBOLEANDOContext())
                {
                    nveces = baseDatos.Comentario.Where(p => p.Idjuego == oJuegoInvitadoCLS.idjuego).Count();
                    if (oJuegoInvitadoCLS.comentario.Trim() == string.Empty)
                    {
                        comentariovacio = true;
                        rpta = 3;
                    }

                    if (nveces > 50)
                    {
                        rpta = 2;
                    }
                    else if (!comentariovacio)      // SI NO ESTA VACIO GRABA
                    {
                        Comentario oComentario = new Comentario();
                        oComentario.Idjuego = oJuegoInvitadoCLS.idjuego;
                        oComentario.Comentario1 = oJuegoInvitadoCLS.comentario;
                        oComentario.Idusuario = oJuegoInvitadoCLS.idusuario;
                        oComentario.Fechacomentario = DateTime.Now;   //DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local); // DateTime.UtcNow();   // DateTime.Now();
                        oComentario.Habilitado = 1;
                        baseDatos.Comentario.Add(oComentario);
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


    }
}
