using System;
using System.Data;

using CustomLog;

using Oracle.DataAccess.Client;

namespace AccesoDatos.NMOracle
{
    public partial class Conexion
    {
        public void SP_Command(string strCommandText, 
                               string strCommandType)
        {
            try
            {
                _Command(strCommandText, strCommandType, false);

            }
            catch (Exception ex)
            {
                // registrando evento
                Bitacora.Current.Error<Conexion>(ex, new { strCommandText, strCommandType });

                //throw new Exception(ex.ToString());
                throw;
            }
        }

        private void _Command(string strCommandText, 
                              string strCommandType, 
                              bool bolTransaction)
        {
            objOracleCommand.CommandText = strCommandText;
            objOracleCommand.CommandTimeout = intCommandTimeout;
            objOracleCommand.Parameters.Clear();
            
            objSBQuery.Append(strCommandText + "(");

            if (strCommandType.Equals(strStoredProcedure))
                objOracleCommand.CommandType = CommandType.StoredProcedure;

            if (strCommandType.Equals(strSentenciaText))
                objOracleCommand.CommandType = CommandType.Text;
       }

        public void AgregarParametro(string Nombre, 
                                     object Valor, 
                                     OracleDbType Tipo, 
                                     ParameterDirection Direccion, 
                                     bool Retorna = false)
        {
            OracleParameter objParameter;

            using (objParameter = new OracleParameter(Nombre, Tipo, Direccion))
            {
                if (Valor != null)
                {
                    //if (Valor.GetType().Equals(typeof(string)))
                    if (Valor is string)
                    {
                        //if (Valor.Equals(string.Empty))
                        /*
                        if (string.IsNullOrWhiteSpace(Valor.ToString()))
                        {
                            objParameter.Size = 200;

                        } else {
                            objParameter.Size = Valor.ToString().Length;
                        }
                        */
                        objParameter.Size = (string.IsNullOrWhiteSpace(Valor.ToString()) ? 200 : Valor.ToString().Length);
                    }

                    objParameter.Value = Valor;
                }

                objSBQuery.Append(Nombre + "--> '" + Valor + "';");
                objOracleCommand.Parameters.Add(objParameter);
                objParameter.Dispose();
            }
        }

        public string LeeParametros(string Nombre, 
                                    string ValorDefecto)
        {
            //return ((objOracleCommand.Parameters[Nombre].Value == DBNull.Value) ? (ValorDefecto == null ? null : ValorDefecto) : objOracleCommand.Parameters[Nombre].Value.ToString());
            return ((objOracleCommand.Parameters[Nombre].Value == DBNull.Value) ? ValorDefecto : objOracleCommand.Parameters[Nombre].Value.ToString());
        }

        public OracleDataReader _ExecuteReader(bool bolCommit)
        {
            OracleDataReader objRespuesta;

            try
            {
                objRespuesta = objOracleCommand.ExecuteReader();

                if (bolCommit)
                    objOracleTransaction.Commit();

            }
            catch (Exception ex)
            {
                // registrando evento
                Bitacora.Current.Error<Conexion>(ex, new { bolCommit });

                if (objOracleTransaction != null)
                {
                    objOracleTransaction.Rollback();
                }

                //throw new Exception(ex.ToString());
                throw;
            }

            return objRespuesta;
        }

        public bool _ExecuteNonQuery(bool bolCommit)
        {
            bool bolRespuesta;

            try
            {
                objOracleCommand.ExecuteNonQuery();

                if (bolCommit)
                    objOracleTransaction.Commit();

                bolRespuesta = true;

            }
            catch (Exception ex)
            {
                // registrando evento
                Bitacora.Current.Error<Conexion>(ex, new { bolCommit });

                if (objOracleTransaction != null)
                {
                    objOracleTransaction.Rollback();
                }

                //throw new Exception(ex.ToString());
                throw;

            }
            finally
            {
                objOracleTransaction = null;
            }

            return bolRespuesta;
        }

        public string LeeColumnasDataReader(OracleDataReader objOracleDataReader, 
                                            string Nombre, 
                                            string ValorDefecto)
        {
            var t = objOracleDataReader[Nombre].GetType();

            //return objOracleDataReader[Nombre] == DBNull.Value ? (ValorDefecto == null ? null : ValorDefecto) : objOracleDataReader[Nombre].ToString().Trim();
            return ((objOracleDataReader[Nombre] == DBNull.Value) ? ValorDefecto : objOracleDataReader[Nombre].ToString().Trim());
        }
    }
}
