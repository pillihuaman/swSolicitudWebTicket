using System.Data;

using Oracle.DataAccess.Client;

namespace AccesoDatos.NMOracle
{
    public partial class Conexion
    {
        public bool Connect(OracleConnection objOracleConnection, 
                            bool bolTransaction)
        {
            var bolRespuesta = false;

            if (objOracleConnection.State == ConnectionState.Closed)
            {
                objOracleConnection.Open();
                bolRespuesta = true;

                if (bolTransaction)
                {
                    objOracleTransaction = objOracleConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                    objOracleCommand.Connection = objOracleTransaction.Connection;

                }
                else
                {
                    objOracleCommand.Connection = objOracleConnection;
                }
            }

            return bolRespuesta;
        }

        public bool Connect(OracleConnection objOracleConnection)
        {
            var bolRespuesta = false;

            if (objOracleConnection.State == ConnectionState.Closed)
            {
                objOracleConnection.Open();
                bolRespuesta = true;
            }

            return bolRespuesta;
        }

        public void Transaction(OracleTransaction objOracleTransaction)
        {
            objOracleCommand.Connection = objOracleTransaction.Connection;
        }
    }
}
