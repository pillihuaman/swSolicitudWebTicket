using System.Data;

using Oracle.DataAccess.Client;

using AccesoDatos.NMOracle;

namespace AccesoDatos
{
    public class cdPlanilla
    {
        public Planilla CodigoPlanilla(string strUsuWebLogin)
        {
            Planilla objPlanillaRS = null;
            var nmOracle = new Conexion();
            OracleConnection connection;

            using (connection = new OracleConnection(nmOracle.strCadena))
            {
                if (nmOracle.Connect(connection, false))
                {
                    nmOracle.SP_Command(nmOracle.Esquema + ".PKG_PERSONAL.SP_PERS_OBTIENE_X_LOGIN", nmOracle.strStoredProcedure);
                    nmOracle.AgregarParametro("pVarLogin_in", strUsuWebLogin.ToString(), OracleDbType.Varchar2, ParameterDirection.Input);
                    nmOracle.AgregarParametro("pCurResult_out", null, OracleDbType.RefCursor, ParameterDirection.Output);

                    var datareade = nmOracle._ExecuteReader(false);

                    if (datareade.HasRows)
                    {
                        while (datareade.Read())
                        {
                            objPlanillaRS = new Planilla
                            {
                                strIdEmpresaPlanilla = nmOracle.LeeColumnasDataReader(datareade, "ID_EMPRESA_PLANILLA", "-1"),
                                strIdCodigoPlanilla = nmOracle.LeeColumnasDataReader(datareade, "ID_CODIGO_PLANILLA", "-1")
                            };
                        }
                    }
                }
            }

            return objPlanillaRS;
        }
    }
}
