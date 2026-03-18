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
    public class ArbitroColegio1Controller : Controller
    {
        [HttpGet]
        [Route("api/ArbitroColegio1/ListarArbitroColegio1")]
        public List<ArbitroColegioCLS> ListarArbitroColegio1()
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
                                           fnacimientocadena = regfechanacimientojugador1(ArbitroColegio.Fnacimiento),
                                           pesocadena = ArbitroColegio.Peso.ToString()
                                       }).ToList();
            }
            return listaArbitroColegio;
        }

        public static string regfechanacimientojugador1(DateTime? fnacimiento)
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
    }
}
