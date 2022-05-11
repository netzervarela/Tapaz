using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;


namespace appcitas.Repository
{
    public class DepositosRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public DepositosPlazoFijo Save(DepositosPlazoFijo pDeposito)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            int CodigoEnvio;
            if (pDeposito.DPF_Codigo != -1)
                CodigoEnvio = pDeposito.DPF_Codigo;
            else
                CodigoEnvio = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SF_SP_Guarda_DPF");

                //PARAMETROS DEPOSITO

                cmd.Parameters.AddWithValue("@Codigo", CodigoEnvio);
                cmd.Parameters.AddWithValue("@Codigo_CLI", pDeposito.ClienteId);
                cmd.Parameters.AddWithValue("@mto_Deposito", pDeposito.DPF_Monto);
                cmd.Parameters.AddWithValue("@Plazo_Meses", pDeposito.DPF_Plazo);
                cmd.Parameters.AddWithValue("@Tasa_Interes", pDeposito.DPF_Tasa_interes);
                cmd.Parameters.AddWithValue("@Tipo", pDeposito.DPF_Tipo);
                cmd.Parameters.AddWithValue("@Beneficiario1", pDeposito.DPF_Beneficiario_1);
                cmd.Parameters.AddWithValue("@ID_Bene_1", pDeposito.DPF_ID_Bene_1);
                cmd.Parameters.AddWithValue("@Porc_Bene_1", pDeposito.DPF_Porc_1);
                cmd.Parameters.AddWithValue("@Beneficiario2", pDeposito.DPF_Beneficiario_2);
                cmd.Parameters.AddWithValue("@ID_Bene_2", pDeposito.DPF_ID_Bene_2);
                cmd.Parameters.AddWithValue("@Porc_Bene_2", pDeposito.DPF_Porc_2);
                cmd.Parameters.AddWithValue("@Beneficiario3", pDeposito.DPF_Beneficiario_3);
                cmd.Parameters.AddWithValue("@ID_Bene_3", pDeposito.DPF_ID_Bene_3);
                cmd.Parameters.AddWithValue("@Porc_Bene_3", pDeposito.DPF_Porc_3);
                cmd.Parameters.AddWithValue("@Observacion", pDeposito.DPF_Observacion);
                cmd.Parameters.AddWithValue("@Pago_intereses", pDeposito.DPF_Pago_intereses);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                //vResultado = Convert.ToInt32(cmd.Parameters["@RazonId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pDeposito.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar los datos: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pDeposito.Accion = vResultado;
            if (vResultado == 0)
            {
                pDeposito.Mensaje = "Se genero un error al insertar la información del deposito!";
            }
            else
            {
                pDeposito.Mensaje = "Se guardo el deposito correctamente!";
            }
            return pDeposito;
        }

        //TRANSACCIONES DE DEPOSITOS
        public List<TransaccionesDepositos> GetTransaccionesDepositos(int codigo)
        {
            List<TransaccionesDepositos> CitasList = new List<TransaccionesDepositos>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_Get_Trans_Depositos"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@Codigo", codigo); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new TransaccionesDepositos()
                             {
                                 Fecha = Convert.ToString(dr["Fecha"]),
                                 TRD_Deposito = Convert.ToDecimal(dr["TRD_Deposito"]),
                                 TRD_Retiro = Convert.ToDecimal(dr["TRD_Retiro"]),
                                 TRD_Interes = Convert.ToDecimal(dr["TRD_Interes"]),
                                 TRD_Agrego = Convert.ToString(dr["TRD_Agrego"]),
                                 Saldo = Convert.ToDecimal(dr["Saldo"]),
                                 TRD_Codigo_DPF = Convert.ToInt32(dr["TRD_Codigo_DPF"]),
                                 //PRES_Estado = Convert.ToInt32(dr["PRES_Estado"]),
                                 Accion = 1,
                                 Mensaje = "Las transacciones fueron cargadas exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    TransaccionesDepositos ss = new TransaccionesDepositos();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                TransaccionesDepositos oCita = new TransaccionesDepositos();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }



        public List<Razones> GetRazones()
        {
            List<Razones> RazonesList = new List<Razones>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetRazon"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                RazonesList = (from DataRow dr in dt.Rows

                                   select new Razones()
                                   {
                                       TipoId = Convert.ToInt32(dr["TipoId"]),
                                       RazonId = Convert.ToInt32(dr["RazonId"]),
                                       RazonDescripcion = Convert.ToString(dr["RazonDescripcion"]),
                                       RazonAbreviatura = Convert.ToString(dr["RazonAbreviatura"]),
                                       TipoAbreviatura = Convert.ToString(dr["TipoAbreviatura"]),
                                       RazonGroup = Convert.ToString(dr["RazonGroup"]),
                                       RazonStatus = Convert.ToString(dr["RazonStatus"]),
                                       ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                       Accion = 1,
                                       Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                   }).ToList();
                if (RazonesList.Count == 0)
                {
                    Razones ss = new Razones();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Razones!";
                    RazonesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Razones oRazon = new Razones();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                RazonesList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return RazonesList;
        }

        public Razones GetRazon(int Id, int TipoId)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetRazon"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@RazonId", Id);
                cmd.Parameters.AddWithValue("@TipoId", TipoId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TipoId = Convert.ToInt32(consulta["TipoId"]);
                        vResultado.RazonId = Convert.ToInt32(consulta["RazonId"]);
                        vResultado.RazonAbreviatura = (string)consulta["RazonAbreviatura"];
                        vResultado.RazonDescripcion = (string)consulta["RazonDescripcion"];
                        vResultado.TipoAbreviatura = (string)consulta["TipoAbreviatura"];
                        vResultado.RazonGroup = (string)consulta["RazonGroup"];
                        vResultado.RazonStatus = (string)consulta["RazonStatus"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el tipo de razon correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }


        public Razones CheckRazon(string descripcion, string abreviatura)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Razon_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@RazonDescripcion", descripcion); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@RazonAbreviatura", abreviatura); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el Trámite!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Hubo un inconveniente al cargar la información: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Razones Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Razones vResultado = new Razones();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelRazon");
                cmd.Parameters.AddWithValue("@RazonId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el tipo de razon!";
                    vResultado.TipoId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.TipoId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Razones GetAllByTipo2(int TipoId)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetRazonByTipoId"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TipoId", TipoId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TipoId = Convert.ToInt32(consulta["TipoId"]);
                        vResultado.RazonId = Convert.ToInt32(consulta["RazonId"]);
                        vResultado.RazonAbreviatura = (string)consulta["RazonAbreviatura"];
                        vResultado.RazonDescripcion = (string)consulta["RazonDescripcion"];
                        vResultado.TipoAbreviatura = (string)consulta["TipoAbreviatura"];
                        vResultado.RazonGroup = (string)consulta["RazonGroup"];
                        vResultado.RazonStatus = (string)consulta["RazonStatus"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el tipo de razon correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<DepositosPlazoFijo> GetAllByTipo(int ClienteId)
        {
            List<DepositosPlazoFijo> DepositosList = new List<DepositosPlazoFijo>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("dbo.SF_GetDepositosClienteId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@ClienteId", ClienteId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                DepositosList = (from DataRow dr in dt.Rows

                                 select new DepositosPlazoFijo()
                               {
                                   DPF_Codigo = Convert.ToInt32(dr["DPF_Codigo"]),
                                   DPF_Fecha_Apert = Convert.ToString(dr["DPF_Fecha_Apert"]),
                                   DPF_Monto = Convert.ToString(dr["DPF_Monto"]),
                                   DPF_Saldo = Convert.ToString(dr["DPF_Saldo"]),
                                   DPF_Plazo = Convert.ToString(dr["DPF_Plazo"]),
                                   DPF_Tasa_interes = Convert.ToString(dr["DPF_Tasa_interes"]),
                                   TDP_Descripcion = Convert.ToString(dr["TDP_Descripcion"]),
                                   DPF_Beneficiario_1 = Convert.ToString(dr["DPF_Beneficiario_1"]),
                                   DPF_ID_Bene_1 = Convert.ToString(dr["DPF_ID_Bene_1"]),
                                   DPF_Porc_1 = Convert.ToString(dr["DPF_Porc_1"]),
                                   DPF_Beneficiario_2 = Convert.ToString(dr["DPF_Beneficiario_2"]),
                                   DPF_ID_Bene_2 = Convert.ToString(dr["DPF_ID_Bene_2"]),
                                   DPF_Porc_2 = Convert.ToString(dr["DPF_Porc_2"]),
                                   DPF_Beneficiario_3 = Convert.ToString(dr["DPF_Beneficiario_3"]),
                                   DPF_ID_Bene_3 = Convert.ToString(dr["DPF_ID_Bene_3"]),
                                   DPF_Porc_3 = Convert.ToString(dr["DPF_Porc_3"]),
                                   DPF_Estado = Convert.ToInt32(dr["DPF_Estado"]),
                                   DPF_Vencido = Convert.ToString(dr["DPF_Vencido"]),
                                   DPF_Observacion = Convert.ToString(dr["DPF_Observacion"]),
                                   DPF_Pago_intereses = Convert.ToString(dr["DPF_Pago_intereses"]),
                                   ClienteId = Convert.ToInt32(dr["DPF_Codigo_CLI"]),
                                   Nombre = Convert.ToString(dr["Nombre"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los Depositos del Cliente"
                               }).ToList();
            }
            catch (Exception ex)
            {
                DepositosPlazoFijo oRazon = new DepositosPlazoFijo();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                DepositosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DepositosList;
        }

        //VER DEPOSITO EN BOTON DE ACCION
        public List<DepositosPlazoFijo> GetDatosDeposito(int id)
        {
            List<DepositosPlazoFijo> DepositosList = new List<DepositosPlazoFijo>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SF_SP_GetDatosDeposito"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@CodDeposito", id); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                DepositosList = (from DataRow dr in dt.Rows

                                 select new DepositosPlazoFijo()
                                 {
                                     DPF_Codigo = Convert.ToInt32(dr["DPF_Codigo"]),
                                     DPF_Fecha_Apert = Convert.ToString(dr["DPF_Fecha_Apert"]),
                                     DPF_Monto = Convert.ToString(dr["DPF_Monto"]),
                                     DPF_Saldo = Convert.ToString(dr["DPF_Saldo"]),
                                     DPF_Plazo = Convert.ToString(dr["DPF_Plazo"]),
                                     DPF_Tasa_interes = Convert.ToString(dr["DPF_Tasa_interes"]),
                                     DPF_Tipo = Convert.ToInt32(dr["DPF_Tipo"]),
                                     DPF_Beneficiario_1 = Convert.ToString(dr["DPF_Beneficiario_1"]),
                                     DPF_ID_Bene_1 = Convert.ToString(dr["DPF_ID_Bene_1"]),
                                     DPF_Porc_1 = Convert.ToString(dr["DPF_Porc_1"]),
                                     DPF_Beneficiario_2 = Convert.ToString(dr["DPF_Beneficiario_2"]),
                                     DPF_ID_Bene_2 = Convert.ToString(dr["DPF_ID_Bene_2"]),
                                     DPF_Porc_2 = Convert.ToString(dr["DPF_Porc_2"]),
                                     DPF_Beneficiario_3 = Convert.ToString(dr["DPF_Beneficiario_3"]),
                                     DPF_ID_Bene_3 = Convert.ToString(dr["DPF_ID_Bene_3"]),
                                     DPF_Porc_3 = Convert.ToString(dr["DPF_Porc_3"]),
                                     DPF_Estado = Convert.ToInt32(dr["DPF_Estado"]),
                                     DPF_Vencido = Convert.ToString(dr["DPF_Vencido"]),
                                     DPF_Observacion = Convert.ToString(dr["DPF_Observacion"]),
                                     DPF_Pago_intereses = Convert.ToString(dr["DPF_Pago_intereses"]),
                                     ClienteId = Convert.ToInt32(dr["DPF_Codigo_CLI"]),
                                     Nombre = Convert.ToString(dr["Nombre"]),
                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos del deposito"
                                 }).ToList();
            }
            catch (Exception ex)
            {
                DepositosPlazoFijo oRazon = new DepositosPlazoFijo();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                DepositosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DepositosList;
        }

        //GENERAR TRANSACCION DE DEPOSITO

        public MensajesResultado GeneraDeposito(int codigo, string Deposito, int TipoPago ,string Cajero)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = codigo;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_GeneraDeposito");
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@Deposito", Convert.ToDecimal(Deposito));
                cmd.Parameters.AddWithValue("@Cajero", Cajero);
                cmd.Parameters.AddWithValue("@tipoPago", TipoPago);
                cmd.Parameters.AddWithValue("@Recibo", 1);
                cmd.Parameters["@Recibo"].Direction = ParameterDirection.InputOutput;
                //cmd.Parameters.AddWithValue("@Recibo", ParameterDirection.InputOutput);
                ////cmd.Parameters["@Recibo"].Direction = ParameterDirection.InputOutput;

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                vControl = Convert.ToInt32(cmd.Parameters["@Recibo"].Value);

                if (vControl > 0)
                {

                    //if (a == 1)
                    //{
                    vResultado.Estado = 1;
                    vResultado.Mensaje = "Deposito Realizado Exitosamente!";
                    vResultado.Num = vControl;

                    //}
                    //else
                    //{
                    //vResultado.Estado = 2;
                    //vResultado.Mensaje = " ";
                    //}
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El pago no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //GENERA CONTRATO DE PRESTAMO
        public List<ContratoPrestamo> GeneraFormato(int PrestamoId)
        {
            List<ContratoPrestamo> CitasList = new List<ContratoPrestamo>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GenerarContrato"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CodPrestamo", PrestamoId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new ContratoPrestamo()
                             {
                                 Nombre = Convert.ToString(dr["Nombre"]),
                                 identidad = Convert.ToString(dr["identidad"]),
                                 CLI_Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                 Monto = Convert.ToDecimal(dr["Monto"]),
                                 Fec = Convert.ToString(dr["Fec"]),
                                 PRES_Plazo_Meses = Convert.ToInt32(dr["PRES_Plazo_Meses"]),
                                 NumCuotas = Convert.ToInt32(dr["NumCuotas"]),
                                 PRES_Porc_Interes = Convert.ToDecimal(dr["PRES_Porc_Interes"]),
                                 DES_Descripcion = Convert.ToString(dr["DES_Descripcion"]),
                                 GAR_Descripcion = Convert.ToString(dr["GAR_Descripcion"]),
                                 Accion = 1,
                                 Mensaje = "Las transacciones fueron cargadas exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    ContratoPrestamo ss = new ContratoPrestamo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                ContratoPrestamo oCita = new ContratoPrestamo();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        //GENERA SOLICITUD DE PRESTAMO
        public List<SolicitudCredito> SolicitudCredito(int PrestamoId)
        {
            List<SolicitudCredito> CitasList = new List<SolicitudCredito>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GenerarSolicitudCredito"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CodPrestamo", PrestamoId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new SolicitudCredito()
                             {
                                 Nombre = Convert.ToString(dr["Nombre"]),
                                 CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                 CLI_Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                 Estado_Civil = Convert.ToString(dr["Estado_Civil"]),
                                 CLI_Cel = Convert.ToString(dr["CLI_Cel"]),
                                 PRES_Mto_Solicitado = Convert.ToDecimal(dr["PRES_Mto_Solicitado"]),
                                 PRES_Porc_Interes = Convert.ToDecimal(dr["PRES_Porc_Interes"]),
                                 PRES_Mto_Aprobado = Convert.ToDecimal(dr["PRES_Mto_Aprobado"]),
                                 PRES_Plazo_Meses = Convert.ToInt32(dr["PRES_Plazo_Meses"]),
                                 DES_Descripcion = Convert.ToString(dr["DES_Descripcion"]),
                                 GAR_Descripcion = Convert.ToString(dr["GAR_Descripcion"]),
                                 FormaPago = Convert.ToString(dr["FormaPago"]),
                                 PRES_Observaciones = Convert.ToString(dr["PRES_Observaciones"]),
                                 Fecha = Convert.ToString(dr["Fecha"]),
                                 PRES_Fecha_Aprob = Convert.ToString(dr["PRES_Fecha_Aprob"]),
                                 REF_Nombre = Convert.ToString(dr["REF_Nombre"]),
                                 REF_Celular = Convert.ToString(dr["REF_Celular"]),
                                 REF_Num = Convert.ToInt32(dr["REF_Num"]),
                                 PRO_Descripcion = Convert.ToString(dr["PRO_Descripcion"]),
                                 CLI_Direc_Trabajo = Convert.ToString(dr["CLI_Direc_Trabajo"]),
                                 Accion = 1,
                                 Mensaje = "Los datos fueron cargadas exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    SolicitudCredito ss = new SolicitudCredito();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                SolicitudCredito oCita = new SolicitudCredito();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }
        //GENERA ESTUDIO SOCIOECONOMICO
        public List<EstudioSocioEconomico> EstudioSocioEconomico(int PrestamoId)
        {
            List<EstudioSocioEconomico> CitasList = new List<EstudioSocioEconomico>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GenerarEstudioSocioEconomico"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@PrestamoId", PrestamoId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new EstudioSocioEconomico()
                             {
                                 Nombre = Convert.ToString(dr["Nombre"]),
                                 CLI_Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                 CLI_Cel = Convert.ToString(dr["CLI_Cel"]),
                                 ESE_NumeroDependientes = Convert.ToDecimal(dr["ESE_NumeroDependientes"]),
                                 ESE_NegocioPropio = Convert.ToDecimal(dr["ESE_NegocioPropio"]),
                                 ESE_Salario = Convert.ToDecimal(dr["ESE_Salario"]),
                                 ESE_ActividadAgropecuaria = Convert.ToDecimal(dr["ESE_ActividadAgropecuaria"]),
                                 ESE_Otros_Ingresos = Convert.ToDecimal(dr["ESE_Otros_Ingresos"]),
                                 ESE_Renta = Convert.ToDecimal(dr["ESE_Renta"]),
                                 ESE_ServiciosPublicos = Convert.ToDecimal(dr["ESE_ServiciosPublicos"]),
                                 ESE_Prestamos = Convert.ToDecimal(dr["ESE_Prestamos"]),
                                 ESE_Transporte = Convert.ToDecimal(dr["ESE_Transporte"]),
                                 ESE_Alimentacion = Convert.ToDecimal(dr["ESE_Alimentacion"]),
                                 ESE_Vestuario = Convert.ToDecimal(dr["ESE_Vestuario"]),
                                 ESE_Otros_Egresos = Convert.ToDecimal(dr["ESE_Otros_Egresos"]),
                                 ESE_Observaciones = Convert.ToString(dr["ESE_Observaciones"]),
                                 Accion = 1,
                                 Mensaje = "Los datos fueron cargadas exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    EstudioSocioEconomico ss = new EstudioSocioEconomico();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                EstudioSocioEconomico oCita = new EstudioSocioEconomico();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        //GENERA PAGARE DE PRESTAMO
        public List<PagarePrestamo> GeneraPagare(int PrestamoId)
        {
            List<PagarePrestamo> CitasList = new List<PagarePrestamo>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GenerarPagare"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CodPrestamo", PrestamoId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new PagarePrestamo()
                             {
                                 Nombre = Convert.ToString(dr["Nombre"]),
                                 identidad = Convert.ToString(dr["identidad"]),
                                 CLI_Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                 Monto = Convert.ToDecimal(dr["Monto"]),
                                 Fec = Convert.ToString(dr["Fec"]),
                                 Tipo = Convert.ToInt32(dr["PRES_Garantia"]),
                                 Accion = 1,
                                 Mensaje = "Los datos fueron cargados exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    PagarePrestamo ss = new PagarePrestamo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                PagarePrestamo oCita = new PagarePrestamo();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }
    }
}