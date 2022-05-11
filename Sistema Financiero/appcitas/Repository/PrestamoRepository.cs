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
    public class PrestamoRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Prestamos Save(Prestamos pPrestamo)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SF_SP_Guarda_Prestamo");

                //PARAMETROS PRESTAMO
                cmd.Parameters.AddWithValue("@Codigo", -1);
                cmd.Parameters.AddWithValue("@Codigo_CLI", pPrestamo.ClienteId);
                cmd.Parameters.AddWithValue("@mto_Solicitado", pPrestamo.MontoSolicitado);
                cmd.Parameters.AddWithValue("@Plazo_Meses", pPrestamo.PlazoMeses);
                cmd.Parameters.AddWithValue("@Garantia", pPrestamo.Garantia);
                cmd.Parameters.AddWithValue("@Porc_Interes", pPrestamo.Interes);
                cmd.Parameters.AddWithValue("@Destino", pPrestamo.Destino);
                cmd.Parameters.AddWithValue("@Frec_Pago", pPrestamo.FrecPago);
                cmd.Parameters.AddWithValue("@TipoCuota", pPrestamo.TipoCuota);
                cmd.Parameters.AddWithValue("@Observaciones", pPrestamo.Observaciones);


                //DATOS GENERALES ESTUDIO SOCIOECONOMICO

                cmd.Parameters.AddWithValue("@Personas", pPrestamo.Personas);
                cmd.Parameters.AddWithValue("@NegocioPropio", pPrestamo.NegocioPropio);
                cmd.Parameters.AddWithValue("@Salario", pPrestamo.Salario);
                cmd.Parameters.AddWithValue("@Finca", pPrestamo.Finca);
                cmd.Parameters.AddWithValue("@Otros", pPrestamo.Otros);
                cmd.Parameters.AddWithValue("@Renta", pPrestamo.Renta);
                cmd.Parameters.AddWithValue("@ServicioPublico", pPrestamo.ServicioPublico);
                cmd.Parameters.AddWithValue("@Prestamo", pPrestamo.Prestamo);
                cmd.Parameters.AddWithValue("@Transporte", pPrestamo.Transporte);
                cmd.Parameters.AddWithValue("@Alimentacion", pPrestamo.Alimentacion);
                cmd.Parameters.AddWithValue("@Vestuario", pPrestamo.Vestuario);
                cmd.Parameters.AddWithValue("@Otros1", pPrestamo.Otros1);
                cmd.Parameters.AddWithValue("@Observaciones1", pPrestamo.Observaciones1);

                //AVALES
                cmd.Parameters.AddWithValue("@IdAval1", pPrestamo.IdAval1);
                cmd.Parameters.AddWithValue("@NomAval1", pPrestamo.NomAval1);
                cmd.Parameters.AddWithValue("@TelAval1", pPrestamo.TelAval1);
                cmd.Parameters.AddWithValue("@CelAval1", pPrestamo.CelAval1);
                cmd.Parameters.AddWithValue("@DirAval1", pPrestamo.DirAval1);
                //cmd.Parameters.AddWithValue("@NumAval1", pPrestamo.NumAval1);

                cmd.Parameters.AddWithValue("@IdAval2", pPrestamo.IdAval2);
                cmd.Parameters.AddWithValue("@NomAval2", pPrestamo.NomAval2);
                cmd.Parameters.AddWithValue("@TelAval2", pPrestamo.TelAval2);
                cmd.Parameters.AddWithValue("@CelAval2", pPrestamo.CelAval2);
                cmd.Parameters.AddWithValue("@DirAval2", pPrestamo.DirAval2);
                //cmd.Parameters.AddWithValue("@NumAval2", pPrestamo.NumAval2);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                //vResultado = Convert.ToInt32(cmd.Parameters["@RazonId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pPrestamo.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pPrestamo.Accion = vResultado;
            if (vResultado == 0)
            {
                pPrestamo.Mensaje = "Se genero un error al insertar la información del préstamo!";
            }
            else
            {
                pPrestamo.Mensaje = "Se ingreso el préstamo correctamente!";
            }
            return pPrestamo;
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

        public List<ContratoPrestamo> GeneraContratoPrestamos(int PrestamoId)
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
                    CitasList.Add(ss)
;
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

        public List<Prestamos> GetAllByTipo(int ClienteId)
        {
            List<Prestamos> PrestamosList = new List<Prestamos>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("dbo.SF_GetPrestamosClienteId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@ClienteId", ClienteId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                PrestamosList = (from DataRow dr in dt.Rows

                               select new Prestamos()
                               {
                                   PrestamoId = Convert.ToInt32(dr["PRES_Codigo"]),
                                   FecSolicitud = Convert.ToString(dr["PRES_Fecha_Solicitud"]),
                                   MontoSolicitado = Convert.ToString(dr["PRES_mto_Solicitado"]),
                                   MontoAprobado = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                   PlazoMeses = Convert.ToString(dr["PRES_Plazo_Meses"]),
                                   Interes = Convert.ToString(dr["PRES_Porc_Interes"]),
                                   Saldo = Convert.ToString(dr["PRES_Saldo"]),
                                   Estado = Convert.ToString(dr["PRES_Estado"]),
                                   Garantia = Convert.ToString(dr["GAR_Descripcion"]),
                                   ClienteId = Convert.ToInt32(dr["PRES_Codigo_CLI"]),
                                   NomPrestamo = Convert.ToString(dr["Nombre"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                               }).ToList();
            }
            catch (Exception ex)
            {
                Prestamos oRazon = new Prestamos();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                PrestamosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PrestamosList;
        }

        //VER PRESTAMO EN BOTON DE ACCION
        public List<Prestamos> GetDatosPrestamos(int id)
        {
            List<Prestamos> PrestamosList = new List<Prestamos>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SF_SP_GetDatosPrestamos"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@CodPrestamo", id); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                PrestamosList = (from DataRow dr in dt.Rows

                                 select new Prestamos()
                                 {
                                     //DATOS PRESTAMO
                                     Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                     MontoSolicitado = Convert.ToString(dr["PRES_mto_Solicitado"]),
                                     PlazoMeses = Convert.ToString(dr["PRES_Plazo_Meses"]),
                                     Garantia = Convert.ToString(dr["GAR_Codigo"]),
                                     Interes = Convert.ToString(dr["PRES_Porc_Interes"]),
                                     Destino = Convert.ToString(dr["DES_Codigo"]),
                                     FrecPago = Convert.ToString(dr["PRES_Frec_Pago"]),
                                     TipoCuota = Convert.ToString(dr["PRES_Tipo_Cuota"]),
                                     Observaciones = Convert.ToString(dr["PRES_Observaciones"]),

                                     //DATOS DEL ESTUDIO SOCIOECONOMICO
                                     Personas = Convert.ToString(dr["ESE_NumeroDependientes"]),
                                     NegocioPropio = Convert.ToString(dr["ESE_NegocioPropio"]),
                                     Salario = Convert.ToString(dr["ESE_Salario"]),
                                     Finca = Convert.ToString(dr["ESE_ActividadAgropecuaria"]),
                                     Otros = Convert.ToString(dr["ESE_Otros_Ingresos"]),
                                     Renta = Convert.ToString(dr["ESE_Renta"]),
                                     ServicioPublico = Convert.ToString(dr["ESE_ServiciosPublicos"]),
                                     Prestamo = Convert.ToString(dr["ESE_Prestamos"]),
                                     Transporte = Convert.ToString(dr["ESE_Transporte"]),
                                     Alimentacion = Convert.ToString(dr["ESE_Alimentacion"]),
                                     Vestuario = Convert.ToString(dr["ESE_Vestuario"]),
                                     Otros1 = Convert.ToString(dr["ESE_Otros_Egresos"]),
                                     Observaciones1 = Convert.ToString(dr["ESE_Observaciones"]),

                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                 }).ToList();
            }
            catch (Exception ex)
            {
                Prestamos oRazon = new Prestamos();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                PrestamosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PrestamosList;
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
                                 totalIngresos = Convert.ToDecimal(dr["TotalIngresos"]),
                                 totalEgresos = Convert.ToDecimal(dr["TotalEgresos"]),
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

        //PROYECCION PLAN DE PAGO
        public List<PlanPago> GeneraPlanPago(decimal monto, decimal tasa, int plazo, int frecuency, string fecha, int tipo)
        {
            List<PlanPago> CitasList = new List<PlanPago>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Calculo_Pago"); //Pasamos el procedimiento almacenado. 
                //cmd.Parameters.AddWithValue("@CodPrest", CodPrest);
                cmd.Parameters.AddWithValue("@Monto", monto);//Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@Tasa", tasa);
                cmd.Parameters.AddWithValue("@Plazo", plazo);
                cmd.Parameters.AddWithValue("@Frecuency", frecuency);
                cmd.Parameters.AddWithValue("@FecAprob", fecha);
                cmd.Parameters.AddWithValue("@Tipo", tipo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new PlanPago()
                             {
                                 //Codigo = Convert.ToString(dr["Codigo"]),
                                 Num = Convert.ToInt32(dr["Num"]),
                                 Fecha = Convert.ToString(dr["Fecha"]),
                                 Capital = Convert.ToDecimal(dr["Cuota"]),
                                 Interes = Convert.ToDecimal(dr["Interes"]),
                                 //Frecuency = Convert.ToString(dr["Frecuency"]),
                                 Total = Convert.ToDecimal(dr["Capital"]),
                                 Saldo = Convert.ToDecimal(dr["Saldo"]),
                                 Accion = 1
                                 //Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    PlanPago ss = new PlanPago();
                    ss.Accion = 0;
                    //ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                PlanPago oCita = new PlanPago();
                //oCita.Accion = 0;
                //oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

    }
}