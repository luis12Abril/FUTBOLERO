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
using System.IO;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
//using FUTBOLEANDO.Server.Clases;        No LO VOY A OCUPAR PORQUE NO VOY A MANDAR CORREOS

namespace FUTBOLERO.Server.Controllers
{
    [ApiController]
    public class PosicionesController : Controller
    {
        private List<PosicionesCLS> ObtenerPosiciones(string idtorneoseleccionado)
        {
            List<PosicionesCLS> listaPosiciones = new List<PosicionesCLS>();
            using (var baseDatos = new FUTBOLEANDOContext())
            {
                listaPosiciones = (from posiciones in baseDatos.Equipo
                                   orderby (posiciones.Puntos + posiciones.Puntosextras) descending,
                                   posiciones.Jugados,
                                   posiciones.Difgoles descending,
                                   posiciones.Golesafavor descending,
                                   posiciones.Nombre
                                   where posiciones.Habilitado == 1 && posiciones.Idtorneo == int.Parse(idtorneoseleccionado) && !posiciones.Nombre.Contains("_SIN EQUIPO")
                                   select new PosicionesCLS
                                   {
                                       nombre = posiciones.Nombre,
                                       jugados = (int)posiciones.Jugados,
                                       ganados = (int)posiciones.Ganados,
                                       empatados = (int)posiciones.Empatados,
                                       perdidos = (int)posiciones.Perdidos,
                                       empatadosganados = (int)posiciones.Empatadosganados,
                                       golesafavor = (int)posiciones.Golesafavor,
                                       golesencontra = (int)posiciones.Golesencontra,
                                       diferenciagoles = (int)posiciones.Difgoles,
                                       puntos = (int)posiciones.Puntos + (int)posiciones.Puntosextras,
                                       torneo = posiciones.Torneo,
                                       idtorneo = (int)posiciones.Idtorneo,
                                       puntosextras = (int)posiciones.Puntosextras
                                   }).ToList();
            }

            return listaPosiciones;
        }


        [HttpGet]
        [Route("api/Posiciones/ListarPosiciones/{idtorneoseleccionado}")]
        public List<PosicionesCLS> ListarPosiciones(string idtorneoseleccionado)
        {
            return ObtenerPosiciones(idtorneoseleccionado);
        }


        [HttpGet]
        [Route("api/Posiciones/ExportarPosicionesExcel/{idtorneoseleccionado}")]
        public IActionResult ExportarPosicionesExcel(string idtorneoseleccionado)
        {
            List<PosicionesCLS> listaPosiciones = ObtenerPosiciones(idtorneoseleccionado);

            ExcelPackage.License.SetNonCommercialPersonal("FUTBOLERO");
            using (MemoryStream ms = new MemoryStream())
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Posiciones");
                    var ws = ep.Workbook.Worksheets[0];

                    string[] cabeceras = { "EQUIPO", "JJ", "JG", "JE", "JEGP", "JP", "GF", "GC", "DG", "PE", "PTOS" };
                    for (int i = 0; i < cabeceras.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = cabeceras[i];
                        ws.Cells[1, i + 1].Style.Font.Bold = true;
                    }

                    int fila = 2;
                    foreach (var item in listaPosiciones)
                    {
                        ws.Cells[fila, 1].Value = item.nombre;
                        ws.Cells[fila, 2].Value = item.jugados;
                        ws.Cells[fila, 3].Value = item.ganados;
                        ws.Cells[fila, 4].Value = item.empatados;
                        ws.Cells[fila, 5].Value = item.empatadosganados;
                        ws.Cells[fila, 6].Value = item.perdidos;
                        ws.Cells[fila, 7].Value = item.golesafavor;
                        ws.Cells[fila, 8].Value = item.golesencontra;
                        ws.Cells[fila, 9].Value = item.diferenciagoles;
                        ws.Cells[fila, 10].Value = item.puntosextras;
                        ws.Cells[fila, 11].Value = item.puntos;
                        fila++;
                    }

                    for (int i = 1; i <= cabeceras.Length; i++)
                    {
                        ws.Column(i).AutoFit();
                    }

                    ep.SaveAs(ms);
                }

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TablaPosiciones.xlsx");
            }
        }


        [HttpGet]
        [Route("api/Posiciones/ExportarPosicionesPdf/{idtorneoseleccionado}")]
        public IActionResult ExportarPosicionesPdf(string idtorneoseleccionado)
        {
            List<PosicionesCLS> listaPosiciones = ObtenerPosiciones(idtorneoseleccionado);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                {
                    Document doc = new Document(pdfDoc);
                    doc.Add(new Paragraph("Tabla de Posiciones").SetFontSize(16));

                    Table tabla = new Table(11);
                    string[] cabeceras = { "EQUIPO", "JJ", "JG", "JE", "JEGP", "JP", "GF", "GC", "DG", "PE", "PTOS" };
                    foreach (string cab in cabeceras)
                    {
                        tabla.AddHeaderCell(new Cell().Add(new Paragraph(cab)));
                    }

                    foreach (var item in listaPosiciones)
                    {
                        tabla.AddCell(new Cell().Add(new Paragraph(item.nombre ?? "")));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.jugados.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.ganados.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.empatados.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.empatadosganados.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.perdidos.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.golesafavor.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.golesencontra.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.diferenciagoles.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.puntosextras.ToString())));
                        tabla.AddCell(new Cell().Add(new Paragraph(item.puntos.ToString())));
                    }

                    doc.Add(tabla);
                    doc.Close();
                }

                return File(ms.ToArray(), "application/pdf", "TablaPosiciones.pdf");
            }
        }

    }
}


