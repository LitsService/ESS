using ESS_Web_Application.Configurations;
using ESS_Web_Application.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ESS_Web_Application.Entity
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DefaultConnection")
        {
        }

        public DbSet<Company> Comapny { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new TestConfiguration());
        }
        public static DataSet GetDataSet(string Select_Command)
        {
            try
            {
                SqlDataAdapter objDA = new SqlDataAdapter(Select_Command, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                DataSet objDS = new DataSet();
                objDA.Fill(objDS);
                return (objDS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetDataTable(string Command, SqlParameter[] SQL_Parameters)
        {
            try
            {
                SqlDataAdapter objDA = new SqlDataAdapter(Command, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                objDA.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (SqlParameter sqlParameter in SQL_Parameters)
                    objDA.SelectCommand.Parameters.Add(sqlParameter);

                DataTable objDS = new DataTable();
                objDA.Fill(objDS);
                return (objDS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet GetDataSet(string Command, Hashtable hsh_Parameters)
        {
            try
            {
                SqlDataAdapter objDA = new SqlDataAdapter(Command, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                objDA.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (hsh_Parameters != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objDA.SelectCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }
                DataSet objDS = new DataSet();
                objDA.Fill(objDS);
                return (objDS);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ExecuteScalar(string Command, Hashtable hsh_Parameters)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                objConnection.Open();
                return objCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static object ExecuteScalar(string Command)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);

            try
            {
                objConnection.Open();
                return objCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static SqlDataReader ExecuteReaderWithCommand(string Command)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);

            try
            {
                objConnection.Open();
                return (objCommand.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
            }
        }

        public static int ExecuteNonQuery(string Command)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);

            try
            {
                objConnection.Open();
                return objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static int ExecuteNonQuery(string Command, Hashtable hsh_Parameters)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                objConnection.Open();
                return objCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static object ExecuteNonQuery(string Command, Hashtable hsh_Parameters, string outParamName, SqlDbType type4outParam, int size4OutParam)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                objCommand.Parameters[outParamName].SqlDbType = type4outParam;
                if (size4OutParam > 0)
                    objCommand.Parameters[outParamName].Size = size4OutParam;
                objCommand.Parameters[outParamName].Direction = ParameterDirection.Output;

                objConnection.Open();
                objCommand.ExecuteNonQuery();

                return objCommand.Parameters[outParamName].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }


        public static void Execute_EB_PROC_EMP_TRX_CALCULATION(Hashtable hsh_Parameters_In, ref Hashtable hsh_Parameters_Out)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand("EB_PROC_EMP_TRX_CALCULATION", objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters_In != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_In.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                if (hsh_Parameters_Out != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        if (obj_Enm.Key.ToString() == "@O_ERR_MSG")
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.VarChar, 200);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }
                        else if (obj_Enm.Key.ToString() == "@O_REJOIN_DATE")
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.DateTime);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }
                        else
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.Decimal, 19);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }

                        objCommand.Parameters[obj_Enm.Key.ToString()].Direction = ParameterDirection.Output;
                    }
                }

                objConnection.Open();
                objCommand.ExecuteNonQuery();

                if (hsh_Parameters_Out != null)
                {
                    Hashtable hsh_Parameters_Out_Temp = new Hashtable();
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        hsh_Parameters_Out_Temp.Add(obj_Enm.Key.ToString(), objCommand.Parameters[obj_Enm.Key.ToString()].Value);
                    }

                    hsh_Parameters_Out = null;
                    hsh_Parameters_Out = hsh_Parameters_Out_Temp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static void Execute_EB_PROC_EMP_LEAVE_BALANCE(Hashtable hsh_Parameters_In, ref Hashtable hsh_Parameters_Out)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand("EB_PROC_EMP_LEAVE_BALANCE", objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters_In != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_In.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                if (hsh_Parameters_Out != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        if (obj_Enm.Key.ToString() == "@O_ERROR_MSG")
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.VarChar, 200);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }
                        else if (obj_Enm.Key.ToString() == "@O_LAST_LEAVE")
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.DateTime);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }
                        else if (obj_Enm.Key.ToString() == "@O_ERROR_CODE")
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.Int);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }
                        else
                        {
                            SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.Decimal, 19);
                            prm.Value = DBNull.Value;
                            objCommand.Parameters.Add(prm);
                        }

                        objCommand.Parameters[obj_Enm.Key.ToString()].Direction = ParameterDirection.Output;
                    }
                }

                objConnection.Open();
                objCommand.ExecuteNonQuery();

                if (hsh_Parameters_Out != null)
                {
                    Hashtable hsh_Parameters_Out_Temp = new Hashtable();
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        hsh_Parameters_Out_Temp.Add(obj_Enm.Key.ToString(), objCommand.Parameters[obj_Enm.Key.ToString()].Value);
                    }

                    hsh_Parameters_Out = null;
                    hsh_Parameters_Out = hsh_Parameters_Out_Temp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }

        public static void Execute_spWithOutParams(string Command, Hashtable hsh_Parameters_In, ref Hashtable hsh_Parameters_Out)
        {
            SqlConnection objConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand objCommand = new SqlCommand(Command, objConnection);
            objCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (hsh_Parameters_In != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_In.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        objCommand.Parameters.AddWithValue(obj_Enm.Key.ToString(), obj_Enm.Value);
                    }
                }

                if (hsh_Parameters_Out != null)
                {
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        int szV = 0;
                        int.TryParse(obj_Enm.Value.ToString(), out szV);
                        SqlParameter prm = new SqlParameter(obj_Enm.Key.ToString(), SqlDbType.VarChar, szV);
                        prm.Value = DBNull.Value;
                        objCommand.Parameters.Add(prm);
                        objCommand.Parameters[obj_Enm.Key.ToString()].Direction = ParameterDirection.Output;
                    }
                }

                objConnection.Open();
                objCommand.ExecuteNonQuery();

                if (hsh_Parameters_Out != null)
                {
                    Hashtable hsh_Parameters_Out_Temp = new Hashtable();
                    IDictionaryEnumerator obj_Enm = hsh_Parameters_Out.GetEnumerator();
                    while (obj_Enm.MoveNext())
                    {
                        hsh_Parameters_Out_Temp.Add(obj_Enm.Key.ToString(), objCommand.Parameters[obj_Enm.Key.ToString()].Value);
                    }

                    hsh_Parameters_Out = null;
                    hsh_Parameters_Out = hsh_Parameters_Out_Temp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objCommand != null)
                {
                    objCommand.Dispose();
                }
                if (!(objConnection.State == ConnectionState.Closed))
                {
                    objConnection.Close();
                }
            }
        }
    }
}