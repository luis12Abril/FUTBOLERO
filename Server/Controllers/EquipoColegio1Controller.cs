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
    public class EquipoColegio1Controller : Controller
    {
        [HttpGet]
        [Route("api/EquipoColegio1/ListaEquipoColegio1")]
        public List<EquipoColegioCLS> ListaEquipoColegio1()
        {
            List<EquipoColegioCLS> listaEquipoColegio = new List<EquipoColegioCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaEquipoColegio = (from equipocolegio in baseDatos.Equipocolegio
                                      orderby equipocolegio.Nombre
                                      where equipocolegio.Habilitado == 1
                                      select new EquipoColegioCLS
                                      {
                                          idequipocolegio = equipocolegio.Idequipocolegio,
                                          nombre = equipocolegio.Nombre
                                      }).ToList();
            }
            return listaEquipoColegio;
        }
    }
}
