using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appcitas.Repository;
using appcitas.Controllers;
using appcitas.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using System.ComponentModel;
using System.Globalization;

namespace appcitas.Controllers
{
    public class CajaController : Controller
    {
        

        public ActionResult InicioCaja()
        {
            return View();
        }

        public ActionResult ClientesCaja()
        {
            return View();
        }

        public ActionResult PrestamoCaja()
        {
            return View();
        }

        public ActionResult DPFCaja()
        {
            return View();
        }

        public ActionResult Cajero()
        {
            return View();
        }

        //LISTA DE ESTADOS
        [HttpPost]
        public JsonResult ListaTiposTrans()
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.ListaTiposTrans(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrosTiposTransacciones> list = new List<OtrosTiposTransacciones>();
                OtrosTiposTransacciones obj = new OtrosTiposTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula Datos sobre pago a prestamo
        [HttpPost]
        public JsonResult GetDatosPagoPrestamo(int id)
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.PagoPrestamo(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoPrestamo> list = new List<GetDatosPagoPrestamo>();
                GetDatosPagoPrestamo obj = new GetDatosPagoPrestamo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Realiza Pago a prestamo
        [HttpPost]
        public JsonResult PagoPrestamo(int codigo, string Capital, string Intereses, string Mora, int TipoPago, string Observacion)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.PagoPrestamo(codigo, Capital, Intereses, Mora, TipoPago, Cajero, Observacion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoPrestamo> list = new List<GetDatosPagoPrestamo>();
                GetDatosPagoPrestamo obj = new GetDatosPagoPrestamo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult PagoPrestamoAjuste(int codigo, string Capital, string Intereses, string Mora, int TipoPago, string Observacion, string FechaAjustePrestamo)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.PagoPrestamoAjuste(codigo, Capital, Intereses, Mora, TipoPago, Cajero, Observacion, FechaAjustePrestamo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoPrestamo> list = new List<GetDatosPagoPrestamo>();
                GetDatosPagoPrestamo obj = new GetDatosPagoPrestamo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult OtrosPagos(decimal Monto, string Observacion, int ListaTiposTrans, string Clave)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.OtrosPagos(Monto, Observacion, Cajero, ListaTiposTrans, Clave), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult OtrosPagosAjuste(decimal Monto, string Observacion, int ListaTiposTrans, string Clave, string FechaAjuste)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.OtrosPagosAjuste(Monto, Observacion, Cajero, ListaTiposTrans, Clave, FechaAjuste), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula registros de cajero
        [HttpPost]
        public JsonResult GetDatosCajero(string fecha,int FechaActual)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GetDatosCajero(Cajero, fecha, FechaActual), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Cajero> list = new List<Cajero>();
                Cajero obj = new Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Carga transacciones de cajero
        [HttpPost]
        public JsonResult GetTransaccionesCajero(string fecha)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GetTransaccionesCajero(Cajero, fecha), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Transaccion de cajero
        [HttpPost]
        public JsonResult GeneraTransCajero(decimal Monto, int Tipo)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GeneraTransCajero(Monto, Tipo, Cajero), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Registro de Cajero
        [HttpPost]
        public JsonResult AgregaRegCajero()
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.AgregaRegCajero(Cajero), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Carga transacciones de cajero
        [HttpPost]
        public JsonResult GetRecibos()
        {
            CajaRepository CitaRep = new CajaRepository();
            try
            {
                return Json(CitaRep.GetRecibos(), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Recibos> list = new List<Recibos>();
                Recibos obj = new Recibos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Registro de Cajero
        [HttpPost]
        public JsonResult AnularRecibo(int Documento)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.AnularRecibo(Documento), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult GuardarDatosArqueo(int CA_Secuencia, int CA_B_1, int CA_B_2, int CA_B_5, int CA_B_10, int CA_B_20, int CA_B_50, int CA_B_100, int CA_B_500, int CA_M_1, int CA_M_2, int CA_M_5, int CA_M_10, int CA_M_20, int CA_M_50)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GuardarDatosArqueo(CA_Secuencia, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5, CA_M_10, CA_M_20, CA_M_50), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult CierreCaja(int Secuencia, float Diferencia )
        {
            
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.CierreCaja(Secuencia, Diferencia), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Valida estado del Cierre de caja
        [HttpPost]
        public JsonResult ValidaCajero()
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.ValidaCajero(Cajero), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Valida estado del Cierre de caja para realizar ajuste 
        [HttpPost]
        public JsonResult ValidaCajeroAjuste(string Fecha)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.ValidaCajeroAjuste(Cajero,Fecha), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula Datos sobre pago a prestamo
        [HttpPost]
        public JsonResult GetCierreDia(string Fecha, string Cajero)
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.GetCierreDia(Fecha,Cajero), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Cajero> list = new List<Cajero>();
                Cajero obj = new Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Actualiza estado de dia
        [HttpPost]
        public JsonResult ActualizaDiaCierre(string Fecha, string Cajero, int Estado)
        {
            CajaRepository CitaRep = new CajaRepository();
            //var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.ActualizaDiaCierre(Fecha, Cajero, Estado), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //RECIBO DE PAGO A PRESTAMO
        //[HttpPost]
        //public int ReciboPagoPrestamo(int recibo)
        //{
        //    string pathPDF = "~\\PDF\\pdf.pdf";
        //    string pathPDF2 = "~\\PDF\\pdf_prueba.pdf";
        //    //Objeto para leer el pdf original
        //    PdfReader oReader = new PdfReader(pathPDF);
        //    //Objeto que tiene el tamaño de nuestro documento
        //    Rectangle oSize = oReader.GetPageSizeWithRotation(1);
        //    //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
        //    Document oDocument = new Document(oSize);

        //    // Creamos el objeto en el cual haremos la inserción
        //    FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
        //    PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
        //    oDocument.Open();

        //    //El contenido del pdf, aqui se hace la escritura del contenido
        //    PdfContentByte oPDF = oWriter.DirectContent;

        //    //Propiedades de nuestra fuente a insertar
        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    oPDF.SetColorFill(BaseColor.RED);
        //    oPDF.SetFontAndSize(bf, 8);

        //    //Se abre el flujo para escribir el texto
        //    oPDF.BeginText();
        //    //asignamos el texto
        //    string text = "HOLA SOY UN TEXTO ROJO EN UN PDF";
        //    // Le damos posición y rotación al texto
        //    // la posición de Y es al revés de como estamos acostumbrados
        //    oPDF.ShowTextAligned(1, text, 30, oSize.Height - 30, 0);
        //    oPDF.EndText();

        //    //crea una nueva pagina y agrega el pdf original
        //    PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
        //    oPDF.AddTemplate(page, 0, 0);

        //    // Cerramos los objetos utilizados
        //    oDocument.Close();
        //    oFS.Close();
        //    oWriter.Close();
        //    oReader.Close();
        //    //CajaRepository CajaRep = new CajaRepository();
        //    //List<ReciboPagoPrestamo> lista = new List<ReciboPagoPrestamo>();
        //    //lista = CajaRep.ReciboPagoPrestamo(recibo);

        //    //int RE_Numero = lista[0].RE_Numero;
        //    //string Nombre = lista[0].Nombre;
        //    //decimal RE_Documento = lista[0].RE_Documento;
        //    //string RE_Fecha = lista[0].RE_Fecha;
        //    //string Tipo = lista[0].Tipo;
        //    //decimal PRES_Saldo = lista[0].PRES_Saldo;
        //    //decimal RE_Total_Rec = lista[0].RE_Total_Rec;

        //    //string NombreArchivo = "ReciboPagoPrestamo" + recibo + ".pdf";
        //    //string carpeta = @"C:\inetpub\wwwroot\SistemaFinanciero\Caja\";
        //    //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
        //    //PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
        //    //iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
        //    //addLogo = iTextSharp.text.Image.GetInstance("C:/inetpub/wwwroot/SistemaFinanciero/imgs" + "/logo_cr.png");
        //    //doc.Open();
        //    //Font LineBreak = FontFactory.GetFont("COURIER", size: 11);
        //    //addLogo.ScaleToFit(200, 128);
        //    //addLogo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
        //    //doc.Add(addLogo);
        //    //doc.Add(new Paragraph("\n"));
        //    //doc.Add(new Paragraph("\n"));
        //    //doc.Add(new Paragraph("\n"));
        //    //doc.Add(new Paragraph("\n"));
        //    //doc.Add(new Paragraph(String.Format("     Recibo de Pago \n"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 14, iTextSharp.text.Font.BOLD)));
        //    //doc.Add(new Paragraph(String.Format("  FINANCIERA UNICREDIT \n\n"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 13, iTextSharp.text.Font.BOLD)));
        //    //doc.Add(new Paragraph("  Nro Boleta: " + RE_Numero + " \n", LineBreak));
        //    //doc.Add(new Paragraph("  Nombres: " + Nombre + "\n", LineBreak));
        //    //doc.Add(new Paragraph("  Documento: " + RE_Documento + "\n", LineBreak));
        //    //doc.Add(new Paragraph("  Fecha: " + RE_Fecha + "\n", LineBreak));
        //    //doc.Add(new Paragraph("  Tipo: " + Tipo + "\n\n", LineBreak));
        //    //doc.Add(new Paragraph("  Resta pagar: L." + PRES_Saldo + " \n\n", LineBreak));
        //    //doc.Add(new Paragraph("  Monto Pagado: L." + RE_Total_Rec + "\n\n\n\n", LineBreak));
        //    //Paragraph Firma = new Paragraph(string.Format("    __________________"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 12, iTextSharp.text.Font.BOLD));
        //    //Firma.Alignment = 0;
        //    //doc.Add(Firma);
        //    //Paragraph Firma1 = new Paragraph(string.Format("          FIRMA"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 12, iTextSharp.text.Font.BOLD));
        //    //Firma1.Alignment = 0;
        //    //doc.Add(Firma1);

        //    //doc.Close();
        //    return 1;

        //}

        [HttpPost]
        public FileResult ReciboPagoPrestamo(int recibo)
        {
            string pathPDF = "~\\PDF\\pdf.pdf";
            string pathPDF2 = "~\\PDF\\pdf_prueba.pdf";
            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.RED);
            oPDF.SetFontAndSize(bf, 8);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string text = "HOLA SOY UN TEXTO ROJO EN UN PDF";
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(1, text, 30, oSize.Height - 30, 0);
            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return File(pathPDF2, "application/pdf");
        }

        [HttpPost]
        public int ReciboOtros(int recibo)
        {
            CajaRepository CajaRep = new CajaRepository();
            List<ReciboOtros> lista = new List<ReciboOtros>();
            lista = CajaRep.ReciboOtros(recibo);

            int re_numero = lista[0].re_numero;
            string servicio = lista[0].servicio;
            string re_observacion = lista[0].re_observacion;
            string re_fecha = lista[0].re_fecha;
            decimal re_total_rec = lista[0].re_total_rec;

            string NombreArchivo = "ReciboOtrasTransacciones" + recibo + ".pdf";
            string carpeta = @"C:\inetpub\wwwroot\SistemaFinanciero\";
            //string carpeta = @"C:\FERNANDOG\Desarrollos\CORE FINANCIERO\Sistema Financiero\Sistema Financiero\appcitas\";
            Document doc = new Document(iTextSharp.text.PageSize.LEGAL, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
            iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
            //addLogo = iTextSharp.text.Image.GetInstance("C:/Users/Fernando Giron/Desktop/FORMATOS" + "/logo_cr.png");
            addLogo = iTextSharp.text.Image.GetInstance("C:/inetpub/wwwroot/SistemaFinanciero/imgs" + "/logo_cr.png");
            doc.Open();
            Font LineBreak = FontFactory.GetFont("COURIER", size: 10);
            Font Negritas = FontFactory.GetFont(FontFactory.COURIER_BOLD, 12);
            addLogo.ScaleToFit(200, 128);
            //addLogo.ScaleToFit(128, 37);
            addLogo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
            doc.Add(addLogo);
            Paragraph Titulo = new Paragraph(string.Format("   Bo El Centro,Calle Principal \n    Santiago Puringla,La Paz"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10, iTextSharp.text.Font.BOLD));
            Paragraph SubTitulo = new Paragraph(string.Format("         Cel.9841-9334 \n   Email:unicredithn@gmail.com"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10, iTextSharp.text.Font.BOLD));
            Paragraph TituloRecibo = new Paragraph(string.Format("    RECIBO DE PAGO"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 14, iTextSharp.text.Font.BOLD));
            //Titulo.SpacingBefore = 200;
            //Titulo.SpacingAfter = 0;
            Titulo.Alignment = 0; //0-Left, 1 middle,2 Right
            SubTitulo.Alignment = 0;
            TituloRecibo.Alignment = 0;
            doc.Add(Titulo);
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(SubTitulo);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(TituloRecibo);
            doc.Add(new Paragraph("\n", LineBreak));
            //Parrafo1.(1, 1);
            Paragraph Servicio = new Paragraph(string.Format("        SERVICIO:\n     " + servicio), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 11, iTextSharp.text.Font.BOLD));
            Servicio.Alignment = 0;
            doc.Add(Servicio);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  NOMBRE: " + re_observacion, LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  FECHA DE PAGO: " + re_fecha, LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  NÚMERO DE SERVICIO: " + re_numero, LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  SALDO EN DOLARES: L. 0.00", LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  TOTAL PAGADO: L. " + re_total_rec, LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("  REF. BANCARIA:", LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            //doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph Firma = new Paragraph(string.Format("    __________________"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 12, iTextSharp.text.Font.BOLD));
            Firma.Alignment = 0;
            doc.Add(Firma);
            Paragraph Firma1 = new Paragraph(string.Format("          FIRMA"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 12, iTextSharp.text.Font.BOLD));
            Firma1.Alignment = 0;
            doc.Add(Firma1);
            //Paragraph Firma2 = new Paragraph(String.Format("    ENCARGADO"), LineBreak);
            //doc.Add(Firma2);
            doc.Close();
            return 1;

        }

        //REIMPRESION DE RECIBOS DE PRESTAMO Y OTRAS TRANSACCIONES

        //public ActionResult ReimprimirRecibo() 
        //{
        //    string pathPDF = @"C:\Users\netze\OneDrive\Documentos\Sistema Financiero\appcitas\PDF\pdf.pdf";
        //    string pathPDF2 = @"C:\Users\netze\OneDrive\Documentos\Sistema Financiero\appcitas\PDF\pdf_prueba.pdf";
        //    //Objeto para leer el pdf original
        //    PdfReader oReader = new PdfReader(pathPDF);
        //    //Objeto que tiene el tamaño de nuestro documento
        //    Rectangle oSize = oReader.GetPageSizeWithRotation(1);
        //    //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
        //    Document oDocument = new Document(oSize);

        //    // Creamos el objeto en el cual haremos la inserción
        //    FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
        //    PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
        //    oDocument.Open();

        //    //El contenido del pdf, aqui se hace la escritura del contenido
        //    PdfContentByte oPDF = oWriter.DirectContent;

        //    //Propiedades de nuestra fuente a insertar
        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    oPDF.SetColorFill(BaseColor.RED);
        //    oPDF.SetFontAndSize(bf, 8);

        //    //Se abre el flujo para escribir el texto
        //    oPDF.BeginText();
        //    //asignamos el texto
        //    string text = "HOLA SOY UN TEXTO ROJO EN UN PDF";
        //    // Le damos posición y rotación al texto
        //    // la posición de Y es al revés de como estamos acostumbrados
        //    oPDF.ShowTextAligned(1, text, 30, oSize.Height - 30, 0);
        //    oPDF.EndText();

        //    //crea una nueva pagina y agrega el pdf original
        //    PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
        //    oPDF.AddTemplate(page, 0, 0);

        //    // Cerramos los objetos utilizados
        //    oDocument.Close();
        //    oFS.Close();
        //    oWriter.Close();
        //    oReader.Close();


        //    return new File(pathPDF2, "application/pdf"); 

        //}

        //[HttpPost]
        //public int ReimprimirRecibo()
        //{
        //    string pathPDF =  System.Configuration.ConfigurationManager.AppSettings["formatopdf"]; 
        //    string pathPDF2 = @"C:\Users\netze\OneDrive\Documentos\Sistema Financiero\appcitas\PDF\pdf_prueba.pdf";
        //    //Objeto para leer el pdf original
        //    PdfReader oReader = new PdfReader(pathPDF);
        //    //Objeto que tiene el tamaño de nuestro documento
        //    Rectangle oSize = oReader.GetPageSizeWithRotation(1);
        //    //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
        //    Document oDocument = new Document(oSize);

        //    // Creamos el objeto en el cual haremos la inserción
        //    FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
        //    PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
        //    oDocument.Open();

        //    //El contenido del pdf, aqui se hace la escritura del contenido
        //    PdfContentByte oPDF = oWriter.DirectContent;

        //    //Propiedades de nuestra fuente a insertar
        //    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    oPDF.SetColorFill(BaseColor.RED);
        //    oPDF.SetFontAndSize(bf, 14);

        //    //Se abre el flujo para escribir el texto
        //    oPDF.BeginText();
        //    //asignamos el texto
        //    string text = "POCA CULERO";
        //    // Le damos posición y rotación al texto
        //    // la posición de Y es al revés de como estamos acostumbrados
        //    oPDF.ShowTextAligned(0, text, int.Parse(Recursos.RQ1.Nombre_cliente_X), int.Parse(Recursos.RQ1.Nombre_cliente_Y), 0);
        //    oPDF.EndText();

        //    //crea una nueva pagina y agrega el pdf original
        //    PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
        //    oPDF.AddTemplate(page, 0, 0);

        //    // Cerramos los objetos utilizados
        //    oDocument.Close();
        //    oFS.Close();
        //    oWriter.Close();
        //    oReader.Close();

            

        //    return 1;

        //}


        [HttpPost]
        public int RQ1(RQ1 gPrestamo)
        {
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["formatopdf"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["formatopdf2"];
            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);
            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;
            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 14);
            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string Nombre = gPrestamo.Nombre;
            string Fecha = DateTime.Today.ToString("dd") + " de " + DateTime.Today.ToString("MMMM") + " del " + DateTime.Today.Year.ToString();
            float Monto = float.Parse(gPrestamo.Monto, CultureInfo.InvariantCulture.NumberFormat);
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(0, Nombre, int.Parse(Recursos.RQ1.Nombre_cliente_X), int.Parse(Recursos.RQ1.Nombre_cliente_Y), 0);
            oPDF.ShowTextAligned(0, Fecha, int.Parse(Recursos.RQ1.Fecha_X), int.Parse(Recursos.RQ1.Fecha_Y), 0);
            oPDF.ShowTextAligned(0, Monto.ToString(), int.Parse(Recursos.RQ1.Monto_X), int.Parse(Recursos.RQ1.Monto_Y), 0);
            oPDF.EndText();
            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);
            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        [HttpPost]
        public int RQ2(RQ1 gPrestamo)
        {
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["RQ2"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["RQ2_2"];
            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);
            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;
            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 14);
            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string Nombre = gPrestamo.Nombre;
            string Fecha = DateTime.Today.ToString("dd") + " de " + DateTime.Today.ToString("MMMM") + " del " + DateTime.Today.Year.ToString();
            float Monto = float.Parse(gPrestamo.Monto, CultureInfo.InvariantCulture.NumberFormat);
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(0, Nombre, int.Parse(Recursos.RQ2.Nombre_cliente_X), int.Parse(Recursos.RQ2.Nombre_cliente_Y), 0);
            oPDF.ShowTextAligned(0, Fecha, int.Parse(Recursos.RQ2.Fecha_X), int.Parse(Recursos.RQ2.Fecha_Y), 0);
            oPDF.ShowTextAligned(0, Monto.ToString(), int.Parse(Recursos.RQ2.Monto_X), int.Parse(Recursos.RQ2.Monto_Y), 0);
            oPDF.EndText();
            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);
            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        [HttpPost]
        public int RQ3(RQ1 gPrestamo)
        {
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["RQ3"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["RQ3_3"];
            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);
            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();
            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;
            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 14);
            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string Nombre = gPrestamo.Nombre;
            string Fecha = DateTime.Today.ToString("dd") + " de " + DateTime.Today.ToString("MMMM") + " del " + DateTime.Today.Year.ToString();
            float Monto = float.Parse(gPrestamo.Monto, CultureInfo.InvariantCulture.NumberFormat);
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(0, Nombre, int.Parse(Recursos.RQ3.Nombre_cliente_X), int.Parse(Recursos.RQ3.Nombre_cliente_Y), 0);
            oPDF.ShowTextAligned(0, Fecha, int.Parse(Recursos.RQ3.Fecha_X), int.Parse(Recursos.RQ3.Fecha_Y), 0);
            oPDF.ShowTextAligned(0, Monto.ToString(), int.Parse(Recursos.RQ3.Monto_X), int.Parse(Recursos.RQ3.Monto_Y), 0);
            oPDF.EndText();
            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);
            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        [HttpPost]
        public JsonResult AutorizaCierre(string user, string pass, string CodigoModulo)
        {
            CajaRepository Home = new CajaRepository();
            try
            {
                return Json(Home.AutorizaCierre(user, pass, CodigoModulo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AutorizaModMora(string user, string pass, string CodigoModulo)
        {
            CajaRepository Home = new CajaRepository();
            try
            {
                return Json(Home.AutorizaModMora(user, pass, CodigoModulo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula Datos sobre Pago de Intereses Depositos a Plazo Fijo
        [HttpPost]
        public JsonResult GetPagoIntDPF(int id)
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.GetPagoIntDPF(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoIntDPF> list = new List<GetDatosPagoIntDPF>();
                GetDatosPagoIntDPF obj = new GetDatosPagoIntDPF();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Generar Pago DPF
        [HttpPost]
        public JsonResult PagoIntDPF(int codigo,string Intereses, string Observacion)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.PagoIntDPF(codigo, Intereses, Cajero, Observacion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoIntDPF> list = new List<GetDatosPagoIntDPF>();
                GetDatosPagoIntDPF obj = new GetDatosPagoIntDPF();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult PagoIntDPFAjuste(int codigo,string Intereses, string Observacion, string FechaAjusteDPF)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.PagoIntDPFAjuste(codigo, Intereses, Cajero, Observacion, FechaAjusteDPF), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoIntDPF> list = new List<GetDatosPagoIntDPF>();
                GetDatosPagoIntDPF obj = new GetDatosPagoIntDPF();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public int CertificadoDeposito(DepositosPlazoFijo DPF)
        {
            Moneda oMoneda = new Moneda();
            //string cantLetras = oMoneda.Convertir(Convert.ToString(certificado.valorDPF), true, "Lempiras");
            string cantLetras = oMoneda.Convertir(Convert.ToString(DPF.DPF_Monto), true, "Lempiras");
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["CertificadoDPF"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["CertificadoDPF_1"];

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSize(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 11);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string NumRecibo = LlenarRecibos(DPF.DPF_Codigo);


            //string NumRecibo = "00087";
            string Lugar = "Santiago de Puringla";
            string Cantidad = DPF.DPF_Monto;
            string Fecha = DateTime.Now.ToString("dd-MM-yyyy");//string.Format("{0:M}", DateTime.Now) + " de " + string.Format("{0:yyyy}", DateTime.Now);
            string ValorLetras = cantLetras;
            string Depositante = DPF.Nombre;
            string IdDepositante = Convert.ToString(DPF.ClienteId);
            string Beneficiario1 = (DPF.DPF_Beneficiario_1 == null ? "" : DPF.DPF_Beneficiario_1);
            string Beneficiario2 = (DPF.DPF_Beneficiario_2 == null ? "" : DPF.DPF_Beneficiario_2);
            string Beneficiario3 = (DPF.DPF_Beneficiario_3 == null ? "" : DPF.DPF_Beneficiario_3);
            string idBeneficiario1 = (DPF.DPF_ID_Bene_1 == null ? "" : DPF.DPF_ID_Bene_1);
            string idBeneficiario2 = (DPF.DPF_ID_Bene_2 == null ? "" : DPF.DPF_ID_Bene_2);
            string FechaConstitucion = Convert.ToDateTime(DPF.DPF_Fecha_Apert).ToString("dd-MM-yyyy");
            string idBeneficiario3 = (DPF.DPF_ID_Bene_3 == null ? "" : DPF.DPF_ID_Bene_3);
            string FechaVencimiento = Convert.ToDateTime(DPF.FechaVencimiento).ToString("dd-MM-yyyy");
            string Plazos = DPF.DPF_Plazo;
            string PeriodicidadInt = (DPF.DPF_Pago_intereses == null ? "" : DPF.DPF_Pago_intereses);
            string Tasa = DPF.DPF_Tasa_interes + " %";

            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(1, NumRecibo, 54, oSize.Height - 135, 0);
            oPDF.ShowTextAligned(1, Cantidad, 230, oSize.Height - 132, 0);
            oPDF.ShowTextAligned(1, Lugar, 200, oSize.Height - 170, 0);
            oPDF.ShowTextAligned(1, Fecha, 470, oSize.Height - 172, 0);
            oPDF.ShowTextAligned(1, cantLetras, 255, oSize.Height - 188, 0);
            oPDF.ShowTextAligned(1, Depositante, 180, oSize.Height - 202, 0);
            oPDF.ShowTextAligned(1, IdDepositante, 460, oSize.Height - 202, 0);
            oPDF.ShowTextAligned(1, Beneficiario1, 180, oSize.Height - 218, 0);
            oPDF.ShowTextAligned(1, Beneficiario2, 180, oSize.Height - 234, 0);
            oPDF.ShowTextAligned(1, Beneficiario3, 180, oSize.Height - 248, 0);
            oPDF.ShowTextAligned(1, idBeneficiario1, 460, oSize.Height - 218, 0);
            oPDF.ShowTextAligned(1, idBeneficiario2, 460, oSize.Height - 234, 0);
            oPDF.ShowTextAligned(1, idBeneficiario3, 460, oSize.Height - 250, 0);
            oPDF.ShowTextAligned(1, FechaConstitucion, 165, oSize.Height - 265, 0);
            oPDF.ShowTextAligned(1, FechaVencimiento, 350, oSize.Height - 265, 0);
            oPDF.ShowTextAligned(1, Plazos, 470, oSize.Height - 265, 0);
            oPDF.ShowTextAligned(1, PeriodicidadInt, 215, oSize.Height - 280, 0);
            oPDF.ShowTextAligned(1, Tasa, 415, oSize.Height - 280, 0);

            //oPDF.ShowTextAligned(1, Convert.ToString(Cantidad), 230, oSize.Height - 410, 0);
            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        public string LlenarRecibos(int recibo)
        {
            var NumRecibo = "";
            if (recibo >= 0 && recibo < 10)
            {
                NumRecibo = "0000" + recibo;
            }
            else if (recibo > 10 && recibo < 100)
            {
                NumRecibo = "000" + recibo;
            }
            else if (recibo > 100 && recibo < 1000)
            {
                NumRecibo = "00" + recibo;
            }
            else if (recibo > 1000 && recibo < 10000)
            {
                NumRecibo = "0" + recibo;
            }
            else
            {
                NumRecibo = Convert.ToString(recibo);
            }
            return NumRecibo;
        }

        public int ReciboInteresDPF(ReciboPlazoFijo Recibo)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["Recibo_Int_PlazoF"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["Recibo_Int_PlazoF_1"];

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 10);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string Fecha = DateTime.Today.ToString("dd-MM-yyyy");
            string Lugar = "SANTIAGO PURINGLA, LA PAZ.";
            string Identidad = Recibo.ClienteId;
            string Cuenta = Recibo.CodigoCliente;
            string Deposito = LlenarRecibos(Recibo.CodigoDPF);
            double Cantidad = Recibo.Cantidad;
            string Descripcion = "Intereses a Deposito";
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            oPDF.ShowTextAligned(1, Lugar, 110, oSize.Height - 150, 0);
            oPDF.ShowTextAligned(1, Fecha, 115, oSize.Height - 160, 0);
            oPDF.ShowTextAligned(1, Cuenta, 120, oSize.Height - 173, 0);
            oPDF.ShowTextAligned(1, Identidad, 128, oSize.Height - 187, 0);
            oPDF.ShowTextAligned(1, Deposito, 100, oSize.Height - 205, 0);
            oPDF.ShowTextAligned(1, Convert.ToString(Cantidad), 100, oSize.Height - 220, 0);
            oPDF.ShowTextAligned(1, Descripcion, 125, oSize.Height - 235, 0);

            //oPDF.ShowTextAligned(1, text, 70, oSize.Height - 100, 0);
            //oPDF.ShowTextAligned(1, text, 150, oSize.Height - 269, 0);
            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        public JsonResult GetReciboDPF(string id)
        {
            CajaRepository CitaRep = new CajaRepository();
            try
            {
                return Json(CitaRep.GetReciboDPF(id), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<DepositosPlazoFijo> list = new List<DepositosPlazoFijo>();
                DepositosPlazoFijo obj = new DepositosPlazoFijo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //OBTIENE INFORMACION DE RECIBO DE OTRAS TRANSACCIONES
        public JsonResult GeneraReciboOtrasTransac(int Recibo)
        {
            try
            {
                CajaRepository CajaRep = new CajaRepository();
                if (ModelState.IsValid)
                {
                    CajaRep.ReciboOtros(Recibo);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(CajaRep.ReciboOtros(Recibo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(Recibo, JsonRequestBehavior.AllowGet);
            }
        }

        //GENERA PDF RECIBO DE PAGO OTRAS TRANSACCIONES
        public int GeneraPdfReciboOtrasTransac(ReciboOtros reciboOtros)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoOT"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoOT1"];

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 10);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string NoRecibo = Convert.ToString(reciboOtros.re_numero);// Convert.ToString(Pagare.Valor);
            string Nombre = Convert.ToString(reciboOtros.re_observacion);
            string Fecha = Convert.ToString(reciboOtros.re_fecha);
            string Clave = Convert.ToString(reciboOtros.OT_Clave);
            string Concepto = Convert.ToString(reciboOtros.servicio);
            string TotalRecibo = Convert.ToString(reciboOtros.re_total_rec);


            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

            oPDF.ShowTextAligned(1, NoRecibo, Convert.ToInt32(Recursos.ReciboOtros.NoRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.NoRecibo_Y), 0);
            oPDF.ShowTextAligned(1, Nombre, Convert.ToInt32(Recursos.ReciboOtros.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Nombre_Y), 0);
            oPDF.ShowTextAligned(1, Fecha, Convert.ToInt32(Recursos.ReciboOtros.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Fecha_Y), 0);
            oPDF.ShowTextAligned(1, Clave, Convert.ToInt32(Recursos.ReciboOtros.Clave_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Clave_Y), 0);
            oPDF.ShowTextAligned(1, Concepto, Convert.ToInt32(Recursos.ReciboOtros.Concepto_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Concepto_Y), 0);
            oPDF.ShowTextAligned(1, TotalRecibo, Convert.ToInt32(Recursos.ReciboOtros.TotalRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.TotalRecibo_Y), 0);

            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        //OBTIENE INFORMACION DE RECIBO PAGO A PRESTAMO
        public JsonResult GeneraReciboPagoPrestamo1(int Recibo)
        {
            try
            {
                CajaRepository CajaRep = new CajaRepository();
                if (ModelState.IsValid)
                {
                    CajaRep.ReciboPagoPrestamo(Recibo);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(CajaRep.ReciboPagoPrestamo(Recibo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(Recibo, JsonRequestBehavior.AllowGet);
            }
        }

        //GENERA PDF RECIBO DE PAGO A PRESTAMO 
        public int GeneraReciboPagoPrestamo(ReciboPagoPrestamo ReciboPagPrestamo)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoPrestamo"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoPrestamo1"];

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 10);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string Saldo = Convert.ToString(ReciboPagPrestamo.PRES_Saldo);// Convert.ToString(Pagare.Valor);
            string Documento = Convert.ToString(ReciboPagPrestamo.RE_Documento);
            string Fecha = Convert.ToString(ReciboPagPrestamo.RE_Fecha);
            string NoRecibo = Convert.ToString(ReciboPagPrestamo.RE_Numero);
            string ReciboTotal = Convert.ToString(ReciboPagPrestamo.RE_Total_Rec);
            string Tipo = Convert.ToString(ReciboPagPrestamo.Tipo);
            string Nombre = Convert.ToString(ReciboPagPrestamo.Nombre);

            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

            oPDF.ShowTextAligned(1, Saldo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Saldo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Saldo_Y), 0);
            oPDF.ShowTextAligned(1, Documento, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Documento_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Documento_Y), 0);
            oPDF.ShowTextAligned(1, Fecha, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Fecha_Y), 0);
            oPDF.ShowTextAligned(1, NoRecibo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.NoRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.NoRecibo_Y), 0);
            oPDF.ShowTextAligned(1, ReciboTotal, Convert.ToInt32(Recursos.ReciboPagoPrestamos.ReciboTotal_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.ReciboTotal_Y), 0);
            oPDF.ShowTextAligned(1, Tipo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Tipo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Tipo_Y), 0);
            oPDF.ShowTextAligned(1, Nombre, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Nombre_Y), 0);
            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }

        //REIMPRESION DE RECIBOS DE PRESTAMO Y OTRAS TRANSACCIONES
        [HttpPost]
        public int ReimprimirRecibo(int recibo, int tipo)
        {
            if (tipo == 0)
            {
                CajaRepository CajaRep = new CajaRepository();
                List<ReciboPagoPrestamo> lista = new List<ReciboPagoPrestamo>();
                lista = CajaRep.ReciboPagoPrestamo(recibo);

                string NoRecibo = Convert.ToString(lista[0].RE_Numero);
                string Nombre = lista[0].Nombre;
                string Documento = Convert.ToString(lista[0].RE_Documento);
                string Fecha = lista[0].RE_Fecha;
                string Tipo = lista[0].Tipo;
                string Saldo = Convert.ToString(lista[0].PRES_Saldo);
                string ReciboTotal = Convert.ToString(lista[0].RE_Total_Rec);

                //rutas de nuestros pdf
                string pathPDF = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoPrestamo"];
                string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoPrestamo1"];

                //Objeto para leer el pdf original
                PdfReader oReader = new PdfReader(pathPDF);
                //Objeto que tiene el tamaño de nuestro documento
                Rectangle oSize = oReader.GetPageSizeWithRotation(1);
                //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
                Document oDocument = new Document(oSize);

                // Creamos el objeto en el cual haremos la inserción
                FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
                PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
                oDocument.Open();

                //El contenido del pdf, aqui se hace la escritura del contenido
                PdfContentByte oPDF = oWriter.DirectContent;

                //Propiedades de nuestra fuente a insertar
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                oPDF.SetColorFill(BaseColor.BLACK);
                oPDF.SetFontAndSize(bf, 10);

                //Se abre el flujo para escribir el texto
                oPDF.BeginText();


                //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

                oPDF.ShowTextAligned(1, Saldo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Saldo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Saldo_Y), 0);
                oPDF.ShowTextAligned(1, Documento, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Documento_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Documento_Y), 0);
                oPDF.ShowTextAligned(1, Fecha, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Fecha_Y), 0);
                oPDF.ShowTextAligned(1, NoRecibo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.NoRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.NoRecibo_Y), 0);
                oPDF.ShowTextAligned(1, ReciboTotal, Convert.ToInt32(Recursos.ReciboPagoPrestamos.ReciboTotal_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.ReciboTotal_Y), 0);
                oPDF.ShowTextAligned(1, Tipo, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Tipo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Tipo_Y), 0);
                oPDF.ShowTextAligned(1, Nombre, Convert.ToInt32(Recursos.ReciboPagoPrestamos.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.ReciboPagoPrestamos.Nombre_Y), 0);
                oPDF.EndText();

                //crea una nueva pagina y agrega el pdf original
                PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
                oPDF.AddTemplate(page, 0, 0);

                // Cerramos los objetos utilizados
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
            }
            else
            {
                CajaRepository CajaRep = new CajaRepository();
                List<ReciboOtros> lista = new List<ReciboOtros>();
                lista = CajaRep.ReciboOtros(recibo);

                string re_numero = Convert.ToString(lista[0].re_numero);
                string servicio = lista[0].servicio;
                string re_observacion = lista[0].re_observacion;
                string re_fecha = lista[0].re_fecha;
                string re_total_rec = Convert.ToString(lista[0].re_total_rec);
                string Clave = lista[0].OT_Clave;

                //rutas de nuestros pdf
                string pathPDF = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoOT"];
                string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["ReciboPagoOT1"];

                //Objeto para leer el pdf original
                PdfReader oReader = new PdfReader(pathPDF);
                //Objeto que tiene el tamaño de nuestro documento
                Rectangle oSize = oReader.GetPageSizeWithRotation(1);
                //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
                Document oDocument = new Document(oSize);

                // Creamos el objeto en el cual haremos la inserción
                FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
                PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
                oDocument.Open();

                //El contenido del pdf, aqui se hace la escritura del contenido
                PdfContentByte oPDF = oWriter.DirectContent;

                //Propiedades de nuestra fuente a insertar
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                oPDF.SetColorFill(BaseColor.BLACK);
                oPDF.SetFontAndSize(bf, 10);

                //Se abre el flujo para escribir el texto
                oPDF.BeginText();

                //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

                oPDF.ShowTextAligned(1, re_numero, Convert.ToInt32(Recursos.ReciboOtros.NoRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.NoRecibo_Y), 0);
                oPDF.ShowTextAligned(1, re_observacion, Convert.ToInt32(Recursos.ReciboOtros.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Nombre_Y), 0);
                oPDF.ShowTextAligned(1, re_fecha, Convert.ToInt32(Recursos.ReciboOtros.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Fecha_Y), 0);
                oPDF.ShowTextAligned(1, Clave, Convert.ToInt32(Recursos.ReciboOtros.Clave_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Clave_Y), 0);
                oPDF.ShowTextAligned(1, servicio, Convert.ToInt32(Recursos.ReciboOtros.Concepto_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.Concepto_Y), 0);
                oPDF.ShowTextAligned(1, re_total_rec, Convert.ToInt32(Recursos.ReciboOtros.TotalRecibo_X), oSize.Height - Convert.ToInt32(Recursos.ReciboOtros.TotalRecibo_Y), 0);

                oPDF.EndText();

                //crea una nueva pagina y agrega el pdf original
                PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
                oPDF.AddTemplate(page, 0, 0);

                // Cerramos los objetos utilizados
                oDocument.Close();
                oFS.Close();
                oWriter.Close();
                oReader.Close();
            }

            return 1;
        }

        //OBTENER ESTADO DEL CAJERO PARA REPORTE DE ARQUEO DESPUES DE CIERRE
        [HttpPost]
        public JsonResult GetEstadoCajero(string Fecha, int SecuenciaE)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GetEstadoCajero(Fecha, SecuenciaE), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Cajero> list = new List<Cajero>();
                Cajero obj = new Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //OBTIENE LOS DATOS PARA EL REPORTE DE ARQUEO
        [HttpPost]
        public JsonResult GetDatosReporteArqueo(string fecha)
        {
            try
            {
                CajaRepository CajaRep = new CajaRepository();
                if (ModelState.IsValid)
                {
                    CajaRep.GetDatosReporteArqueo(fecha);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(CajaRep.GetDatosReporteArqueo(fecha), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(fecha, JsonRequestBehavior.AllowGet);
            }
        }

        //GENERA PDF DEL REPORTE DE ARQUEO CAJA

        public int GeneraPdfReporteArqueo(ReporteArqueo reporteArqueo)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["FormatoArqueoCaja"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["FormatoArqueoCaja1"];

            //Objeto para leer el pdf original
            PdfReader oReader = new PdfReader(pathPDF);
            //Objeto que tiene el tamaño de nuestro documento
            Rectangle oSize = oReader.GetPageSizeWithRotation(1);
            //documento de itextsharp para realizar el trabajo asignandole el tamaño del original
            Document oDocument = new Document(oSize);

            // Creamos el objeto en el cual haremos la inserción
            FileStream oFS = new FileStream(pathPDF2, FileMode.Create, FileAccess.Write);
            PdfWriter oWriter = PdfWriter.GetInstance(oDocument, oFS);
            oDocument.Open();

            //El contenido del pdf, aqui se hace la escritura del contenido
            PdfContentByte oPDF = oWriter.DirectContent;

            //Propiedades de nuestra fuente a insertar
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 10);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string NombreCajero = Convert.ToString(reporteArqueo.NombreCajero);// Convert.ToString(Pagare.Valor);
            string fecha = Convert.ToString(reporteArqueo.CA_Fecha);
            string EstadoCaja = Convert.ToString(reporteArqueo.EstadoCajero);
            string InicioDia = Convert.ToString(reporteArqueo.CA_Valor_Inicial_Dia);
            string RecibidoEfectivo = Convert.ToString(reporteArqueo.CA_Valor_Recib);
            string EntregadoEfectivo = Convert.ToString(reporteArqueo.CA_Valor_Entrega);
            string SupervisorEntregado = Convert.ToString(reporteArqueo.CA_Valor_Entrega_Supervisor);
            string SupervisorRecibido = Convert.ToString(reporteArqueo.CA_Valor_Recib_Supervisor);
            string SaldoCaja = Convert.ToString(reporteArqueo.SaldoEnCaja);
            string B1 = Convert.ToString(reporteArqueo.CA_B_1);
            string B2 = Convert.ToString(reporteArqueo.CA_B_2);
            string B5 = Convert.ToString(reporteArqueo.CA_B_5);
            string B10 = Convert.ToString(reporteArqueo.CA_B_10);
            string B20 = Convert.ToString(reporteArqueo.CA_B_20);
            string B50 = Convert.ToString(reporteArqueo.CA_B_50);
            string B100 = Convert.ToString(reporteArqueo.CA_B_100);
            string B500 = Convert.ToString(reporteArqueo.CA_B_500);
            string M1 = Convert.ToString(reporteArqueo.CA_M_1);
            string M2 = Convert.ToString(reporteArqueo.CA_M_2);
            string M5 = Convert.ToString(reporteArqueo.CA_M_5);
            string M10 = Convert.ToString(reporteArqueo.CA_M_10);
            string M20 = Convert.ToString(reporteArqueo.CA_M_20);
            string M50 = Convert.ToString(reporteArqueo.CA_M_50);
            string TotalBilletes = Convert.ToString(reporteArqueo.TotalBilletes);
            string TotalMonedas = Convert.ToString(reporteArqueo.TotalMonedas);
            string SaldoCaja2 = Convert.ToString(reporteArqueo.SaldoEnCaja);
            string TotalArqueo = Convert.ToString(reporteArqueo.TotalArqueo);
            string Faltante = Convert.ToString(reporteArqueo.Faltante);
            string Sobrante = Convert.ToString(reporteArqueo.Sobrante);

            string TB1 = Convert.ToString(reporteArqueo.Tot_B1);
            string TB2 = Convert.ToString(reporteArqueo.Tot_B2);
            string TB5 = Convert.ToString(reporteArqueo.Tot_B5);
            string TB10 = Convert.ToString(reporteArqueo.Tot_B10);
            string TB20 = Convert.ToString(reporteArqueo.Tot_B20);
            string TB50 = Convert.ToString(reporteArqueo.Tot_B50);
            string TB100 = Convert.ToString(reporteArqueo.Tot_B100);
            string TB500 = Convert.ToString(reporteArqueo.Tot_B500);

            string TM1 = Convert.ToString(reporteArqueo.Tot_M1);
            string TM2 = Convert.ToString(reporteArqueo.Tot_M2);
            string TM5 = Convert.ToString(reporteArqueo.Tot_M5);
            string TM10 = Convert.ToString(reporteArqueo.Tot_M10);
            string TM20 = Convert.ToString(reporteArqueo.Tot_M20);
            string TM50 = Convert.ToString(reporteArqueo.Tot_M50);

            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

            oPDF.ShowTextAligned(1, NombreCajero, Convert.ToInt32(Recursos.ReporteArqueo.NombreCajero_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.NombreCajero_Y), 0);
            oPDF.ShowTextAligned(1, fecha, Convert.ToInt32(Recursos.ReporteArqueo.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.Fecha_Y), 0);
            oPDF.ShowTextAligned(1, EstadoCaja, Convert.ToInt32(Recursos.ReporteArqueo.EstadoCajero_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.EstadoCajero_Y), 0);
            oPDF.ShowTextAligned(1, InicioDia, Convert.ToInt32(Recursos.ReporteArqueo.InicioDia_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.InicioDia_Y), 0);
            oPDF.ShowTextAligned(1, RecibidoEfectivo, Convert.ToInt32(Recursos.ReporteArqueo.RecibidoEfectivo_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.RecibidoEfectivo_Y), 0);
            oPDF.ShowTextAligned(1, EntregadoEfectivo, Convert.ToInt32(Recursos.ReporteArqueo.EntregadoEfectivo_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.EntregadoEfectivo_Y), 0);
            oPDF.ShowTextAligned(1, SupervisorEntregado, Convert.ToInt32(Recursos.ReporteArqueo.EntregadoTesorero_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.EntregadoTesorero_Y), 0);
            oPDF.ShowTextAligned(1, SupervisorRecibido, Convert.ToInt32(Recursos.ReporteArqueo.RecibidoTesorero_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.RecibidoTesorero_Y), 0);
            oPDF.ShowTextAligned(1, SaldoCaja, Convert.ToInt32(Recursos.ReporteArqueo.SaldoCaja_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.SaldoCaja_Y), 0);

            //BILLETES
            oPDF.ShowTextAligned(1, B1, Convert.ToInt32(Recursos.ReporteArqueo.B1_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B1_Y), 0);
            oPDF.ShowTextAligned(1, B2, Convert.ToInt32(Recursos.ReporteArqueo.B2_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B2_Y), 0);
            oPDF.ShowTextAligned(1, B5, Convert.ToInt32(Recursos.ReporteArqueo.B5_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B5_Y), 0);
            oPDF.ShowTextAligned(1, B10, Convert.ToInt32(Recursos.ReporteArqueo.B10_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B10_Y), 0);
            oPDF.ShowTextAligned(1, B20, Convert.ToInt32(Recursos.ReporteArqueo.B20_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B20_Y), 0);
            oPDF.ShowTextAligned(1, B50, Convert.ToInt32(Recursos.ReporteArqueo.B50_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B50_Y), 0);
            oPDF.ShowTextAligned(1, B100, Convert.ToInt32(Recursos.ReporteArqueo.B100_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B100_Y), 0);
            oPDF.ShowTextAligned(1, B500, Convert.ToInt32(Recursos.ReporteArqueo.B500_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.B500_Y), 0);
            oPDF.ShowTextAligned(1, TotalBilletes, Convert.ToInt32(Recursos.ReporteArqueo.TotalBillete_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TotalBillete_Y), 0);

            //VALOR DE BILLETES
            oPDF.ShowTextAligned(1, TB1, Convert.ToInt32(Recursos.ReporteArqueo.TB1_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB1_Y), 0);
            oPDF.ShowTextAligned(1, TB2, Convert.ToInt32(Recursos.ReporteArqueo.TB2_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB2_Y), 0);
            oPDF.ShowTextAligned(1, TB5, Convert.ToInt32(Recursos.ReporteArqueo.TB5_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB5_Y), 0);
            oPDF.ShowTextAligned(1, TB10, Convert.ToInt32(Recursos.ReporteArqueo.TB10_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB10_Y), 0);
            oPDF.ShowTextAligned(1, TB20, Convert.ToInt32(Recursos.ReporteArqueo.TB20_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB20_Y), 0);
            oPDF.ShowTextAligned(1, TB50, Convert.ToInt32(Recursos.ReporteArqueo.TB50_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB50_Y), 0);
            oPDF.ShowTextAligned(1, TB100, Convert.ToInt32(Recursos.ReporteArqueo.TB100_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB100_Y), 0);
            oPDF.ShowTextAligned(1, TB500, Convert.ToInt32(Recursos.ReporteArqueo.TB500_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TB500_Y), 0);

            //MONEDAS
            oPDF.ShowTextAligned(1, M1, Convert.ToInt32(Recursos.ReporteArqueo.M1_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M1_Y), 0);
            oPDF.ShowTextAligned(1, M2, Convert.ToInt32(Recursos.ReporteArqueo.M2_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M2_Y), 0);
            oPDF.ShowTextAligned(1, M5, Convert.ToInt32(Recursos.ReporteArqueo.M5_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M5_Y), 0);
            oPDF.ShowTextAligned(1, M10, Convert.ToInt32(Recursos.ReporteArqueo.M10_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M10_Y), 0);
            oPDF.ShowTextAligned(1, M20, Convert.ToInt32(Recursos.ReporteArqueo.M20_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M20_Y), 0);
            oPDF.ShowTextAligned(1, M50, Convert.ToInt32(Recursos.ReporteArqueo.M50_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.M50_Y), 0);
            oPDF.ShowTextAligned(1, TotalMonedas, Convert.ToInt32(Recursos.ReporteArqueo.TotalMoneda_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TotalMoneda_Y), 0);

            //VALOR DE MONEDAS
            oPDF.ShowTextAligned(1, TM1, Convert.ToInt32(Recursos.ReporteArqueo.TM1_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM1_Y), 0);
            oPDF.ShowTextAligned(1, TM2, Convert.ToInt32(Recursos.ReporteArqueo.TM2_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM2_Y), 0);
            oPDF.ShowTextAligned(1, TM5, Convert.ToInt32(Recursos.ReporteArqueo.TM5_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM5_Y), 0);
            oPDF.ShowTextAligned(1, TM10, Convert.ToInt32(Recursos.ReporteArqueo.TM10_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM10_Y), 0);
            oPDF.ShowTextAligned(1, TM20, Convert.ToInt32(Recursos.ReporteArqueo.TM20_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM20_Y), 0);
            oPDF.ShowTextAligned(1, TM50, Convert.ToInt32(Recursos.ReporteArqueo.TM50_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TM50_Y), 0);

            //Totales
            oPDF.ShowTextAligned(1, TotalArqueo, Convert.ToInt32(Recursos.ReporteArqueo.TotalArqueo_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.TotalArqueo_Y), 0);
            oPDF.ShowTextAligned(1, SaldoCaja2, Convert.ToInt32(Recursos.ReporteArqueo.SaldoCaja2_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.SaldoCaja2_Y), 0);
            oPDF.ShowTextAligned(1, Faltante, Convert.ToInt32(Recursos.ReporteArqueo.Faltante_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.Faltante_Y), 0);
            oPDF.ShowTextAligned(1, Sobrante, Convert.ToInt32(Recursos.ReporteArqueo.Sobrante_X), oSize.Height - Convert.ToInt32(Recursos.ReporteArqueo.Sobrante_Y), 0);

            oPDF.EndText();

            //crea una nueva pagina y agrega el pdf original
            PdfImportedPage page = oWriter.GetImportedPage(oReader, 1);
            oPDF.AddTemplate(page, 0, 0);

            // Cerramos los objetos utilizados
            oDocument.Close();
            oFS.Close();
            oWriter.Close();
            oReader.Close();

            return 1;
        }


    }
}

