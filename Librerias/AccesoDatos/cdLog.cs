using System.Data;
using System.Text;

using Oracle.DataAccess.Client;

using AccesoDatos.NMOracle;

namespace AccesoDatos
{
    public partial class cdSolicitudesWebTicket
    {
        public void Inserta(int pIntIdUsuario, 
                            string pStrNomPagina, 
                            string pStrComment, 
                            int pIntIdLang, 
                            int pIntIdWeb, 
                            StringBuilder objSBQuery, 
                            OracleConnection connection, 
                            OracleTransaction objTx)
        { 
            var nmOracle = new Conexion();

            nmOracle.SP_Command(nmOracle.Esquema + ".PKG_LOG.SP_LOG_INSERTA", nmOracle.strStoredProcedure);

            objSBQuery.Append(")");
            
            nmOracle.AgregarParametro("pNumIdUsuario_in", pIntIdUsuario,OracleDbType.Int64, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarQuery_in", objSBQuery.ToString(), OracleDbType.Clob, ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarNomPagina_in",pStrNomPagina, OracleDbType.Varchar2,  ParameterDirection.Input);
            nmOracle.AgregarParametro("pVarComment_in", pStrComment,OracleDbType.Varchar2, ParameterDirection.Input);
            nmOracle.AgregarParametro("pNumIdLang_in", pIntIdLang, OracleDbType.Int64, ParameterDirection.Input);
            nmOracle.AgregarParametro("pNumIdWeb_in", pIntIdWeb, OracleDbType.Int64, ParameterDirection.Input);

            nmOracle.Transaction(objTx);
            nmOracle._ExecuteNonQuery(false);
        }
    }
}
