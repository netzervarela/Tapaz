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
using CrystalDecisions.CrystalReports.Engine;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace appcitas.Controllers
{
    public class PrestamosController : Controller
    {
        // GET: Prestamos

        Prestamos pPrestamo = new Prestamos();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prestamo()
        {
            return View();
        }

        class Conv
        {
            public string enletras(string num)
            {
                string res, dec = "";
                Int64 entero;
                int decimales;
                double nro;

                try
                {
                    nro = Convert.ToDouble(num);
                }
                catch
                {
                    return "";
                }

                entero = Convert.ToInt64(Math.Truncate(nro));
                decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
                if (decimales > 0)
                {
                    dec = " CON " + decimales.ToString() + "/100";
                }

                res = toText(Convert.ToDouble(entero)) + dec;
                return res;
            }


            public string MesAText(int Nume)
            {
                String[] Mes = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                return Mes[Nume - 1];

            }
            private string toText(double value)
            {
                string Num2Text = "";
                value = Math.Truncate(value);
                if (value == 0) Num2Text = "CERO";
                else if (value == 1) Num2Text = "UNO";
                else if (value == 2) Num2Text = "DOS";
                else if (value == 3) Num2Text = "TRES";
                else if (value == 4) Num2Text = "CUATRO";
                else if (value == 5) Num2Text = "CINCO";
                else if (value == 6) Num2Text = "SEIS";
                else if (value == 7) Num2Text = "SIETE";
                else if (value == 8) Num2Text = "OCHO";
                else if (value == 9) Num2Text = "NUEVE";
                else if (value == 10) Num2Text = "DIEZ";
                else if (value == 11) Num2Text = "ONCE";
                else if (value == 12) Num2Text = "DOCE";
                else if (value == 13) Num2Text = "TRECE";
                else if (value == 14) Num2Text = "CATORCE";
                else if (value == 15) Num2Text = "QUINCE";
                else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
                else if (value == 20) Num2Text = "VEINTE";
                else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
                else if (value == 30) Num2Text = "TREINTA";
                else if (value == 40) Num2Text = "CUARENTA";
                else if (value == 50) Num2Text = "CINCUENTA";
                else if (value == 60) Num2Text = "SESENTA";
                else if (value == 70) Num2Text = "SETENTA";
                else if (value == 80) Num2Text = "OCHENTA";
                else if (value == 90) Num2Text = "NOVENTA";
                else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
                else if (value == 100) Num2Text = "CIEN";
                else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
                else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
                else if (value == 500) Num2Text = "QUINIENTOS";
                else if (value == 700) Num2Text = "SETECIENTOS";
                else if (value == 900) Num2Text = "NOVECIENTOS";
                else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
                else if (value == 1000) Num2Text = "MIL";
                else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
                else if (value < 1000000)
                {
                    Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                    if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
                }

                else if (value == 1000000) Num2Text = "UN MILLON";
                else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
                else if (value < 1000000000000)
                {
                    Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                    if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
                }

                else if (value == 1000000000000) Num2Text = "UN BILLON";
                else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

                else
                {
                    Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                    if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
                return Num2Text;

            }

        }


        //GENERA CONTRATO DE PRESTAMO
        [HttpPost]
        public int GeneraFormato(int PrestamoId)
        {
            
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            List<ContratoPrestamo> lista = new List<ContratoPrestamo>();
            lista = PrestamoRep.GeneraFormato(PrestamoId);

            string Nombre = lista[0].Nombre;
            string Identidad = lista[0].identidad;
            string CLI_Direccion = lista[0].CLI_Direccion;
            decimal Monto = lista[0].Monto;
            string Fecha = lista[0].Fec;
            int PRES_Plazo_Meses = lista[0].PRES_Plazo_Meses;
            int NumCuotas = lista[0].NumCuotas;
            decimal PRES_Porc_Interes = lista[0].PRES_Porc_Interes;
            string DES_Descripcion = lista[0].DES_Descripcion;
            string GAR_Descripcion = lista[0].GAR_Descripcion;

            DateTime Fec = DateTime.Parse(Fecha);
            int Dia = Convert.ToInt32(Fec.Day);
            int DiaMes = Convert.ToInt32(Fec.Month);
            string Anio = Convert.ToString(Fec.Year);
            
            Conv Numer = new Conv();

            String Mes = Numer.MesAText(DiaMes);
            string año = Numer.enletras(Anio);
            año = año.ToLower();

            string NombreArchivo = "ContratoPrestamo" + PrestamoId + ".pdf";
            string carpeta = @"C:\inetpub\wwwroot\SistemaFinanciero\Prestamos\";
            //string carpeta = @"C:\FERNANDOG\Desarrollos\CORE FINANCIERO\Sistema Financiero\Sistema Financiero\appcitas\Prestamos\";
            Document doc = new Document(iTextSharp.text.PageSize.LEGAL, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
            iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
            addLogo = iTextSharp.text.Image.GetInstance("C:/inetpub/wwwroot/SistemaFinanciero/imgs" + "/logo_cr.png");
            doc.Open();
            Font LineBreak = FontFactory.GetFont("COURIER", size: 11);
             Font Negritas = FontFactory.GetFont(FontFactory.COURIER_BOLD, 11);
            addLogo.ScaleToFit(128, 37);
            addLogo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            doc.Add(addLogo);
            Paragraph Titulo = new Paragraph(string.Format("CONTRATO DE PRESTAMO No. " + PrestamoId), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 16, iTextSharp.text.Font.BOLD));
            //Titulo.SpacingBefore = 200;
            //Titulo.SpacingAfter = 0;
            Titulo.Alignment = 1; //0-Left, 1 middle,2 Right
            doc.Add(Titulo);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph Parrafo1 = new Paragraph(String.Format("Nosotros; de una parte ESAÚ SALOMÓN GARCIA ARGUETA, con identidad No. 1218-1993-00277, mayor de edad, abogado y administrador industrial y de negocios, domicilio en el Municipio de Santiago Puringla, La Paz, actuando en representación de la Sociedad Financiera UNICREDIT, y que en lo sucesivo se llamará El Prestamista, y el Señor. " + Nombre + ",mayor de edad, con tarjeta de identidad No. " + Identidad + ", con domicilio en " + CLI_Direccion + ", que en adelante se llamará El Prestatario; hemos convenido celebrar, y en efecto celebramos, un contrato de préstamo que se regirá por las estipulaciones siguientes:"), LineBreak);
            //Parrafo1.(1, 1);
            Parrafo1.Alignment =Element.ALIGN_JUSTIFIED;
            doc.Add(Parrafo1);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("PRIMERO: DATOS DEL PRESTAMO", Negritas));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("Monto del Préstamo:               L. " + Monto , LineBreak ));
            doc.Add(new Paragraph("\nPlazo:                            " + PRES_Plazo_Meses + " Meses", LineBreak));
            doc.Add(new Paragraph("\nCantidad de cuotas:               " + NumCuotas, LineBreak));
            doc.Add(new Paragraph("\nAmortización:                     " + "Cuota fija Mensual", LineBreak));
            doc.Add(new Paragraph("\nMonto de las cuotas:              " + "según plan de pago", LineBreak));
            doc.Add(new Paragraph("\nTasa de interés:                  " + PRES_Porc_Interes + " % anual", LineBreak));
            doc.Add(new Paragraph("\nObjetivo del préstamo:            " + DES_Descripcion, LineBreak));
            doc.Add(new Paragraph("\nGarantía:                         " + GAR_Descripcion, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("SEGUNDO: CONDICIONES GENERALES", Negritas));
            Paragraph Parrafo2 = new Paragraph(String.Format("a.) Lugar de Pago: El prestatario pagara el préstamo en las oficinas de UNICREDIT ubicadas en el barrio el centro, del Municipio de Santiago Puringla, La Paz. \n\nb.) Garantías: La garantía hipotecaria se realizará mediante el traspaso de un inmueble que se encuentre a favor del deudor. La garantía Fiduciaria se realizará con el aval de uno o más fiadores solidarios que compartirán responsabilidad. La garantía prendaria será mediante el traspaso de cualquier bien mueble previo avalúo. La garantía automática se utilizará cuando el deudor tenga un respaldo en ahorros a la vista o depósitos a plazo fijo. \n\nc.) Mora: El prestatario incurre en mora si deja de pagar en el plazo establecido en el plan de pago, la multa diaria corresponde al 1% sobre el saldo en mora, en caso de no llegar a un acuerdo de pago tiene derecho el prestamista a declarar vencido el préstamo y exigir judicialmente el pago total del mismo, con sus respectivos intereses, honorarios del abogado y gastos incurridos."), LineBreak);
            Parrafo2.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(Parrafo2);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("TERCERO: CONDICIONES ESPECIALES", Negritas));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph Parrafo3 = new Paragraph(String.Format("El presente contrato podrá ser modificado por acuerdo de ambas partes y sus modificaciones deberán constar por escrito. \n\nCUARTO: Ambos contratantes declaran: Estar de acuerdo en todas y cada una de las cláusulas de este contrato, y se obligan a cumplirlo fielmente. \n\nPara constancia firman este contrato en Santiago Puringla, La Paz, a los " + Dia + " días del mes de " + Mes + " del año " + Anio + "."), LineBreak);
            Parrafo3.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(Parrafo3);
            doc.Add(new Paragraph("\n\n\n\n\n", LineBreak));
            doc.Add(new Paragraph("    _________________________                                                       __________________________"));
            Paragraph Firma1 = new Paragraph(String.Format("    POR LA FINANCIERA                                  " + Nombre), LineBreak);
            doc.Add(Firma1);
            Paragraph Firma2 = new Paragraph(String.Format("    GERENTE GENERAL                                             " + Identidad), LineBreak);
            doc.Add(Firma2);
            doc.Close();
            return 1 ;
        }

        [HttpPost]
        public int EstudioSocioEconomico1(int PrestamoId)
        {
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            List<EstudioSocioEconomico> lista = new List<EstudioSocioEconomico>();
            lista = PrestamoRep.EstudioSocioEconomico(PrestamoId);

            string Nombre = lista[0].Nombre;
            string CLI_Direccion = lista[0].CLI_Direccion;
            string CLI_Cel = lista[0].CLI_Cel;
            decimal ESE_NumeroDependientes = lista[0].ESE_NumeroDependientes;
            decimal ESE_NegocioPropio = lista[0].ESE_NegocioPropio;
            decimal ESE_Salario = lista[0].ESE_Salario;
            decimal ESE_ActividadAgropecuaria = lista[0].ESE_ActividadAgropecuaria;
            decimal ESE_Otros_Ingresos = lista[0].ESE_Otros_Ingresos;
            decimal ESE_Renta = lista[0].ESE_Renta;
            decimal ESE_ServiciosPublicos = lista[0].ESE_ServiciosPublicos;
            decimal ESE_Prestamos = lista[0].ESE_Prestamos;
            decimal ESE_Transporte = lista[0].ESE_Transporte;
            decimal ESE_Alimentacion = lista[0].ESE_Alimentacion;
            decimal ESE_Vestuario = lista[0].ESE_Vestuario;
            decimal ESE_Otros_Egresos = lista[0].ESE_Otros_Egresos;
            string ESE_Observaciones = lista[0].ESE_Observaciones;

            string NombreArchivo = "SocioEconomico" + PrestamoId + ".pdf";
            string carpeta = @"C:\Users\Fernando Giron\Desktop\FORMATOS\";
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
            iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
            addLogo = iTextSharp.text.Image.GetInstance("C:/Users/Fernando Giron/Desktop/FORMATOS" + "/logo_cr.png");
            doc.Open();
            Font LineBreak = FontFactory.GetFont("TIMES_ROMAN", size: 12);
            Font Datos = FontFactory.GetFont("TIMES_ROMAN", size: 8);
            Font Parrafos = FontFactory.GetFont("TIMES_ROMAN", size: 12);
            Font Negritas = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
            PdfPTable tabla = new PdfPTable(5);
            addLogo.ScaleToFit(128, 37);
            addLogo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
            doc.Add(addLogo);
            Paragraph Titulo = new Paragraph(string.Format("                ESTUDIO SOCIOECONOMICO"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16, iTextSharp.text.Font.BOLD));
            Titulo.Alignment = 1; //0-Left, 1 middle,2 Right
            doc.Add(Titulo);
            Paragraph Direccion = new Paragraph(String.Format("Bo. El Centro, Calle Principal, Santiago Puringla, La Paz"), Datos);
            Direccion.Alignment = Element.ALIGN_LEFT;
            doc.Add(Direccion);
            Paragraph Contacto = new Paragraph(String.Format("Cel. 9841-9334 | Email: unicredithn@gmail.com"), Datos);
            Contacto.Alignment = Element.ALIGN_LEFT;
            doc.Add(Contacto);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo = new Paragraph(String.Format("DATOS DEL SOLICITANTE"), Negritas);
            SubTitulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(SubTitulo);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph Parrafo1 = new Paragraph(String.Format("NOMBRE COMPLETO: " + Nombre + "\n\nDIRECCIÓN: " + CLI_Direccion + "                                                               CELULAR :" + CLI_Cel), Parrafos);
            Parrafo1.Alignment = Element.ALIGN_LEFT;
            doc.Add(Parrafo1);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo2 = new Paragraph(String.Format("              INGRESOS MENSUALES                                                                   EGRESOS MENSUALES"), Negritas);
            doc.Add(SubTitulo2);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph(String.Format("¿NUMERO DE PERSONAS QUE DEPENDEN ECONOMICAMENTE DE USTED? " + ESE_NumeroDependientes), Parrafos));
            tabla.AddCell(new Paragraph("NEGOCIO PROPIO"));
            doc.Add(tabla);
            //doc.Add(new Paragraph(String.Format("\n\nNEGOCIO PROPIO:                               L. " + ESE_NegocioPropio + "                    RENTA                                           L." + ESE_Renta), Parrafos));
            //doc.Add(new Paragraph(String.Format("\n\nSALARIO:                                                 L. " + ESE_Salario + "                     SERVICIOS PUBLICOS                  L." + ESE_ServiciosPublicos), Parrafos));
            doc.Close();
            return 1;
        }

        //GENERA SOLICITUD DE PRESTAMO
        [HttpPost]
        public int SolicitudCredito1(int PrestamoId)
        {
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            List<SolicitudCredito> lista = new List<SolicitudCredito>();
            lista = PrestamoRep.SolicitudCredito(PrestamoId);

            string Nombre = lista[0].Nombre;
            string CLI_Identidad = lista[0].CLI_Identidad;
            string CLI_Direccion = lista[0].CLI_Direccion;
            string Estado_Civil = lista[0].Estado_Civil;
            string CLI_Cel = lista[0].CLI_Cel;
            decimal PRES_Mto_Solicitado = lista[0].PRES_Mto_Solicitado;
            decimal PRES_Porc_Interes = lista[0].PRES_Porc_Interes;
            decimal PRES_Mto_Aprobado = lista[0].PRES_Mto_Aprobado;
            int PRES_Plazo_Meses = lista[0].PRES_Plazo_Meses;
            string FormaPago = lista[0].FormaPago;
            string DES_Descripcion = lista[0].DES_Descripcion;
            string GAR_Descripcion = lista[0].GAR_Descripcion;
            string PRES_Observaciones = lista[0].PRES_Observaciones;
            string Fecha = lista[0].Fecha;
            string PRES_Fecha_Aprob = lista[0].PRES_Fecha_Aprob;
            string PRO_Descripcion = lista[0].PRO_Descripcion;
            string CLI_Direc_Trabajo = lista[0].CLI_Direc_Trabajo;
            var NombreRef1 = "";
            var CelularRef1 = "";
            var NombreRef2 = "";
            var CelularRef2 = "";

            foreach (var registro in lista)
            {
                if(registro.REF_Num ==1)
                {
                    NombreRef1 = registro.REF_Nombre;
                    CelularRef1 = registro.REF_Celular;
                }
                if (registro.REF_Num == 2)
                {
                   NombreRef2 = registro.REF_Nombre;
                   CelularRef2 = registro.REF_Celular;
                }
            }
            string NombreArchivo = "SolicitudCredito" + PrestamoId + ".pdf";
            //string carpeta = @"C:\inetpub\wwwroot\SistemaFinanciero\Prestamos\";
            string carpeta = @"C:\FERNANDOG\Desarrollos\CORE FINANCIERO\Sistema Financiero\Sistema Financiero\appcitas\Prestamos\";
            Document doc = new Document(iTextSharp.text.PageSize.LEGAL, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
            iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
            addLogo = iTextSharp.text.Image.GetInstance("C:/Users/Fernando Giron/Desktop/FORMATOS" + "/logo_cr.png");
            //addLogo = iTextSharp.text.Image.GetInstance("C:/inetpub/wwwroot/SistemaFinanciero/imgs" + "/logo_cr.png");
            doc.Open();
            Font LineBreak = FontFactory.GetFont("COURIER", size: 12);
            Font Negritas = FontFactory.GetFont(FontFactory.COURIER_BOLD, 12);
            addLogo.ScaleToFit(128, 37);
            addLogo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            doc.Add(addLogo);
            Paragraph Titulo = new Paragraph(string.Format("SOLICITUD DE CRÉDITO"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 16, iTextSharp.text.Font.BOLD));
            Paragraph SubTitulo = new Paragraph(string.Format("DATOS DEL SOLICITANTE"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 15, iTextSharp.text.Font.BOLD));
            //Titulo.SpacingBefore = 200;
            //Titulo.SpacingAfter = 0;
            Titulo.Alignment = 1; //0-Left, 1 middle,2 Right
            SubTitulo.Alignment = 1;
            doc.Add(Titulo);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(SubTitulo);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph DireccionEmpresa = new Paragraph(String.Format("SANTIAGO PURINGLA, LA PAZ                       FECHA: " + Fecha), LineBreak);
            //Parrafo1.(1, 1);
            DireccionEmpresa.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(DireccionEmpresa);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("NOMBRE COMPLETO: " + Nombre, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("IDENTIDAD: " + CLI_Identidad + "                        ESTADO CIVIL: " + Estado_Civil, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("DOMICILIO: " + CLI_Direccion, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("DIRECCIÓN DE TRABAJO: " + CLI_Direc_Trabajo +                          "        TELEFONO: ", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("PROFESIÓN U OFICIO: " + PRO_Descripcion +                          "                   CELULAR: " + CLI_Cel, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo2 = new Paragraph(String.Format("INFORMACIÓN DEL CRÉDITO"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 15, iTextSharp.text.Font.BOLD));
            SubTitulo2.Alignment = 1;
            doc.Add(SubTitulo2);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("MONTO SOLICITADO: L. " + PRES_Mto_Solicitado, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("TASA DE INTERES: " + PRES_Porc_Interes + " %" + "                        PLAZO: " + PRES_Plazo_Meses + " Meses", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("TIPO DE GARANTÍA: " + GAR_Descripcion + "                   FORMA DE PAGO: " + FormaPago , LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("CUOTA MENSUAL: según plan de pago               FECHA DESEMBOLSO: "+ PRES_Fecha_Aprob, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("MONTO APROBADO: L. "+PRES_Mto_Aprobado + "                     CRÉDITO NUEVO:", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("OBJETIVO DEL PRÉSTAMO: "+ DES_Descripcion, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo3 = new Paragraph(String.Format("OBSERVACIONES"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 15, iTextSharp.text.Font.BOLD));
            SubTitulo3.Alignment = 1;
            doc.Add(SubTitulo3);
            doc.Add(new Paragraph(PRES_Observaciones, LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph(""));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo4 = new Paragraph(String.Format("REFERENCIAS"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 15, iTextSharp.text.Font.BOLD));
            SubTitulo4.Alignment = 1;
            doc.Add(SubTitulo4);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("NOMBRE: " +  NombreRef1 +"                            CELULAR: " + CelularRef1 , LineBreak));
            doc.Add(new Paragraph("NOMBRE: " +  NombreRef2 +"                            CELULAR: " + CelularRef2 , LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph SubTitulo5 = new Paragraph(String.Format("FIRMAS"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 15, iTextSharp.text.Font.BOLD));
            SubTitulo5.Alignment = 1;
            doc.Add(SubTitulo5);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("    _________________________                                                       __________________________"));
            Paragraph Firma1 = new Paragraph(String.Format("               SOLICITANTE                                                                                    ENCARGADO"));
            doc.Add(Firma1);
            //Paragraph Firma2 = new Paragraph(String.Format("    ENCARGADO"), LineBreak);
            //doc.Add(Firma2);
            doc.Close();
            return 1;
        }


        //GENERA PAGARE DE PRESTAMO
        [HttpPost]
        public int GeneraPagare(int PrestamoId)
        {
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            List<PagarePrestamo> lista = new List<PagarePrestamo>();
            lista = PrestamoRep.GeneraPagare(PrestamoId);

            //DateTime Fech = Today.now;
            string NombreCliente = lista[0].Nombre;
            string IdentidadCli = lista[0].identidad;
            string CLI_Direccion = lista[0].CLI_Direccion;
            decimal MontoPag = lista[0].Monto;
            string Fecha = lista[0].Fec;
            int Tipo = lista[0].Tipo;
            DateTime Fec = DateTime.Parse(Fecha);
            int Dia = Convert.ToInt32(Fec.Day);
            int DiaMes = Convert.ToInt32(Fec.Month);
            string Anio = Convert.ToString(Fec.Year);

            String NomAval = " ";
            String IdAval = " ";
            String NomAval2 = " ";
            String IdAval2 = " ";

            //DateTime FechaHoy = DateTime.Now.ToString.

            //int PRES_Plazo_Meses = lista[0].PRES_Plazo_Meses;
            //int NumCuotas = lista[0].NumCuotas;
            //decimal PRES_Porc_Interes = lista[0].PRES_Porc_Interes;
            //string DES_Descripcion = lista[0].DES_Descripcion;
            //string GAR_Descripcion = lista[0].GAR_Descripcion;


            Conv Numer = new Conv();

            String Numeros = Numer.enletras(MontoPag.ToString());
            String Mes = Numer.MesAText(DiaMes);
            string año = Numer.enletras(Anio);
            año = año.ToLower();
            string NombreArchivo = "Pagare" + PrestamoId + ".pdf";
            string carpeta = @"C:\inetpub\wwwroot\SistemaFinanciero\Prestamos\";
            //string carpeta = @"C:\FERNANDOG\Desarrollos\CORE FINANCIERO\Sistema Financiero\Sistema Financiero\appcitas\Prestamos\";
            Document doc = new Document(iTextSharp.text.PageSize.LEGAL, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(carpeta + NombreArchivo, FileMode.Create));
            iTextSharp.text.Image addLogo = default(iTextSharp.text.Image);
            addLogo = iTextSharp.text.Image.GetInstance("C:/inetpub/wwwroot/SistemaFinanciero/imgs" + "/logo_cr.png");
            doc.Open();
            Font LineBreak = FontFactory.GetFont("COURIER", size: 11);
            Font Negritas = FontFactory.GetFont(FontFactory.COURIER_BOLD, 11);
            addLogo.ScaleToFit(128, 37);
            addLogo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            doc.Add(addLogo);
            Paragraph Titulo = new Paragraph(string.Format("PAGARÉ POR L." + MontoPag), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 16, iTextSharp.text.Font.BOLD));
            //Titulo.SpacingBefore = 200;
            //Titulo.SpacingAfter = 0;
            Titulo.Alignment = 1; //0-Left, 1 middle,2 Right
            doc.Add(Titulo);
            doc.Add(new Paragraph("\n", LineBreak));
            Paragraph Parrafo1 = new Paragraph(String.Format("Yo," + NombreCliente + "  mayor de edad, hondureño (a),  con identidad No. " + IdentidadCli + " con domicilio en: " + CLI_Direccion + " declaro que recibí y debo a la empresa UNICREDIT, la suma de L. " + MontoPag + " " + Numeros + " que pagaré incondicionalmente, a la vista y sin protesto, con simple requerimiento de pago en sus oficinas en Barrio el centro, calle principal, Santiago Puringla, La Paz, a más tardar el día *fecha del vencimiento de todo el préstamo* devengando un interés del 2% mensual, y en caso de mora se cobrará un recargo adicional del 36% anual sobre saldos vencidos y no pagados. En este mismo acepto que la empresa puede dar por vencido en forma anticipada mi Préstamo al tener TRES cuotas en mora. Para la ejecución y cumplimiento de la presente obligación, hago RENUNCIA DE DOMICILIO, y me someto expresamente al que la Empresa UNICREDIT designe. En fe de lo cual firmo el presente pagaré en el Municipio de Santiago Puringla, la paz, a los  " + Dia + " días del mes " + Mes + " del año " + año + "."), LineBreak);
            //Parrafo1.(1, 1);
            Parrafo1.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(Parrafo1);
            doc.Add(new Paragraph("\n", LineBreak));
            doc.Add(new Paragraph("\n\n\n\n\n", LineBreak));
            //doc.Add(new Paragraph("_________________________"));
            Paragraph Firma1 = new Paragraph(String.Format("   _________________________ \n Firma \n" + NombreCliente), LineBreak);
            Firma1.Alignment = Element.ALIGN_CENTER;
            doc.Add(Firma1);
            Paragraph Firma2 = new Paragraph(String.Format("Identidad No. " + IdentidadCli), LineBreak);
            Firma2.Alignment = Element.ALIGN_CENTER;
            doc.Add(Firma2);

            if (Tipo == 3)
            {
                Paragraph Titulo2 = new Paragraph(string.Format("\n\n GARANTÍA"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 16, iTextSharp.text.Font.BOLD));
                Titulo2.Alignment = 1;
                doc.Add(Titulo2);
                Paragraph Parrafo2 = new Paragraph(String.Format("Como aval solidario garantizo a la Empresa UNICREDIT incondicionalmente el pago inmediato a su vencimiento del capital total más los intereses devengados a la fecha de acuerdo a las condiciones establecidas en este PAGARÉ. Y así mismo renuncio a toda diligencia, protestas, avisos, así como a cualquier requerimiento a que tenga derecho y AUTORIZO  así mismo cualquier extensión de tiempo o renovación de esta obligación y deducción de cualquiera de mis cuentas que en calidad de ahorro o depósito me corresponden."), LineBreak);
                Parrafo2.Alignment = Element.ALIGN_JUSTIFIED;
                doc.Add(Parrafo2);
                doc.Add(new Paragraph("\n\n\n\n\n", LineBreak));
                doc.Add(new Paragraph("       _________________________                                                         __________________________"));
                doc.Add(new Paragraph("                       Firma Aval                                                                                            Firma Aval       "));
                doc.Add(new Paragraph("                " + NomAval + "                                                                               " + NomAval2 + " "));
                doc.Add(new Paragraph("         Identidad " + IdAval + "                                                                   Identidad " + IdAval2 + " "));
            }

            doc.Close();
            return 1;
        }




        //[HttpPost]
        //public ActionResult exportReport(int PrestamoId)
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Contrato definitivo.rpt"));
        //    rd.SetDatabaseLogon(appcitas.Content.Temp18.Use1, appcitas.Content.Temp18.Pas1);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        rd.SetParameterValue("@CodPrestamo", PrestamoId);
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);


        //        rd.PrintToPrinter(1, false, 0, 0);



        //        //return File(stream, "application/pdf", "Employee_list.pdf");
        //        return View();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        //[HttpPost]
        //public ActionResult SolicitudCredito(int PrestamoId)
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Solicitud_Crédito.rpt"));
        //    rd.SetDatabaseLogon(appcitas.Content.Temp18.Use1, appcitas.Content.Temp18.Pas1);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        rd.SetParameterValue("@CodPrestamo", PrestamoId);
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);

        //        rd.PrintToPrinter(1, true, 0, 0);

        //        //return File(stream, "application/pdf", "Employee_list.pdf");
        //        return View();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        ////[HttpPost]
        ////[ValidateInput(false)]
        //public ActionResult ReciboOtros(int recibo)
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Pago_Servicio.rpt"));
        //    rd.SetDatabaseLogon(appcitas.Content.Temp18.Use1, appcitas.Content.Temp18.Pas1);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        rd.SetParameterValue("@NunRecibo", recibo);
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);

        //        rd.PrintToPrinter(1, true, 0, 0);

        //        //return File(stream, "application/pdf", "Employee_list.pdf");
        //        return View();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        //public ActionResult ReciboPagoPrestamo(int recibo)
        //{
        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Pago_Cuota.rpt"));
        //    rd.SetDatabaseLogon(appcitas.Content.Temp18.Use1, appcitas.Content.Temp18.Pas1);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        rd.SetParameterValue("@NumRecibo", recibo);
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);

        //        rd.PrintToPrinter(1, true, 0, 0);

        //        //return File(stream, "application/pdf", "Employee_list.pdf");
        //        return View();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        [HttpPost]
        public JsonResult GetAll(int ClienteId)
        {
            
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            try
            {
                return Json(PrestamoRep.GetAllByTipo(ClienteId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Prestamos> list = new List<Prestamos>();
                Prestamos obj = new Prestamos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public JsonResult SaveData(Prestamos Prestam)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.Save(Prestam);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(Prestam, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(Prestam, JsonRequestBehavior.AllowGet);
            }

        }

        //CONTROLADOR VER PRESTAMOS EN BOTON DE ACCION
        [HttpPost]
        public JsonResult GetDatosPrestamos(int id)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.GetDatosPrestamos(id);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(PrestamoRep.GetDatosPrestamos(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(id, JsonRequestBehavior.AllowGet);
            }
        }

        //PROYECCION PLAN DE PAGO
        [HttpPost]
        public JsonResult GeneraPlanPago(decimal monto, decimal tasa, int plazo, int frecuency, string fecha, int tipo)
        {
            PrestamoRepository CitaRep = new PrestamoRepository();
            try
            {
                return Json(CitaRep.GeneraPlanPago(monto, tasa, plazo, frecuency, fecha, tipo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public int SolicitudCreditoImp(SolicitudPrestamo solCredit)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["SolicitudCredito"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["SolicitudCredito1"];

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
            //oPDF.SetFontAndSize(bf, 18);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            //string Valor = Convert.ToString(Econ.Monto);// Convert.ToString(Pagare.Valor);
            //oPDF.ShowTextAligned(1, Valor, Convert.ToInt32(Recursos.PagareNormal.ValorGeneral_X),
            //    oSize.Height - Convert.ToInt32(Recursos.PagareNormal.ValorGeneral_Y), 0);
            ////oPDF.EndText();



            oPDF.SetFontAndSize(bf, 11);
            //oPDF.BeginText();

            Moneda Mone = new Moneda();

            //string valorLetras = Mone.Convertir(Convert.ToString(), true);

            string Fecha = DateTime.Now.ToString("dd-MM-yyyy");
            string Identidad = solCredit.Identidad;// Pagare.Identidad;
            string Nombre = solCredit.Nombre;
            string Direccion = solCredit.Direccion;
            string MontoSol = Convert.ToString(solCredit.MontoSolicitado);
            string MontoAprob = Convert.ToString(solCredit.MontoAprobado);
            string Lugar = "Santiago Puringla, La Paz.";
            string EstadoCivil = solCredit.EstadoCivil;
            string Dirtrabajo = solCredit.dirTrabajo;
            string profesion = solCredit.Profesion;
            string Celular = solCredit.Celular;
            string telefono = solCredit.Telefono != null ? solCredit.Telefono : "33438465";
            string Interes = solCredit.TasaInteres;
            string plazo = solCredit.Plazo != null ? solCredit.Plazo + " Meses" : "12 Meses";
            string Garantia = solCredit.TipoGarantia;
            string FormaPago = solCredit.FormaPago;
            string cuota = solCredit.CuotaMensual != null ? solCredit.CuotaMensual : "1500";
            string FechaDesemb = solCredit.FechaDesembolso != null ? solCredit.FechaDesembolso : DateTime.Now.ToString("dd-MM-yyyy");
            string creditoNuevo = solCredit.CreditoNuevo != null ? solCredit.CreditoNuevo : "NO SE";
            string objetivo = solCredit.ObjetivoPrestamo;
            string observ = solCredit.Observaciones;
            string NomRef1 = solCredit.NomRef1;
            string NomRef2 = solCredit.NomRef2;
            string CelRef1 = solCredit.CelRef1;
            string CelRef2 = solCredit.CelRef2;

            //string Descripcion = "Interese a Deposito";
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);
            oPDF.ShowTextAligned(1, Lugar, Convert.ToInt32(Recursos.solicitudPrestamo.Lugar_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Lugar_Y), 0);
            oPDF.ShowTextAligned(1, Fecha, Convert.ToInt32(Recursos.solicitudPrestamo.Fecha_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Fecha_Y), 0);
            oPDF.ShowTextAligned(1, Identidad, Convert.ToInt32(Recursos.solicitudPrestamo.Identidad_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Identidad_Y), 0);
            oPDF.ShowTextAligned(1, Direccion, Convert.ToInt32(Recursos.solicitudPrestamo.Domicilio_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Domicilio_Y), 0);
            oPDF.ShowTextAligned(1, telefono, Convert.ToInt32(Recursos.solicitudPrestamo.Tel_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Tel_Y), 0);
            oPDF.ShowTextAligned(1, MontoSol, Convert.ToInt32(Recursos.solicitudPrestamo.MontoSol_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.MontoSol_Y), 0);
            oPDF.ShowTextAligned(1, Interes, Convert.ToInt32(Recursos.solicitudPrestamo.TasaInt_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.TasaInt_Y), 0);
            oPDF.ShowTextAligned(1, EstadoCivil, Convert.ToInt32(Recursos.solicitudPrestamo.EstadoCivil_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.EstadoCivil_Y), 0);
            oPDF.ShowTextAligned(1, Dirtrabajo, Convert.ToInt32(Recursos.solicitudPrestamo.DirTrabajo_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.DirTrabajo_Y), 0);
            oPDF.ShowTextAligned(1, profesion, Convert.ToInt32(Recursos.solicitudPrestamo.Profesion_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Profesion_Y), 0);
            oPDF.ShowTextAligned(1, FechaDesemb, Convert.ToInt32(Recursos.solicitudPrestamo.FecDesembolso_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.FecDesembolso_Y), 0);
            oPDF.ShowTextAligned(1, Celular, Convert.ToInt32(Recursos.solicitudPrestamo.Celular_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Celular_Y), 0);
            oPDF.ShowTextAligned(1, plazo, Convert.ToInt32(Recursos.solicitudPrestamo.Plazo_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Plazo_Y), 0);
            oPDF.ShowTextAligned(1, FormaPago, Convert.ToInt32(Recursos.solicitudPrestamo.FormaPago_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.FormaPago_Y), 0);
            oPDF.ShowTextAligned(1, cuota, Convert.ToInt32(Recursos.solicitudPrestamo.Cuota_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Cuota_Y), 0);
            oPDF.ShowTextAligned(1, MontoAprob, Convert.ToInt32(Recursos.solicitudPrestamo.MontoAprob_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.MontoAprob_Y), 0);
            oPDF.ShowTextAligned(1, creditoNuevo, Convert.ToInt32(Recursos.solicitudPrestamo.CreditoN_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.CreditoN_Y), 0);
            oPDF.ShowTextAligned(1, Garantia, Convert.ToInt32(Recursos.solicitudPrestamo.TipoGaran_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.TipoGaran_Y), 0);
            oPDF.ShowTextAligned(1, objetivo, Convert.ToInt32(Recursos.solicitudPrestamo.ObjetivoPres_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.ObjetivoPres_Y), 0);
            oPDF.ShowTextAligned(1, observ, Convert.ToInt32(Recursos.solicitudPrestamo.Observacion_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Observacion_Y), 0);
            oPDF.ShowTextAligned(1, NomRef1, Convert.ToInt32(Recursos.solicitudPrestamo.AvalNomb_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.AvalNomb_Y), 0);
            oPDF.ShowTextAligned(1, CelRef1, Convert.ToInt32(Recursos.solicitudPrestamo.AvalCel_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.AvalCel_Y), 0);
            oPDF.ShowTextAligned(1, NomRef1, Convert.ToInt32(Recursos.solicitudPrestamo.Nom_Aval2_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Nom_Aval2_Y), 0);
            oPDF.ShowTextAligned(1, CelRef1, Convert.ToInt32(Recursos.solicitudPrestamo.Cel_Aval2_X), oSize.Height - Convert.ToInt32(Recursos.solicitudPrestamo.Cel_Aval2_Y), 0);

            //oPDF.ShowTextAligned(1, Lugar, 150, oSize.Height - 188, 0);
            //oPDF.ShowTextAligned(1, Fecha, 420, oSize.Height - 188, 0);
            //oPDF.ShowTextAligned(1, Identidad, 200, oSize.Height - 222, 0);
            //oPDF.ShowTextAligned(1, Direccion, 200, oSize.Height - 245, 0);
            //oPDF.ShowTextAligned(1, telefono, 480, oSize.Height - 267, 0);
            //oPDF.ShowTextAligned(1, Nombre, 320, oSize.Height - 283, 0);
            //oPDF.ShowTextAligned(1, MontoSol, 200, oSize.Height - 357, 0);
            //oPDF.ShowTextAligned(1, Interes, 200, oSize.Height - 378, 0);

            //oPDF.ShowTextAligned(1, EstadoCivil, 480, oSize.Height - 222, 0);
            //oPDF.ShowTextAligned(1, Dirtrabajo, 229, oSize.Height - 267, 0);
            //oPDF.ShowTextAligned(1, profesion, 220, oSize.Height - 290, 0);
            //oPDF.ShowTextAligned(1, FechaDesemb, 518, oSize.Height - 424, 0);
            //oPDF.ShowTextAligned(1, Celular, 480, oSize.Height - 290, 0);
            //oPDF.ShowTextAligned(1, plazo, 500, oSize.Height - 378, 0);
            //oPDF.ShowTextAligned(1, FormaPago, 500, oSize.Height - 401, 0);
            //oPDF.ShowTextAligned(1, cuota, 205, oSize.Height - 425, 0);

            //oPDF.ShowTextAligned(1, MontoAprob, 210, oSize.Height - 447, 0);
            //oPDF.ShowTextAligned(1, creditoNuevo, 500, oSize.Height - 447, 0);
            //oPDF.ShowTextAligned(1, Garantia, 200, oSize.Height - 401, 0);
            //oPDF.ShowTextAligned(1, objetivo, 228, oSize.Height - 469, 0);
            //oPDF.ShowTextAligned(1, observ, 170, oSize.Height - 530, 0);
            //oPDF.ShowTextAligned(1, NomRef1, 165, oSize.Height - 604, 0);
            //oPDF.ShowTextAligned(1, CelRef1, 460, oSize.Height - 604, 0);
            ////REFERENCIAS 2
            //oPDF.ShowTextAligned(1, NomRef1, 165, oSize.Height - 627, 0);
            //oPDF.ShowTextAligned(1, CelRef1, 460, oSize.Height - 627, 0);




            //oPDF.ShowTextAligned(1, Convert.ToString(Cantidad), 100, oSize.Height - 220, 0);
            //oPDF.ShowTextAligned(1, Descripcion, 125, oSize.Height - 235, 0);

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

        [HttpPost]
        public int EstudioSocioeconomicoImp(Socioeconomico Econ)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["EstudioEconomico"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["EstudioEconomico1"];

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
            //oPDF.SetFontAndSize(bf, 10);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            //string Valor = Convert.ToString(Econ.Monto);// Convert.ToString(Pagare.Valor);
            //oPDF.ShowTextAligned(1, Valor, Convert.ToInt32(Recursos.PagareNormal.ValorGeneral_X),
            //    oSize.Height - Convert.ToInt32(Recursos.PagareNormal.ValorGeneral_Y), 0);
            ////oPDF.EndText();



            oPDF.SetFontAndSize(bf, 10);
            //oPDF.BeginText();

            Moneda Mone = new Moneda();

            //string valorLetras = Mone.Convertir(Convert.ToString(Econ.Prestamos), true);
            //oPDF.ShowTextAligned(1, Nombre, Convert.ToInt32(Recursos.socioEconomico.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Nombre_Y), 0);

            string Fecha = DateTime.Now.ToString("dd-MM-yyyy");
            //string Identidad = Econ.Salario;// Pagare.Identidad;
            string Nombre = Econ.Nombre;
            string Direccion = Econ.Direccion;
            string FechaVence = Econ.ServiciosPub;
            string Celular = Econ.Celular;
            string Renta = Econ.Renta;
            string ServiciosPub = Econ.ServiciosPub;
            string Transporte = Econ.Transporte;
            string Prestamos = Econ.Prestamos;
            string Alimentacion = Econ.Alimentacion;
            string otrosIngresos = Econ.otrosIngresos;
            string Salario = Econ.Salario;
            string NegocioProp = Econ.Negocio;
            string ActivAgrop = Econ.ActividadAgricola;
            string totalIngresos = Econ.TotalIngresos;
            string totalEgresos = Econ.TotalEgresos;
            decimal CapacidadPago = Convert.ToDecimal(totalIngresos) - Convert.ToDecimal(Econ.TotalEgresos);
            string observaciones = Econ.Observaciones;
            string Dependientes = Econ.NumDependientes;

            //string Descripcion = "Interese a Deposito";
            // Le damos posición y rotación al texto
            // la posición de Y es al revés de como estamos acostumbrados
            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

            //oPDF.ShowTextAligned(1, Nombre, 250, oSize.Height - 210, 0);
            //oPDF.ShowTextAligned(1, Direccion, 170, oSize.Height - 231, 0);
            //oPDF.ShowTextAligned(1, Celular, 472, oSize.Height - 231, 0);
            //oPDF.ShowTextAligned(1, Renta, 500, oSize.Height - 337, 0);
            //oPDF.ShowTextAligned(1, ServiciosPub, 500, oSize.Height - 380, 0);
            //oPDF.ShowTextAligned(1, Transporte, 500, oSize.Height - 443, 0);
            //oPDF.ShowTextAligned(1, Prestamos, 500, oSize.Height - 421, 0);
            //oPDF.ShowTextAligned(1, Alimentacion, 500, oSize.Height - 464, 0);
            //oPDF.ShowTextAligned(1, NegocioProp, 295, oSize.Height - 337, 0);
            //oPDF.ShowTextAligned(1, Salario, 295, oSize.Height - 380, 0);
            //oPDF.ShowTextAligned(1, ActivAgrop, 295, oSize.Height - 422, 0);
            //oPDF.ShowTextAligned(1, otrosIngresos, 295, oSize.Height - 464, 0);
            //oPDF.ShowTextAligned(1, totalIngresos, 295, oSize.Height - 527, 0);
            //oPDF.ShowTextAligned(1, totalEgresos, 500, oSize.Height - 527, 0);
            //oPDF.ShowTextAligned(1, Convert.ToString(CapacidadPago), 370, oSize.Height - 570, 0);
            //oPDF.ShowTextAligned(1, observaciones, 215, oSize.Height - 612, 0);
            //oPDF.ShowTextAligned(1, Dependientes, 402, oSize.Height - 295, 0);

            oPDF.ShowTextAligned(1, Nombre, Convert.ToInt32(Recursos.socioEconomico.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Nombre_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Direccion, Convert.ToInt32(Recursos.socioEconomico.Direccion_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Direccion_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Celular, Convert.ToInt32(Recursos.socioEconomico.Celular_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Celular_y), 0); //120,100
            oPDF.ShowTextAligned(1, Renta, Convert.ToInt32(Recursos.socioEconomico.Renta_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Renta_Y), 0); //120,100
            oPDF.ShowTextAligned(1, ServiciosPub, Convert.ToInt32(Recursos.socioEconomico.ServPub_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.ServPub_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Transporte, Convert.ToInt32(Recursos.socioEconomico.Transporte_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Transporte_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Prestamos, Convert.ToInt32(Recursos.socioEconomico.Prestamos_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Prestamos_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Alimentacion, Convert.ToInt32(Recursos.socioEconomico.Alimentacion_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Alimentacion_Y), 0); //120,100
            oPDF.ShowTextAligned(1, NegocioProp, Convert.ToInt32(Recursos.socioEconomico.Negocio_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Negocio_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Salario, Convert.ToInt32(Recursos.socioEconomico.Salario_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Salario_Y), 0); //120,100
            oPDF.ShowTextAligned(1, ActivAgrop, Convert.ToInt32(Recursos.socioEconomico.ActAgropec_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.ActAgropec_Y), 0); //120,100
            oPDF.ShowTextAligned(1, otrosIngresos, Convert.ToInt32(Recursos.socioEconomico.Otros_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Otros_Y), 0); //120,100
            oPDF.ShowTextAligned(1, totalIngresos, Convert.ToInt32(Recursos.socioEconomico.TotalIngresos_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.TotalIngresos_Y), 0); //120,100
            oPDF.ShowTextAligned(1, totalEgresos, Convert.ToInt32(Recursos.socioEconomico.TotalEgresos_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.TotalEgresos_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Convert.ToString(CapacidadPago), Convert.ToInt32(Recursos.socioEconomico.CapacidadPago_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.CapacidadPago_Y), 0); //120,100
            oPDF.ShowTextAligned(1, observaciones, Convert.ToInt32(Recursos.socioEconomico.Observaciones_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Observaciones_Y), 0); //120,100
            oPDF.ShowTextAligned(1, Dependientes, Convert.ToInt32(Recursos.socioEconomico.Dependientes_X), oSize.Height - Convert.ToInt32(Recursos.socioEconomico.Dependientes_Y), 0); //120,100


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

        public int GeneraPdfContratoPrestamo(ContratoPrestamo contratoPrestamo)
        {
            //rutas de nuestros pdf
            string pathPDF = System.Configuration.ConfigurationManager.AppSettings["ContratoPrestamo"];
            string pathPDF2 = System.Configuration.ConfigurationManager.AppSettings["ContratoPrestamo1"];

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
            BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            oPDF.SetColorFill(BaseColor.BLACK);
            oPDF.SetFontAndSize(bf, 11);

            //Se abre el flujo para escribir el texto
            oPDF.BeginText();
            //asignamos el texto
            string PrestamoId = Convert.ToString(contratoPrestamo.IdPres);
            string Identidad = Convert.ToString(contratoPrestamo.identidad);
            string Nombre = Convert.ToString(contratoPrestamo.Nombre);
            string Nombre2 = Nombre.ToUpper();
            string Direccion = Convert.ToString(contratoPrestamo.CLI_Direccion);
            string Monto = Convert.ToString(contratoPrestamo.Monto);
            string Fecha = Convert.ToString(contratoPrestamo.Fec);
            string Plazo = Convert.ToString(contratoPrestamo.PRES_Plazo_Meses);
            string NumeroCuotas = Convert.ToString(contratoPrestamo.NumCuotas);
            string PorcentajeInteres = Convert.ToString(contratoPrestamo.PRES_Porc_Interes);
            string Descripcion = Convert.ToString(contratoPrestamo.DES_Descripcion);
            string GarantiaDescripcion = Convert.ToString(contratoPrestamo.GAR_Descripcion);

            DateTime Fec = DateTime.Parse(Fecha);
            string Dia = Convert.ToString(Fec.Day);
            int DiaMes = Convert.ToInt32(Fec.Month);
            string Anio = Convert.ToString(Fec.Year);

            Conv Numer = new Conv();

            string Mes = Numer.MesAText(DiaMes);

            //oPDF.ShowTextAligned(1, Valor, 210, oSize.Height - 150, 0);

            oPDF.ShowTextAligned(1, PrestamoId, Convert.ToInt32(Recursos.ContratoPrestamo.IdPrestamo_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.IdPrestamo_Y), 0);
            oPDF.ShowTextAligned(1, Identidad, Convert.ToInt32(Recursos.ContratoPrestamo.Identidad_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Identidad_Y), 0);
            oPDF.ShowTextAligned(1, Nombre2, Convert.ToInt32(Recursos.ContratoPrestamo.Nombre_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Nombre_Y), 0);
            oPDF.ShowTextAligned(1, Direccion, Convert.ToInt32(Recursos.ContratoPrestamo.Direccion_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Direccion_Y), 0);
            oPDF.ShowTextAligned(1, Monto, Convert.ToInt32(Recursos.ContratoPrestamo.Monto_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Monto_Y), 0);
            oPDF.ShowTextAligned(1, Plazo, Convert.ToInt32(Recursos.ContratoPrestamo.Plazo_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Plazo_Y), 0);
            oPDF.ShowTextAligned(1, NumeroCuotas, Convert.ToInt32(Recursos.ContratoPrestamo.NumeroCuotas_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.NumeroCuotas_Y), 0);
            oPDF.ShowTextAligned(1, PorcentajeInteres, Convert.ToInt32(Recursos.ContratoPrestamo.PorcentajeInteres_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.PorcentajeInteres_Y), 0);
            oPDF.ShowTextAligned(1, Descripcion, Convert.ToInt32(Recursos.ContratoPrestamo.Descripcion_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Descripcion_Y), 0);
            oPDF.ShowTextAligned(1, GarantiaDescripcion, Convert.ToInt32(Recursos.ContratoPrestamo.GarantiaDescripcion_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.GarantiaDescripcion_Y), 0);
            oPDF.ShowTextAligned(1, Dia, Convert.ToInt32(Recursos.ContratoPrestamo.Dia_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Dia_Y), 0);
            oPDF.ShowTextAligned(1, Mes, Convert.ToInt32(Recursos.ContratoPrestamo.Mes_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Mes_Y), 0);
            oPDF.ShowTextAligned(1, Anio, Convert.ToInt32(Recursos.ContratoPrestamo.Anio_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.Anio_Y), 0);
            oPDF.ShowTextAligned(1, Identidad, Convert.ToInt32(Recursos.ContratoPrestamo.IdentidadFirma_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.IdentidadFirma_Y), 0);
            oPDF.ShowTextAligned(1, Nombre2, Convert.ToInt32(Recursos.ContratoPrestamo.NombreFirma_X), oSize.Height - Convert.ToInt32(Recursos.ContratoPrestamo.NombreFirma_Y), 0);

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

        public JsonResult GeneraContratoPrestamo(int PrestamoId)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.GeneraContratoPrestamos(PrestamoId);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(PrestamoRep.GeneraContratoPrestamos(PrestamoId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(PrestamoId, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EstudioSocioEconomico(int PrestamoId)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.EstudioSocioEconomico(PrestamoId);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(PrestamoRep.EstudioSocioEconomico(PrestamoId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(PrestamoId, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SolicitudCredito(int PrestamoId)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.EstudioSocioEconomico(PrestamoId);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(PrestamoRep.SolicitudCredito(PrestamoId), JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                //throw;
                return Json(PrestamoId, JsonRequestBehavior.AllowGet);
            }
        }

    }
}