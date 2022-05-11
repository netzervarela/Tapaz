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
    public class TipoRazonRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public TipoRazones Save(TipoRazones pTipoRazon)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_TipoRazon_Save");
                cmd.Parameters.AddWithValue("@TipoId", pTipoRazon.TipoId);
                cmd.Parameters["@TipoId"].Direction = ParameterDirection.InputOutput; //Se indica que el TipoId sera un parametro de Entrada/Salida.
            
                cmd.Parameters.AddWithValue("@TipoDescripcion", pTipoRazon.TipoDescripcion);
                cmd.Parameters.AddWithValue("@TipoAbreviatura", pTipoRazon.TipoAbreviatura);
                cmd.Parameters.AddWithValue("@TipoTieneListadoExtra", pTipoRazon.TipoTieneListadoExtra);
                cmd.Parameters.AddWithValue("@TipoEtiquetaListadoExtra", pTipoRazon.TipoEtiquetaListadoExtra);
                cmd.Parameters.AddWithValue("@TipoOrigenListadoExtra", pTipoRazon.TipoOrigenListadoExtra);
                cmd.Parameters.AddWithValue("@TipoCodigoListadoExtra", pTipoRazon.TipoCodigoListadoExtra);
                cmd.Parameters.AddWithValue("@TipoStatus", pTipoRazon.TipoStatus);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@TipoId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pTipoRazon.Mensaje = Ex.Message;
                throw new Exception("Ocurrio un error al guardar el tipo de razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pTipoRazon.Accion = vResultado;
            if (vResultado == 0)
            {
                pTipoRazon.Mensaje = "Se genero un error al insertar la información del tipo de razon!";
            }
            else
            {
                pTipoRazon.Mensaje = "Se ingreso el tipo de razon correctamente!";
            }
            return pTipoRazon;
        }

        public TipoRazones Save1(TipoRazones pTipoRazon)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_TipoRazonAutoservicio_Save");
                cmd.Parameters.AddWithValue("@TipoId", pTipoRazon.TipoId);
                cmd.Parameters["@TipoId"].Direction = ParameterDirection.InputOutput; //Se indica que el TipoId sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@TipoDescripcion", pTipoRazon.TipoDescripcion);
                cmd.Parameters.AddWithValue("@TipoAbreviatura", pTipoRazon.TipoAbreviatura);
                cmd.Parameters.AddWithValue("@TipoTieneListadoExtra", pTipoRazon.TipoTieneListadoExtra);
                cmd.Parameters.AddWithValue("@TipoEtiquetaListadoExtra", pTipoRazon.TipoEtiquetaListadoExtra);
                cmd.Parameters.AddWithValue("@TipoOrigenListadoExtra", pTipoRazon.TipoOrigenListadoExtra);
                cmd.Parameters.AddWithValue("@TipoCodigoListadoExtra", pTipoRazon.TipoCodigoListadoExtra);
                cmd.Parameters.AddWithValue("@TipoStatus", pTipoRazon.TipoStatus);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@TipoId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pTipoRazon.Mensaje = Ex.Message;
                throw new Exception("Ocurrio un error al guardar el tipo de razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pTipoRazon.Accion = vResultado;
            if (vResultado == 0)
            {
                pTipoRazon.Mensaje = "Se genero un error al insertar la información del tipo de razon!";
            }
            else
            {
                pTipoRazon.Mensaje = "Se ingreso el tipo de razon correctamente!";
            }
            return pTipoRazon;
        }


        public List<TipoRazones> GetTipoRazones()
        {
            List<TipoRazones> TipoRazonesList = new List<TipoRazones>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetTipoRazon"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                TipoRazonesList = (from DataRow dr in dt.Rows

                                  select new TipoRazones()
                                  {
                                      TipoId = Convert.ToInt32(dr["TipoId"]),
                                      TipoDescripcion = Convert.ToString(dr["TipoDescripcion"]),
                                      TipoAbreviatura = Convert.ToString(dr["TipoAbreviatura"]),
                                      TipoTieneListadoExtra = Convert.ToInt32(dr["TipoTieneListadoExtra"]),
                                      TipoEtiquetaListadoExtra = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                      TipoOrigenListadoExtra = Convert.ToString(dr["TipoOrigenListadoExtra"]),
                                      TipoCodigoListadoExtra = Convert.ToString(dr["TipoCodigoListadoExtra"]),
                                      TipoStatus = Convert.ToString(dr["TipoStatus"]),
                                      Accion = 1,
                                      Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                  }).ToList();
                if (TipoRazonesList.Count == 0)
                {
                    TipoRazones ss = new TipoRazones();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tipos!";
                    TipoRazonesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                TipoRazones oTipoRazon = new TipoRazones();
                oTipoRazon.Accion = 0;
                oTipoRazon.Mensaje = ex.Message.ToString();
                TipoRazonesList.Add(oTipoRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return TipoRazonesList;
        }


        public List<TipoRazones> GetTipoRazonesAutoservicio()
        {
            List<TipoRazones> TipoRazonesList = new List<TipoRazones>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetTipoRazonAutoservicio"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                TipoRazonesList = (from DataRow dr in dt.Rows

                                   select new TipoRazones()
                                   {
                                       TipoId = Convert.ToInt32(dr["TipoId"]),
                                       TipoDescripcion = Convert.ToString(dr["TipoDescripcion"]),
                                       TipoAbreviatura = Convert.ToString(dr["TipoAbreviatura"]),
                                       TipoTieneListadoExtra = Convert.ToInt32(dr["TipoTieneListadoExtra"]),
                                       TipoEtiquetaListadoExtra = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                       TipoOrigenListadoExtra = Convert.ToString(dr["TipoOrigenListadoExtra"]),
                                       TipoCodigoListadoExtra = Convert.ToString(dr["TipoCodigoListadoExtra"]),
                                       TipoStatus = Convert.ToString(dr["TipoStatus"]),
                                       Accion = 1,
                                       Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                   }).ToList();
                if (TipoRazonesList.Count == 0)
                {
                    TipoRazones ss = new TipoRazones();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tipos!";
                    TipoRazonesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                TipoRazones oTipoRazon = new TipoRazones();
                oTipoRazon.Accion = 0;
                oTipoRazon.Mensaje = ex.Message.ToString();
                TipoRazonesList.Add(oTipoRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return TipoRazonesList;
        }


        public TipoRazones GetTipoRazon(int Id)
        {
            TipoRazones vResultado = new TipoRazones(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetTipoRazon"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TipoId", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TipoId = Convert.ToInt32(consulta["TipoId"]);
                        vResultado.TipoAbreviatura = (string)consulta["TipoAbreviatura"];
                        vResultado.TipoDescripcion = (string)consulta["TipoDescripcion"];
                        vResultado.TipoTieneListadoExtra = Convert.ToInt32(consulta["TipoTieneListadoExtra"]);
                        vResultado.TipoEtiquetaListadoExtra = (string)consulta["TipoEtiquetaListadoExtra"];
                        vResultado.TipoOrigenListadoExtra = (string)consulta["TipoOrigenListadoExtra"];
                        vResultado.TipoCodigoListadoExtra = (string)consulta["TipoCodigoListadoExtra"];
                        vResultado.TipoStatus = (string)consulta["TipoStatus"];
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
                throw new Exception("Error al cargar los datos del tipo de razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //TRAE INFORMACION DE CLIENTES CON REFERENCIAS
        public List<Clientes> GetInfoClientes(int Id)
        {
            //TipoRazones vClientes = new TipoRazones(); //Se crea una variable que contendra los datos del almacen.
            List<Clientes> ClientesList = new List<Clientes>();
            try
            {
                //SqlCommand cmd = new SqlCommand();
                SqlCommand cmd = CrearComando("SF_SP_GetDatosClientesRef"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@id", Id); //Agregamos los parametros.

                //SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                AbrirConexion(); //Se abre la conexion a la BD.
                da.Fill(dt);
                CerrarConexion();

                ClientesList = (from DataRow dr in dt.Rows

                                select new Clientes()
                                {
                                    //ClienteId = Convert.ToInt32(dr["CLI_Codigo"]),
                                    Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                    RTN = Convert.ToString(dr["CLI_RTN"]),
                                    PrimerNombre = Convert.ToString(dr["CLI_Primer_Nombre"]),
                                    SegundoNombre = Convert.ToString(dr["CLI_Segundo_Nombre"]),
                                    PrimerApellido = Convert.ToString(dr["CLI_Primer_Apellido"]),
                                    SegundoApellido = Convert.ToString(dr["CLI_Segundo_Apellido"]),
                                    Genero = Convert.ToInt32(dr["CLI_Genero"]),
                                    Edad = Convert.ToString(dr["CLI_Fecha_Nacimiento"]),
                                    EstadoCli = Convert.ToString(dr["CLI_Estado"]),
                                    EstadoCivil = Convert.ToString(dr["CLI_Estado_Civil"]),
                                    Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                    TelCasa = Convert.ToString(dr["CLI_Tel_Casa"]),
                                    Cel = Convert.ToString(dr["CLI_Cel"]),
                                    Correo = Convert.ToString(dr["CLI_Correo"]),
                                    Profesion = Convert.ToInt32(dr["CLI_Pro_Ofi"]),
                                    DireccionTrabajo = Convert.ToString(dr["CLI_Direc_Trabajo"]),
                                    RefId1 = Convert.ToString(dr["Identidad_Ref1"]),
                                    RefNom1 = Convert.ToString(dr["Nombre_Ref1"]),
                                    RefTel1 = Convert.ToString(dr["Telefono_Ref1"]),
                                    RefCel1 = Convert.ToString(dr["Celular_Ref1"]),
                                    RefId2 = Convert.ToString(dr["Identidad_Ref2"]),
                                    RefNom2 = Convert.ToString(dr["Nombre_Ref2"]),
                                    RefTel2 = Convert.ToString(dr["Telefono_Ref2"]),
                                    RefCel2 = Convert.ToString(dr["Celular_Ref2"]),
                                    NumRef1 = Convert.ToString(dr["Numero_Ref1"]),
                                    NumRef2 = Convert.ToString(dr["Numero_Ref2"]),

                                    Accion = 1,
                                    Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                }).ToList();
            }
            catch (Exception Ex)
            {
                //vClientes.Accion = 0;
                //vClientes.Mensaje = Ex.Message.ToString();

                Clientes oRazon = new Clientes();
                oRazon.Accion = 0;
                oRazon.Mensaje = Ex.Message.ToString();
                ClientesList.Add(oRazon);
                throw new Exception("Error al cargar los datos del Clientes: " + Ex.Message, Ex);
            }
            //finally
            //{
            //    cmd.Dispose();
            //    CerrarConexion();
            //}
            return ClientesList;
        }

        public TipoRazones CheckTipoRazon(string descripcion, string abreviatura)
        {
            TipoRazones vResultado = new TipoRazones(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_TipoRazon_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TipoDescripcion", descripcion); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@TipoAbreviatura", abreviatura); //Agregamos los parametros.

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

        public TipoRazones Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            TipoRazones vResultado = new TipoRazones();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelTipoRazon");
                cmd.Parameters.AddWithValue("@TipoId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito la sucursal!";
                    vResultado.TipoId = Id;
                }
                else
                {
 
                    vResultado.Accion = 0; 
                    vResultado.Mensaje = "No se puede eliminar, ya que tiene razones relacionadas"; 
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
    }
}
