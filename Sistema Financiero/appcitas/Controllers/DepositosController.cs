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
    public class DepositosController : Controller
    {
        // GET: Prestamos

        DepositosPlazoFijo pDeposito = new DepositosPlazoFijo();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Deposito()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDepositos(int ClienteId)
        {

            DepositosRepository DepositosRep = new DepositosRepository();
            try
            {
                return Json(DepositosRep.GetAllByTipo(ClienteId), JsonRequestBehavior.AllowGet);
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



        //GUARDAR NUEVO DEPOSITO A PLAZO
        [HttpPost]
        public JsonResult SaveData(DepositosPlazoFijo Deposit)
        {
            try
            {
                DepositosRepository DepositRep = new DepositosRepository();
                if (ModelState.IsValid)
                {
                    DepositRep.Save(Deposit);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(Deposit, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(Deposit, JsonRequestBehavior.AllowGet);
            }

        }

        //CONTROLADOR VER DEPOSITO EN BOTON DE ACCION
        [HttpPost]
        public JsonResult GetDatosDeposito(int id)
        {
            try
            {
                DepositosRepository DepositosRep = new DepositosRepository();
                if (ModelState.IsValid)
                {
                    DepositosRep.GetDatosDeposito(id);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(DepositosRep.GetDatosDeposito(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(id, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTransaccionesDepositos(int codigo)
        {
            DepositosRepository DepositosRep = new DepositosRepository();
            try
            {
                return Json(DepositosRep.GetTransaccionesDepositos(codigo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<TransaccionesDepositos> list = new List<TransaccionesDepositos>();
                TransaccionesDepositos obj = new TransaccionesDepositos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Realiza Pago a prestamo
        [HttpPost]
        public JsonResult GeneraDeposito(int codigo, string Deposito, int TipoPago)
        {
            DepositosRepository DepositoRep = new DepositosRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(DepositoRep.GeneraDeposito(codigo, Deposito, TipoPago, Cajero), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<TransaccionesDepositos> list = new List<TransaccionesDepositos>();
                TransaccionesDepositos obj = new TransaccionesDepositos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

    }
}